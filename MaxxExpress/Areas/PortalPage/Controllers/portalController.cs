using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Helpers;
using MaxxExpress.Areas.PortalPage.Models.Tools;
using MaxxExpress.Areas.PortalPage.Models;
using System.Data.Entity.Validation;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using System.Data.Entity;


namespace MaxxExpress.Areas.PortalPage.Controllers
{

    public class PortalController : Controller
    {
        public IEnumerable<tbl_web_notes> GetWebNotes(string pCurrent_User)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                IEnumerable<tbl_web_notes> notesList = db_data.tbl_web_notes.ToList().Where(b => b.Active_IND == "1").Where(d => d.Archive_IND == "0").Where(c => c.SentTo == pCurrent_User).OrderBy(c => c.CreateDate);
                return notesList;
            }
        }
        public IEnumerable<tbl_web_notes> GetWebAlerts(string pCurrent_User)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                DateTime today = DateTime.Today;

                IEnumerable<tbl_web_notes> notesList = db_data.tbl_web_notes.ToList()
                    .Where(b => b.Active_IND == "1")
                    .Where(x => x.Alert_IND == "1")
                    .Where(d => Convert.ToDateTime(d.Alert_Start_Dt) <= today)
                    .Where(d => Convert.ToDateTime(d.Alert_End_Dt) >= today)
                    .Where(c => c.SentTo == pCurrent_User)
                    .OrderBy(c => c.CreateDate);
                return notesList;
            }
        }

        public IEnumerable<vw_loads_summary_open> GetOpenLoadsSummary()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();

                if (access_lvl.ToLower() == "driver")
                {
                    IEnumerable<vw_loads_summary_open> openLoadsSummaryList_Driver = db_data.vw_loads_summary_open.Where(b => b.Status.ToString() == "Delivered").Where(a => a.Driver_Nbr.ToString().Equals(p_id)).ToList().OrderBy(a => a.Load_Date).ThenBy(a => int.Parse(a.Load_of_Day_Driver));
                    return openLoadsSummaryList_Driver;
                }
                else // (access_lvl == "admin")
                {
                    IEnumerable<vw_loads_summary_open> openLoadsSummaryList = db_data.vw_loads_summary_open.ToList().OrderBy(a => a.Load_Date).ThenBy(a => int.Parse(a.Load_of_Day_Total));
                    return openLoadsSummaryList;
                }
            }
        }

        public IEnumerable<vw_loads_detail_open> GetOpenLoadsDetail(string LoadNbr)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();

                if (access_lvl.ToLower() == "driver")
                {
                    IEnumerable<vw_loads_detail_open> openLoadDetailList_Driver = db_data.vw_loads_detail_open.Where(b => b.Driver_Nbr.ToString().Equals(p_id)).Where(a => a.Load_Number.ToString().Equals(LoadNbr)).ToList();
                    return openLoadDetailList_Driver;
                }
                else // (access_lvl == "admin")
                {
                    IEnumerable<vw_loads_detail_open> openLoadDetailList = db_data.vw_loads_detail_open.Where(a => a.Load_Number.ToString().Equals(LoadNbr)).ToList();
                    return openLoadDetailList;
                }
            }
        }
        public IEnumerable<vw_loads_summary_closed> GetClosedLoadsSummary(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();

                DateTime dtFrom = Convert.ToDateTime(pViewDate);

                if (access_lvl.ToLower() == "driver")
                {
                    IEnumerable<vw_loads_summary_closed> closedLoadsSummaryList_Driver = db_data.vw_loads_summary_closed.Where(a => a.Driver_Nbr.ToString().Equals(p_id)).ToList();
                    return closedLoadsSummaryList_Driver;
                }
                else // (access_lvl == "admin")
                {

                    IEnumerable<vw_loads_summary_closed> closedLoadsSummaryList = db_data.vw_loads_summary_closed.ToList().Where(c => c.Pickup_Dt_Full >= dtFrom).OrderBy(c => c.Week_of).ThenBy(a => int.Parse(a.Load_of_Week_Total));
                    return closedLoadsSummaryList;
                }
            }
        }
        public IEnumerable<vw_loads_detail_closed> GetClosedLoadsDetail(string LoadNbr)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();

                if (access_lvl.ToLower() == "driver")
                {
                    IEnumerable<vw_loads_detail_closed> closedLoadDetailList = db_data.vw_loads_detail_closed.Where(b => b.Driver_Nbr.ToString().Equals(p_id)).Where(a => a.Load_Number.ToString().Equals(LoadNbr)).ToList();
                    return closedLoadDetailList;
                }
                else // (access_lvl == "Admin")
                {
                    IEnumerable<vw_loads_detail_closed> closedLoadDetailList = db_data.vw_loads_detail_closed.Where(a => a.Load_Number.ToString().Equals(LoadNbr)).ToList();
                    return closedLoadDetailList;
                }
            }
        }

        public IEnumerable<tbl_fct_payroll_adjustments> GetPayrollAdjusments()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                //Select Single Values for DD (Selected Values) - Transfrom Customer Name to get Costumer ID
                var currentPayDay = (from pPeriod in db_data.vw_payroll_period_summary
                                     select pPeriod.Pay_Day).SingleOrDefault();


                IEnumerable<tbl_fct_payroll_adjustments> payrollAdjustmentList = db_data.tbl_fct_payroll_adjustments.Where(a => a.Pay_Day.Equals(currentPayDay)).ToList();
                return payrollAdjustmentList;

            }
        }

        public IEnumerable<vw_payroll_period_summary> GetPayrollPeriod()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                IEnumerable<vw_payroll_period_summary> payrollPeriodList = db_data.vw_payroll_period_summary.ToList();
                return payrollPeriodList;
            }
        }


        public IEnumerable<vw_payroll_web> GetPayrollSummary(string pViewDate, string pPerson)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                string employee = null;

                if (pPerson == null || pPerson == "null")
                {
                    employee = "All";
                }

                DateTime dtFrom = Convert.ToDateTime(pViewDate);

                if (employee == "All" || pPerson == "All")
                {

                    IEnumerable<vw_payroll_web> payrollSummaryList = db_data.vw_payroll_web.Where(c => c.Pay_Day >= dtFrom).OrderBy(c => c.Pay_Day).ThenBy(c => c.Employee).ToList();
                    return payrollSummaryList;
                }
                else
                {
                    IEnumerable<vw_payroll_web> payrollSummaryList = db_data.vw_payroll_web.Where(c => c.Pay_Day >= dtFrom).Where(c => c.Employee.ToString().Equals(pPerson)).OrderBy(c => c.Pay_Day).ThenBy(c => c.Employee).ToList();
                    return payrollSummaryList;

                }
            }
        }

        public IEnumerable<vw_fuel> GetFuelData(string pViewDate, string pPerson, bool pRange, string pRangeEndDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                string employee = null;

                if (pPerson == null || pPerson == "null")
                {
                    employee = "All";
                }

                DateTime dtFrom = Convert.ToDateTime(pViewDate);

                if (employee == "All" || pPerson == "All")
                {
                    if (pRange)
                    {
                        DateTime dtTo = Convert.ToDateTime(pRangeEndDate);

                        IEnumerable<vw_fuel> fuelSummaryList = db_data.vw_fuel.Where(c => c.Trans_Date >= dtFrom).Where(g => g.Trans_Date <= dtTo).OrderByDescending(c => c.Trans_Date).ThenBy(c => c.Name).ToList();
                        return fuelSummaryList;
                    }
                    else
                    {

                        IEnumerable<vw_fuel> fuelSummaryList = db_data.vw_fuel.Where(c => c.Trans_Date >= dtFrom).OrderByDescending(c => c.Trans_Date).ThenBy(c => c.Name).ToList();
                        return fuelSummaryList;
                    }
                }
                else
                {
                    if (pRange)
                    {
                        DateTime dtTo = Convert.ToDateTime(pRangeEndDate);
                        IEnumerable<vw_fuel> fuelSummaryList = db_data.vw_fuel.Where(c => c.Trans_Date >= dtFrom).Where(g => g.Trans_Date <= dtTo).Where(c => c.Name.ToString().Equals(pPerson)).OrderByDescending(c => c.Trans_Date).ThenBy(c => c.Name).ToList();
                        return fuelSummaryList;
                    }
                    else
                    {
                        IEnumerable<vw_fuel> fuelSummaryList = db_data.vw_fuel.Where(c => c.Trans_Date >= dtFrom).Where(c => c.Name.ToString().Equals(pPerson)).OrderByDescending(c => c.Trans_Date).ThenBy(c => c.Name).ToList();
                        return fuelSummaryList;
                    }


                }
            }
        }

        public IEnumerable<vw_expenses> GetExpenseData(string pViewDate, string pExpenseType, bool pRange, string pRangeEndDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                string expenseType = null;

                if (pExpenseType == null || pExpenseType == "null")
                {
                    expenseType = "All";
                }

                DateTime dtFrom = Convert.ToDateTime(pViewDate);


                if (expenseType == "All" || pExpenseType == "All")
                {
                    if (pRange)
                    {
                        DateTime dtTo = Convert.ToDateTime(pRangeEndDate);

                        IEnumerable<vw_expenses> expenseSummaryList = db_data.vw_expenses.Where(c => c.DateValue >= dtFrom).Where(g => g.DateValue <= dtTo).OrderByDescending(c => c.DateValue).ThenBy(c => c.Authorized_By).ToList();
                        return expenseSummaryList;
                    }
                    else
                    {
                        IEnumerable<vw_expenses> expenseSummaryList = db_data.vw_expenses.Where(c => c.DateValue >= dtFrom).OrderByDescending(c => c.DateValue).ThenBy(c => c.Authorized_By).ToList();
                        return expenseSummaryList;
                    }
                }
                else
                {
                    if (pRange)
                    {
                        DateTime dtTo = Convert.ToDateTime(pRangeEndDate);

                        IEnumerable<vw_expenses> expenseSummaryList = db_data.vw_expenses.Where(c => c.DateValue >= dtFrom).Where(g => g.DateValue <= dtTo).Where(c => c.Reoccurring_IND.ToString().Equals(pExpenseType)).OrderByDescending(c => c.DateValue).ThenBy(c => c.Authorized_By).ToList();
                        return expenseSummaryList;
                    }
                    else
                    {
                        IEnumerable<vw_expenses> expenseSummaryList = db_data.vw_expenses.Where(c => c.DateValue >= dtFrom).Where(c => c.Reoccurring_IND.ToString().Equals(pExpenseType)).OrderByDescending(c => c.DateValue).ThenBy(c => c.Authorized_By).ToList();
                        return expenseSummaryList;
                    }

                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> GetFinancialsExpenses_RO_Detail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> ExpensesDetailData = db_data.FN_Financial_Expense_Reoccuring(period);
                    IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> ExpensesData = db_data.tbl_web_monthly_financial_expenses_reoccurring.ToList();

                    return ExpensesData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> ExpensesDetailData = db_data.FN_Financial_Expense_Reoccuring(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> ExpensesData = db_data.tbl_web_monthly_financial_expenses_reoccurring.ToList();

                    return ExpensesData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_expenses> GetFinancialsExpensesDetail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_expenses> ExpensesDetailData = db_data.FN_Financial_Expenses(period);
                    IEnumerable<tbl_web_monthly_financial_expenses> ExpensesData = db_data.tbl_web_monthly_financial_expenses.OrderBy(x => x.Trans_Date).ToList();

                    return ExpensesData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_expenses> ExpensesDetailData = db_data.FN_Financial_Expenses(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_expenses> ExpensesData = db_data.tbl_web_monthly_financial_expenses.OrderBy(x => x.Trans_Date).ToList();

                    return ExpensesData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_payroll> GetFinancialsPayrollDetail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_payroll> PayrollDetailData = db_data.FN_Financial_Payroll(period);
                    IEnumerable<tbl_web_monthly_financial_payroll> PayrollData = db_data.tbl_web_monthly_financial_payroll.ToList();

                    return PayrollData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_payroll> PayrollDetailData = db_data.FN_Financial_Payroll(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_payroll> PayrollData = db_data.tbl_web_monthly_financial_payroll.ToList();

                    return PayrollData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_fuel> GetFinancialsFuelDetail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_fuel> FuelDetailData = db_data.FN_Financial_Fuel(period);
                    IEnumerable<tbl_web_monthly_financial_fuel> FuelData = db_data.tbl_web_monthly_financial_fuel.ToList();

                    return FuelData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_fuel> FuelDetailData = db_data.FN_Financial_Fuel(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_fuel> FuelData = db_data.tbl_web_monthly_financial_fuel.ToList();

                    return FuelData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_revenue_load> GetFinancialsLoadGrossDetail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_revenue_load> loadGrossData = db_data.FN_Financial_Revenue_Load(period);
                    IEnumerable<tbl_web_monthly_financial_revenue_load> GrossData = db_data.tbl_web_monthly_financial_revenue_load.ToList();

                    return GrossData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_revenue_load> loadGrossData = db_data.FN_Financial_Revenue_Load(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_revenue_load> GrossData = db_data.tbl_web_monthly_financial_revenue_load.ToList();

                    return GrossData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial_revenue_deposit> GetFinancialsDepositDetail(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pViewDate == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    IEnumerable<tbl_web_monthly_financial_revenue_deposit> depositData = db_data.FN_Financial_Revenue_Deposit(period);
                    IEnumerable<tbl_web_monthly_financial_revenue_deposit> getData = db_data.tbl_web_monthly_financial_revenue_deposit.ToList();

                    return getData;
                }
                else
                {
                    IEnumerable<tbl_web_monthly_financial_revenue_deposit> loadGrossData = db_data.FN_Financial_Revenue_Deposit(pViewDate);
                    IEnumerable<tbl_web_monthly_financial_revenue_deposit> getData = db_data.tbl_web_monthly_financial_revenue_deposit.ToList();

                    return getData;
                }
            }
        }

        public IEnumerable<tbl_web_monthly_financial> GetFinancials(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                IEnumerable<tbl_web_monthly_financial> SummaryData = db_data.FN_Financial_PNL(pViewDate);
                IEnumerable<tbl_web_monthly_financial> pnlData = db_data.tbl_web_monthly_financial.ToList();

                return pnlData;

            }
        }

        public IEnumerable<vw_weekly_production> GetWeeklyProductionData(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {


                DateTime dtFrom = Convert.ToDateTime(pViewDate);


                IEnumerable<vw_weekly_production> productionWeeklyList = db_data.vw_weekly_production.OrderByDescending(c => c.Start_Date).ToList().Where(c => Convert.ToDateTime(c.Start_Date) >= dtFrom);
                return productionWeeklyList;


            }
        }

        public IEnumerable<tbl_rpt_weekly_production> GetWeeklyProductionDetailData(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                DateTime dtFrom = Convert.ToDateTime(pViewDate);

                IEnumerable<tbl_rpt_weekly_production> productionWeeklyDetailList = db_data.tbl_rpt_weekly_production.OrderBy(c => c.ID).ToList().Where(c => Convert.ToDateTime(c.Start_Date) >= dtFrom);
                return productionWeeklyDetailList;


            }
        }

        public IEnumerable<vw_weekly_production_gross> GetProductionModal_Gross(string pPeriod, string pEntity)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                if (pPeriod == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_weekly_production_gross select c.ID).Min();
                    var period = (
                                  from tperiod in db_data.vw_weekly_production_gross
                                  where tperiod.ID == maxId
                                  select tperiod.Date_Period
                                  ).SingleOrDefault();

                    IEnumerable<vw_weekly_production_gross> productionGross = db_data.vw_weekly_production_gross.OrderBy(c => c.Delivered_Dt).ToList().Where(c => c.Date_Period == period);
                    return productionGross;

                }
                else
                {
                    if (pEntity == null)
                    {
                        IEnumerable<vw_weekly_production_gross> productionGross = db_data.vw_weekly_production_gross.OrderBy(c => c.Delivered_Dt).ToList().Where(c => c.Date_Period == pPeriod);
                        return productionGross;
                    }
                    else
                    {
                        IEnumerable<vw_weekly_production_gross> productionGross = db_data.vw_weekly_production_gross.OrderBy(c => c.Delivered_Dt).ToList().Where(c => c.Date_Period == pPeriod).Where(p => p.Name == pEntity);
                        return productionGross;
                    }

                }


            }
        }

        public IEnumerable<vw_weekly_production_miles> GetProductionModal_Miles(string pPeriod, string pEntity)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pPeriod == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_weekly_production_miles select c.ID).Min();
                    var period = (
                                  from tperiod in db_data.vw_weekly_production_miles
                                  where tperiod.ID == maxId
                                  select tperiod.Date_Period
                                  ).SingleOrDefault();

                    IEnumerable<vw_weekly_production_miles> productionMiles = db_data.vw_weekly_production_miles.ToList().Where(c => c.Date_Period == period).OrderByDescending(c => Convert.ToInt64(c.ID));
                    return productionMiles;

                }
                else
                {
                    if (pEntity == null)
                    {
                        IEnumerable<vw_weekly_production_miles> productionMiles = db_data.vw_weekly_production_miles.ToList().Where(c => c.Date_Period == pPeriod).OrderByDescending(c => Convert.ToInt64(c.ID));
                        return productionMiles;
                    }
                    else
                    {
                        IEnumerable<vw_weekly_production_miles> productionMiles = db_data.vw_weekly_production_miles.ToList().Where(c => c.Date_Period == pPeriod).Where(p => p.Driver_Name == pEntity).OrderByDescending(c => Convert.ToInt64(c.ID));
                        return productionMiles;
                    }
                }
            }
        }

        public IEnumerable<vw_weekly_production_fuel> GetProductionModal_Fuel(string pPeriod, string pEntity)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pPeriod == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_weekly_production_fuel select c.ID).Min();
                    var period = (
                                  from tperiod in db_data.vw_weekly_production_fuel
                                  where tperiod.ID == maxId
                                  select tperiod.Date_Period
                                  ).SingleOrDefault();

                    IEnumerable<vw_weekly_production_fuel> productionFuel = db_data.vw_weekly_production_fuel.ToList().Where(c => c.Date_Period == period).OrderByDescending(c => Convert.ToInt64(c.ID));
                    return productionFuel;

                }
                else
                {
                    if (pEntity == null)
                    {
                        IEnumerable<vw_weekly_production_fuel> productionFuel = db_data.vw_weekly_production_fuel.ToList().Where(c => c.Date_Period == pPeriod).OrderByDescending(c => Convert.ToInt64(c.ID));
                        return productionFuel;
                    }
                    else
                    {
                        IEnumerable<vw_weekly_production_fuel> productionFuel = db_data.vw_weekly_production_fuel.ToList().Where(c => c.Date_Period == pPeriod).Where(p => p.Name == pEntity).OrderByDescending(c => Convert.ToInt64(c.ID));
                        return productionFuel;
                    }
                }
            }
        }

        public IEnumerable<vw_weekly_production_payroll> GetProductionModal_Payroll(string pPeriod)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (pPeriod == null)
                {
                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_weekly_production_payroll select c.ID).Min();
                    var period = (
                                  from tperiod in db_data.vw_weekly_production_payroll
                                  where tperiod.ID == maxId
                                  select tperiod.Date_Period
                                  ).SingleOrDefault();

                    IEnumerable<vw_weekly_production_payroll> productionPayroll = db_data.vw_weekly_production_payroll.OrderBy(c => c.ID).ToList().Where(c => c.Date_Period == period);
                    return productionPayroll;

                }
                else
                {
                    IEnumerable<vw_weekly_production_payroll> productionPayroll = db_data.vw_weekly_production_payroll.OrderBy(c => c.ID).ToList().Where(c => c.Date_Period == pPeriod);
                    return productionPayroll;
                }
            }
        }



        public string getHomepageWeekStats(string StartDate, string EndDate, string type)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                if (StartDate == null && EndDate == null)
                { // Return Last Weeks Data by Default
                    var p_start_date = (from tdate in db_data.vw_date_last_week

                                        select tdate.Week_of).SingleOrDefault();

                    var p_end_date = (from tdate in db_data.vw_date_last_week

                                      select tdate.Week_Ending).SingleOrDefault();

                    IEnumerable<tbl_web_homepage_stats_week> homestats = db_data.FN_Homepage_week_stats(p_start_date, p_end_date);

                    var p_value = (from tdata in db_data.vw_web_homepage_stats_week
                                   where tdata.Type.Equals(type)
                                   select tdata.Amount).SingleOrDefault();

                    return p_value;
                }
                else
                {

                    IEnumerable<tbl_web_homepage_stats_week> homestats_custom = db_data.FN_Homepage_week_stats(StartDate, EndDate);
                    return "no data";
                }
            }
        }

        public IEnumerable<vw_sand_payout> GetSandPayout_Data(int ID)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                IEnumerable<vw_sand_payout> sandPayoutList = db_data.vw_sand_payout.Where(a => a.ID == ID).ToList();
                return sandPayoutList;
            }
        }


        public string getMaxDayLoadCount_Data(string dateVal)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                int count = (from x in db_data.vw_loads_detail_open where x.Load_Date == dateVal select x).Count();

                count = count + 1;

                return count.ToString();
            }
        }

        public IEnumerable<tbl_fct_fuel_entry> GetFuelPurchases_Data()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();

                if (access_lvl.ToLower() == "driver")
                {
                    IEnumerable<tbl_fct_fuel_entry> fuelPurchasesList_Driver = db_data.tbl_fct_fuel_entry.Where(a => a.Driver_Nbr.ToString().Equals(p_id)).ToList().Where(c => c.Fuel_Paid_Ind.ToString().Equals("0")).OrderBy(a => a.Fuel_Date);
                    return fuelPurchasesList_Driver;
                }
                else // (access_lvl == "admin")
                {
                    IEnumerable<tbl_fct_fuel_entry> fuelPurchasesList = db_data.tbl_fct_fuel_entry.ToList().Where(c => c.Fuel_Paid_Ind.ToString().Equals("0")).OrderBy(a => a.Fuel_Date).ThenBy(a => int.Parse(a.Driver_Nbr));
                    return fuelPurchasesList;
                }
            }
        }

        [NoCache]
        public ActionResult GetIndexStats_Load()
        {

            //Homepage Stats
            ViewData["HomepageStats_DeliveredNotPaidDollars"] = getHomepageWeekStats(null, null, "Delivered - Not Paid Amount");
            ViewData["HomepageStats_DeliveredNotPaidCount"] = getHomepageWeekStats(null, null, "Delivered - Not Paid Count");
            //ViewData["HomepageStats_DeliveredGross"] = getHomepageWeekStats(null, null, "Delivered - Gross");
            //ViewData["HomepageStats_MilesDrivenLastWeek"] = getHomepageWeekStats(null, null, "Miles Driven");
            //ViewData["HomepageStats_FuelDollarsLastWeek"] = getHomepageWeekStats(null, null, "Fuel Dollars Used");
            //ViewData["HomepageStats_PayrollLastWeek"] = getHomepageWeekStats(null, null, "Payroll");
            //ViewData["HomepageStats_ExpensesLastWeek"] = getHomepageWeekStats(null, null, "Expenses");

            //End Homepage Stats


            return PartialView("_LoadStats_Load", ViewData);

        }

        [NoCache]
        public ActionResult GetIndexStats_Fuel()
        {

            //Homepage Stats 
            ViewData["HomepageStats_FuelDollarsOwed"] = getHomepageWeekStats(null, null, "Fuel Dollars Used");
            //End Homepage Stats


            return PartialView("_LoadStats_Fuel", ViewData);

        }

        [NoCache]
        public ActionResult GetOpenLoadSummaryIndex()
        {
            string access_lvl = Session["LogUserAccess"].ToString();

            ViewData["OpenLoadsSummary"] = GetOpenLoadsSummary();

            if (access_lvl.ToLower() == "driver")
            {
                return PartialView("_LoadSummaryOpen_Index_Driver", ViewData);
            }
            else //if (access_lvl == "Admin")
            {
                return PartialView("_LoadSummaryOpen_Index", ViewData);
            }
        }

        [NoCache]
        public ActionResult GetFuelPurchases()
        {
            string access_lvl = Session["LogUserAccess"].ToString();

            ViewData["FuelPurchases"] = GetFuelPurchases_Data();

            if (access_lvl.ToLower() == "driver")
            {
                return PartialView("_FuelPurchase_Index", ViewData);
            }
            else //if (access_lvl == "Admin")
            {
                return PartialView("_FuelPurchase_Index", ViewData);
            }
        }

        [NoCache]
        public ActionResult GetFuelPurchasesList()
        {
            string access_lvl = Session["LogUserAccess"].ToString();

            ViewData["FuelPurchasesList"] = GetFuelPurchases_Data();

            if (access_lvl.ToLower() == "driver")
            {
                return PartialView("_FuelEditList", ViewData);
            }
            else //if (access_lvl == "Admin")
            {
                return PartialView("_FuelEditList", ViewData);
            }
        }

        [NoCache]
        public ActionResult GetClosedLoadSummaryIndex(string pViewDate)
        {
            string access_lvl = Session["LogUserAccess"].ToString();

            DateTime date = DateTime.Now;
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Days_7 = date.AddDays(-7).ToString("MM/dd/yyyy");
            var Days_All = "01/01/2018";
            string final_date = null;

            if (pViewDate == "ALL")
            {
                final_date = Days_All;
            }
            if (pViewDate == "7Days")
            {
                final_date = Days_7;
            }
            if (pViewDate == "30Days" || pViewDate == null)
            {
                final_date = Days_30;
            }

            ViewData["ClosedLoadsSummary"] = GetClosedLoadsSummary(final_date);

            if (access_lvl.ToLower() == "driver")
            {
                return PartialView("_LoadSummaryClosed_Index_Driver", ViewData);
            }
            else //if (access_lvl == "Admin")
            {
                return PartialView("_LoadSummaryClosed_Index", ViewData);
            }
        }

        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult EditNote(string NoteID, string EditType)
        {

            System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                try
                {
                    tbl_web_notes note = db_data.tbl_web_notes.FirstOrDefault(x => x.ID.ToString() == NoteID.ToString());

                    if (note != null)
                    {
                        if (EditType == "ArchiveNote")
                        {
                            note.Archive_IND = "1";
                            note.Alert_IND = "0";
                        }

                        else if (EditType == "DeleteNote")
                        {
                            note.Active_IND = "0";
                            note.Alert_IND = "0";
                        }
                        else if (EditType == "DeleteAlert")
                        {
                            note.Alert_IND = "0";
                        }
                        else
                        {
                            note.Alert_IND = "0";
                        }

                    }

                    db_data.SaveChanges();

                    return Json(new { success = true, responseText = "" + EditType + " was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);
                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult CreateNote([FromBody] tbl_web_notes note)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();
                PortalViewModel addLoadVM = new PortalViewModel();

                try
                {
                    db_data.tbl_web_notes.Add(note);
                    db_data.SaveChanges();

                    return Json(new { success = true, responseText = "Create note was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult uploadNoteAttachment(string dateID, string attachmentNBR, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string filename = file.FileName;
                    string extension = Path.GetExtension(file.FileName.Replace(" ", "_"));
                    string path = Path.Combine(Server.MapPath("~/Areas/PortalPage/docs/notes"),
                                           dateID + "_" + filename);

                    file.SaveAs(path);

                    var url = path.Replace("G:\\PleskVhosts\\axxioweb.com\\httpdocs\\maxxexpress", "https://maxx-express.com");
                    var web_url = url.Replace("\\", "/");
                    return Json(new { success = true, responseText = web_url }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Upload Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }

        }

        [NoCache]
        public PartialViewResult CreateNoteIndex()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel createNoteVM = new PortalViewModel();

                //Key Value Select from DB
                var staffList = (from c in db_data.tbl_web_users
                                 where c.Active_Ind == "1"
                                 select new { ID = c.Name, Value = c.Name })
                                    .ToList();

                createNoteVM.vM_CreateNote_SentFrom = Session["LogUserFullName"].ToString();
                createNoteVM.vM_CreateNote_SendTo_List = staffList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });

                return PartialView("_WebCreateNote_Index", createNoteVM);
            }
        }

        [NoCache]
        public ActionResult WebNotesIndex()
        {
            PortalViewModel NoteVM = new PortalViewModel();

            string current_user = Session["LogUserFullName"].ToString();

            ViewData["WebNotes"] = GetWebNotes(current_user);

            return PartialView("_WebNotes_Index", ViewData);

        }
        [NoCache]
        public ActionResult WebNotesCountIndex()
        {
            PortalViewModel NoteVM = new PortalViewModel();

            string current_user = Session["LogUserFullName"].ToString();
            NoteVM.vM_ViewNotes = GetWebNotes(current_user);
            ViewData["NotesCount"] = NoteVM.vM_ViewNotes.Count().ToString();


            return PartialView("_WebNotesCount_Index", ViewData);

        }
        [NoCache]
        public ActionResult WebAlertsIndex()
        {
            string current_user = Session["LogUserFullName"].ToString();

            ViewData["WebAlerts"] = GetWebAlerts(current_user);

            return PartialView("_WebAlerts_Index", ViewData);

        }

        public ActionResult Index()
        {
            var SessData = Session;

            ///Comment START Here for DEV TESTING
            if (Session["LogUserID"] != null)
            {
                ///Comment END Here for DEV TESTING
                ///

                PortalViewModel portalVM = new PortalViewModel();

                ViewData["ClosedLoadsDetail"] = GetClosedLoadsDetail(null);
                ViewData["OpenLoadsDetail"] = GetOpenLoadsDetail(null);
                ViewData["OpenLoadsEdit"] = GetOpenLoadsDetail(null);
                ViewData["OpenLoadsEditList"] = GetOpenLoadsSummary();
                ViewData["OpenLoadsDeleteList"] = GetOpenLoadsSummary();
                ViewData["OpenLoadsStatusList"] = GetOpenLoadsSummary();

                List<PortalViewModel.KeyVal_Object> emptyList = new List<PortalViewModel.KeyVal_Object>();

                //Add Load Modal                 
                portalVM.AddLoad_Weekof_List = emptyList;
                portalVM.AddLoad_Driver_List = emptyList;
                portalVM.AddLoad_Customer_List = emptyList;
                portalVM.AddLoad_DayofWeek_List = emptyList;
                portalVM.AddLoad_Route_List = emptyList;
                portalVM.AddLoad_LoadofWeek_List = emptyList;
                portalVM.AddLoad_Truck_List = emptyList;
                portalVM.AddLoad_Trailer_List = emptyList;
                portalVM.AddLoad_LTL_IND_List = emptyList;
                portalVM.AddLoad_LoadFileName = null;
                portalVM.AddLoad_Load_Confirmation_Upload = null;
                //End Add Load Modal

                //Edit Load Modal  
                portalVM.EditLoad_Customer_List = emptyList;
                portalVM.EditLoad_Route_List = emptyList;
                portalVM.EditLoad_Driver_List = emptyList;
                portalVM.EditLoad_Truck_List = emptyList;
                portalVM.EditLoad_DayofWeek_List = emptyList;
                portalVM.EditLoad_Trailer_List = emptyList;
                portalVM.EditLoad_LTL_IND_List = emptyList;
                portalVM.EditLoad_LoadofWeek_List = emptyList;
                portalVM.EditLoad_Weekof_List = emptyList;
                portalVM.EditLoad_Customer_ID = null;
                portalVM.EditLoad_Customer_NM = null;
                portalVM.EditLoad_Driver_Nbr = null;
                portalVM.EditLoad_Truck_Nbr = null;
                portalVM.EditLoad_Load_Date = null;
                portalVM.EditLoad_Trailer_Nbr = null;
                portalVM.EditLoad_Load_Confirmation_Upload = null;
                portalVM.EditLoad_Weekof_Current = null;
                portalVM.EditLoad_HasLoad_LoadConfirmation = false;
                portalVM.EditLoad_HasDeliveryConfirmation = false;
                portalVM.EditLoad_MulitpleStops = false;
                //End Edit Load Modal

                //Period in header
                //ViewData["HomepageStats_PeriodLastWeek"] = getHomepageWeekStats(null, null, "Period");

                return View(portalVM);

                ///Comment START Here for DEV TESTING
            }
            else
            {
                return RedirectToAction("Login");
            }
            ///Comment END Here for DEV TESTING
        }


        [NoCache]
        public PartialViewResult GetLoadDetail(string LoadNbr, string LoadState)
        {
            if (LoadState == "open")
            {
                ViewData["OpenLoadsDetail"] = GetOpenLoadsDetail(LoadNbr);
                //return PartialView("_LoadDetailOpenDiv", ViewData);
                return PartialView("_LoadDetailOpenDiv_Sand", ViewData);
            }
            if (LoadState == "closed")
            {
                ViewData["ClosedLoadsDetail"] = GetClosedLoadsDetail(LoadNbr);
                return PartialView("_LoadDetailClosedDiv", ViewData);
            }
            return null;
        }

        [NoCache]
        public PartialViewResult GetAddLoad()
        {
        
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel addLoadVM = new PortalViewModel();

                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();
                IEnumerable<dynamic> DriverList;

                //Single Column Select Example
                IEnumerable<string> Week_List = db_data.vw_date_week_of.OrderBy(x => x.Sort_Order).Select(y => y.Week_of.ToString());

                //Key Value Select from DB
                var CustomerList = (from c in db_data.tbl_dim_customer
                                    where c.Active_Ind == "1"
                                    select new { ID = c.Cust_ID, Value = c.CompanyName })
                                    .ToList();
                var RouteList = (from r in db_data.vw_sand_payout
                                 where r.Active_Ind == "1"
                                 orderby r.Region
                                 select new { ID = r.ID, Value = r.Route_NM })
                                  .ToList();
                if (access_lvl.ToLower() == "driver")
                {

                    DriverList = (from d in db_data.tbl_dim_driver
                                  where d.Active_Ind == "1"
                                  where d.Driver_Nbr == p_id
                                  select new { ID = d.Driver_Nbr, Value = d.Name })
                                     .ToList();
                }
                else
                {
                    DriverList = (from d in db_data.tbl_dim_driver
                                  where d.Active_Ind == "1"
                                  select new { ID = d.Driver_Nbr, Value = d.Name })
                                     .ToList();
                }
                var TruckList = (from t in db_data.tbl_dim_truck
                                 select new { ID = t.Truck_Nbr, Value = t.Truck_Nbr })
                                 .ToList();
                var TrailerList = (from tr in db_data.tbl_dim_trailer
                                   where tr.Active_IND == "1"
                                   orderby tr.Sort_Order
                                   select new { ID = tr.Trailer_Nbr, Value = tr.Trailer_Nbr })
                                 .ToList();
                var DayList = (from d in db_data.vw_date_week
                               orderby d.ID descending
                               select new { ID = d.DateValue, Value = d.DateValue })
                                 .ToList();

                //Select Single Values for DD (Selected Values)
                var currentWeekof_val = (from tLoads in db_data.vw_date_week_of
                                         where tLoads.Sort_Order == 0
                                         select tLoads.Week_of).SingleOrDefault();


                var currentloadofWeek_val = db_data.tbl_fct_loads
                                                 .Where(x => x.Week_of == currentWeekof_val)
                                                 .OrderByDescending(x => x.ID)
                                                 .Take(1)
                                                 .Select(x => x.Load_of_Week)
                                                 .ToList()
                                                 .FirstOrDefault();

                var weekDistinct = Week_List.Cast<string>().Distinct().ToList();
                addLoadVM.AddLoad_Weekof_List = weekDistinct.Select(m => new PortalViewModel.KeyVal_Object { Id = m, Value = m });
                addLoadVM.AddLoad_Customer_List = CustomerList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addLoadVM.AddLoad_DayofWeek_List = DayList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addLoadVM.AddLoad_Route_List = RouteList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addLoadVM.AddLoad_Driver_List = DriverList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addLoadVM.AddLoad_Truck_List = TruckList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addLoadVM.AddLoad_Trailer_List = TrailerList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                if (currentloadofWeek_val == null)
                {
                    addLoadVM.AddLoad_LoadofWeek_Val = "1";
                }
                else
                {
                    addLoadVM.AddLoad_LoadofWeek_Val = (Convert.ToInt32(currentloadofWeek_val) + 1).ToString();
                }

                //return PartialView("_LoadAddDiv", addLoadVM);
                return PartialView("_LoadAddDiv_Sand", addLoadVM);
            }
        }

        [NoCache]
        public PartialViewResult GetAddFuel()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel addFuelVM = new PortalViewModel();

                string access_lvl = Session["LogUserAccess"].ToString();
                string p_id = Session["LogUserUserTypeID"].ToString();
                IEnumerable<dynamic> DriverList;

                //Single Column Select Example
                IEnumerable<string> Week_List = db_data.vw_date_week_of.OrderBy(x => x.Sort_Order).Select(y => y.Week_of.ToString());

                //Key Value Select from DB
                if (access_lvl.ToLower() == "driver")
                {

                    DriverList = (from d in db_data.tbl_dim_driver
                                  where d.Active_Ind == "1"
                                  where d.Driver_Nbr == p_id
                                  select new { ID = d.Driver_Nbr, Value = d.Name })
                                     .ToList();
                }
                else
                {
                    DriverList = (from d in db_data.tbl_dim_driver
                                  where d.Active_Ind == "1"
                                  select new { ID = d.Driver_Nbr, Value = d.Name })
                                     .ToList();
                }
                var TruckList = (from t in db_data.tbl_dim_truck
                                 select new { ID = t.Truck_Nbr, Value = t.Truck_Nbr })
                                 .ToList();
                var DayList = (from d in db_data.vw_date_week
                               orderby d.ID descending
                               select new { ID = d.DateValue, Value = d.DateValue })
                                 .ToList();


                var weekDistinct = Week_List.Cast<string>().Distinct().ToList();
                addFuelVM.AddFuel_DayofWeek_List = DayList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addFuelVM.AddFuel_Driver_List = DriverList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                addFuelVM.AddFuel_Truck_List = TruckList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });


                return PartialView("_FuelAddDiv", addFuelVM);
            }
        }

        public JsonResult getSandPayout(int ID)
        {
            var sandData = GetSandPayout_Data(ID);
            return Json(sandData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMaxDayLoadCount(string date_Val)
        {
            var jsonData = getMaxDayLoadCount_Data(date_Val);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [NoCache]
        public PartialViewResult GetLoadEditDetail(string LoadNbr)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel editLoadVM = new PortalViewModel();

                //Select All Values for DD List 
                var CustomerList = (from c in db_data.tbl_dim_customer
                                    select new { ID = c.Cust_ID, Value = c.CompanyName })
                                   .ToList();
                var RouteList = (from r in db_data.vw_sand_payout
                                 where r.Active_Ind == "1"
                                 orderby r.Region
                                 select new { ID = r.ID, Value = r.Route_NM })
                                  .ToList();
                var DriverList = (from c in db_data.tbl_dim_driver
                                  where c.Active_Ind == "1"
                                  select new { ID = c.Driver_Nbr, Value = c.Name })
                                   .ToList();
                var TruckList = (from c in db_data.tbl_dim_truck
                                 select new { ID = c.Truck_Nbr, Value = c.Truck_Nbr })
                                   .ToList();
                var TrailerList = (from c in db_data.tbl_dim_trailer
                                   where c.Active_IND == "1"
                                   orderby c.Sort_Order
                                   select new { ID = c.Trailer_Nbr, Value = c.Trailer_Nbr })
                                   .ToList();
                var DayList = (from d in db_data.vw_date_week
                               orderby d.ID descending
                               select new { ID = d.DateValue, Value = d.DateValue })
                 .ToList();

                //Select Single Values for DD (Selected Values) - Transfrom Customer Name to get Costumer ID
                var custName = (from tLoads in db_data.vw_loads_detail_open
                                where tLoads.Load_Number.Equals(LoadNbr)
                                select tLoads.Customer_Nm).SingleOrDefault();

                var cID = (from customer in db_data.tbl_dim_customer
                           where customer.CompanyName.Equals(custName)
                           select customer.Cust_ID).SingleOrDefault();

                var driverName = (from tLoads in db_data.vw_loads_detail_open
                                  where tLoads.Load_Number.Equals(LoadNbr)
                                  select tLoads.Driver).SingleOrDefault();

                var dID = (from driver in db_data.tbl_dim_driver
                           where driver.Name.Equals(driverName)
                           select driver.Driver_Nbr).SingleOrDefault();

                var truckName = (from tloads in db_data.vw_loads_detail_open
                                 where tloads.Load_Number.Equals(LoadNbr)
                                 select tloads.Truck).SingleOrDefault();

                var trailerName = (from tloads in db_data.vw_loads_detail_open
                                   where tloads.Load_Number.Equals(LoadNbr)
                                   select tloads.Trailer).SingleOrDefault();

                var tID = (from trucks in db_data.tbl_dim_truck
                           where trucks.Truck_Nbr.Equals(truckName)
                           select trucks.Truck_Nbr).SingleOrDefault();

                var ldID = (from tloads in db_data.vw_loads_detail_open
                            where tloads.Load_Number.Equals(LoadNbr)
                            select tloads.Load_Date).SingleOrDefault();

                var hasConfirmationdoc = (from tLoads in db_data.vw_loads_detail_open
                                          where tLoads.Load_Number.Equals(LoadNbr)
                                          select tLoads.Load_Confirmation_URL).SingleOrDefault();

                var hasDeliverydoc = (from tLoads in db_data.vw_loads_detail_open
                                      where tLoads.Load_Number.Equals(LoadNbr)
                                      select tLoads.BOL_URL).SingleOrDefault();

                var hasMultipleStops = (from tLoads in db_data.vw_loads_detail_open
                                        where tLoads.Load_Number.Equals(LoadNbr)
                                        select tLoads.Dropoff_2_Location).SingleOrDefault();

                // Get List of Weeks for DD
                IEnumerable<string> Week_List = db_data.vw_date_week_of.OrderBy(x => x.Sort_Order).Select(y => y.Week_of.ToString());

                //Get Single Week of for Selected Loads
                var weekText = (from data in db_data.vw_loads_detail_open
                                where data.Load_Number.Equals(LoadNbr)
                                select data.Week_of).SingleOrDefault();

                ///Set Values
                editLoadVM.EditLoad_Customer_ID = cID.ToString();
                editLoadVM.EditLoad_Driver_Nbr = dID;
                editLoadVM.EditLoad_Truck_Nbr = tID;
                editLoadVM.EditLoad_Load_Date = ldID;
                editLoadVM.EditLoad_Trailer_Nbr = trailerName.ToString();
                editLoadVM.EditLoad_HasLoad_LoadConfirmation = hasConfirmationdoc == null ? false : true;
                editLoadVM.EditLoad_HasDeliveryConfirmation = hasDeliverydoc == null ? false : true;
                editLoadVM.EditLoad_MulitpleStops = hasMultipleStops == null ? false : true;
                editLoadVM.EditLoad_Customer_List = CustomerList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.EditLoad_Driver_List = DriverList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.EditLoad_Route_List = RouteList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.EditLoad_Truck_List = TruckList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.EditLoad_Trailer_List = TrailerList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.EditLoad_Weekof_Current = weekText;
                editLoadVM.EditLoad_DayofWeek_List = DayList.Select(m => new PortalViewModel.KeyVal_Object { Id = m.ID.ToString(), Value = m.Value.ToString() });
                editLoadVM.vM_EditLoadData = db_data.vw_loads_detail_open.Single(a => a.Load_Number == LoadNbr);
                var weekDistinct = Week_List.Cast<string>().Distinct().ToList();
                editLoadVM.EditLoad_Weekof_List = weekDistinct.Select(m => new PortalViewModel.KeyVal_Object { Id = m, Value = m });

                //return PartialView("_LoadEditDiv", editLoadVM);
                return PartialView("_LoadEditDiv_Sand", editLoadVM);
            }
        }
        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult AddLoad([FromBody] tbl_fct_loads newload)
        {
            int route_ID;
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();
                PortalViewModel addLoadVM = new PortalViewModel();

                try
                {
                    db_data.tbl_fct_loads.Add(newload);
                    db_data.SaveChanges();

                    route_ID = Int32.Parse(newload.Route_ID);
                    try
                    {
                         SendSandLoad_Email("statusUpdate", newload.Load_Number, newload.Driver_NBR, route_ID, newload.Pickup_Location, newload.Dropoff_1_Location, newload.Load_Confirmation_URL, newload.BOL_URL, newload.BOL_FILE_TYPE, newload.Pickup_DO1_Miles);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.ToString());
                    }

                    return Json(new { success = true, responseText = "Add Load was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult EditLoad(string LoadNbr, [FromBody] tbl_fct_loads load)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();
                PortalViewModel editLoadVM = new PortalViewModel();

                if (editLoadVM.EditLoad_Load_Confirmation_Upload == null || editLoadVM.EditLoad_Load_Confirmation_Upload.ContentLength == 0)
                {
                    ModelState.AddModelError("EditLoad_Load_Confirmation_Upload", "This field is required");
                }

                try
                {
                    tbl_fct_loads loadToUpdate = db_data.tbl_fct_loads.FirstOrDefault(x => x.Load_Number == LoadNbr);

                    if (loadToUpdate == null)
                    {
                        return Json(new { success = false, responseText = "Record Not Found." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string PickupDate;
                        if (load.Pickup_Date.Length <= 5)
                        {
                            PickupDate = load.Pickup_Date + '/' + DateTime.Now.Year.ToString();
                        }
                        else
                        {
                            PickupDate = load.Pickup_Date;
                        }

                        loadToUpdate.Week_of = load.Week_of;
                        loadToUpdate.Load_of_Week = load.Load_of_Week;
                        loadToUpdate.Aux_Load_Number = load.Aux_Load_Number;
                        loadToUpdate.Customer_ID = load.Customer_ID;
                        loadToUpdate.Driver_NBR = load.Driver_NBR;
                        loadToUpdate.Truck_NBR = load.Truck_NBR;
                        loadToUpdate.Trailer_NBR = load.Trailer_NBR;
                        loadToUpdate.Weight = load.Weight;
                        loadToUpdate.Contents = load.Contents;
                        loadToUpdate.LTL = load.LTL;
                        loadToUpdate.Pickup_Location = load.Pickup_Location;
                        loadToUpdate.Pickup_Name = load.Pickup_Name;
                        loadToUpdate.Pickup_Address = load.Pickup_Address;
                        loadToUpdate.Pickup_Zip = load.Pickup_Zip;
                        loadToUpdate.Pickup_Contact = load.Pickup_Contact;
                        loadToUpdate.Pickup_Date = PickupDate;
                        loadToUpdate.Pickup_Time = load.Pickup_Time;
                        loadToUpdate.Pickup_Notes = load.Pickup_Notes;
                        loadToUpdate.Deadhead_Miles = load.Deadhead_Miles;
                        loadToUpdate.Dropoff_1_Location = load.Dropoff_1_Location;
                        loadToUpdate.Dropoff_1_Name = load.Dropoff_1_Name;
                        loadToUpdate.Dropoff_1_Address = load.Dropoff_1_Address;
                        loadToUpdate.Dropoff_1_Zip = load.Dropoff_1_Zip;
                        loadToUpdate.Dropoff_1_Contact = load.Dropoff_1_Contact;
                        loadToUpdate.Dropoff_1_Date = load.Dropoff_1_Date;
                        loadToUpdate.Dropoff_1_Time = load.Dropoff_1_Time;
                        loadToUpdate.Dropoff_1_Notes = load.Dropoff_1_Notes;
                        loadToUpdate.Pickup_DO1_Miles = load.Pickup_DO1_Miles;
                        loadToUpdate.Dropoff_2_Location = load.Dropoff_2_Location;
                        loadToUpdate.Dropoff_2_Name = load.Dropoff_2_Name;
                        loadToUpdate.Dropoff_2_Address = load.Dropoff_2_Address;
                        loadToUpdate.Dropoff_2_Zip = load.Dropoff_2_Zip;
                        loadToUpdate.Dropoff_2_Contact = load.Dropoff_2_Contact;
                        loadToUpdate.Dropoff_2_Date = load.Dropoff_2_Date;
                        loadToUpdate.Dropoff_2_Time = load.Dropoff_2_Time;
                        loadToUpdate.Dropoff_2_Notes = load.Dropoff_2_Notes;
                        loadToUpdate.DO1_DO2_Miles = load.DO1_DO2_Miles;
                        loadToUpdate.Dropoff_3_Location = load.Dropoff_3_Location;
                        loadToUpdate.Dropoff_3_Name = load.Dropoff_3_Name;
                        loadToUpdate.Dropoff_3_Address = load.Dropoff_3_Address;
                        loadToUpdate.Dropoff_3_Zip = load.Dropoff_3_Zip;
                        loadToUpdate.Dropoff_3_Contact = load.Dropoff_3_Contact;
                        loadToUpdate.Dropoff_3_Date = load.Dropoff_3_Date;
                        loadToUpdate.Dropoff_3_Time = load.Dropoff_3_Time;
                        loadToUpdate.Dropoff_3_Notes = load.Dropoff_3_Notes;
                        loadToUpdate.DO2_DO3_Miles = load.DO2_DO3_Miles;
                        loadToUpdate.Dropoff_4_Location = load.Dropoff_4_Location;
                        loadToUpdate.Dropoff_4_Name = load.Dropoff_4_Name;
                        loadToUpdate.Dropoff_4_Address = load.Dropoff_4_Address;
                        loadToUpdate.Dropoff_4_Zip = load.Dropoff_4_Zip;
                        loadToUpdate.Dropoff_4_Contact = load.Dropoff_4_Contact;
                        loadToUpdate.Dropoff_4_Date = load.Dropoff_4_Date;
                        loadToUpdate.Dropoff_4_Time = load.Dropoff_4_Time;
                        loadToUpdate.Dropoff_4_Notes = load.Dropoff_4_Notes;
                        loadToUpdate.DO3_DO4_Miles = load.DO3_DO4_Miles;
                        loadToUpdate.Dropoff_5_Location = load.Dropoff_5_Location;
                        loadToUpdate.Dropoff_5_Name = load.Dropoff_5_Name;
                        loadToUpdate.Dropoff_5_Address = load.Dropoff_5_Address;
                        loadToUpdate.Dropoff_5_Zip = load.Dropoff_5_Zip;
                        loadToUpdate.Dropoff_5_Contact = load.Dropoff_5_Contact;
                        loadToUpdate.Dropoff_5_Date = load.Dropoff_5_Date;
                        loadToUpdate.Dropoff_5_Time = load.Dropoff_5_Time;
                        loadToUpdate.Dropoff_5_Notes = load.Dropoff_5_Notes;
                        loadToUpdate.DO4_DO5_Miles = load.DO4_DO5_Miles;
                        loadToUpdate.Dropoff_6_Location = load.Dropoff_6_Location;
                        loadToUpdate.Dropoff_6_Name = load.Dropoff_6_Name;
                        loadToUpdate.Dropoff_6_Address = load.Dropoff_6_Address;
                        loadToUpdate.Dropoff_6_Zip = load.Dropoff_6_Zip;
                        loadToUpdate.Dropoff_6_Contact = load.Dropoff_6_Contact;
                        loadToUpdate.Dropoff_6_Date = load.Dropoff_6_Date;
                        loadToUpdate.Dropoff_6_Time = load.Dropoff_6_Time;
                        loadToUpdate.Dropoff_6_Notes = load.Dropoff_6_Notes;
                        loadToUpdate.DO5_DO6_Miles = load.DO5_DO6_Miles;
                        loadToUpdate.Dropoff_7_Location = load.Dropoff_7_Location;
                        loadToUpdate.Dropoff_7_Name = load.Dropoff_7_Name;
                        loadToUpdate.Dropoff_7_Address = load.Dropoff_7_Address;
                        loadToUpdate.Dropoff_7_Zip = load.Dropoff_7_Zip;
                        loadToUpdate.Dropoff_7_Contact = load.Dropoff_7_Contact;
                        loadToUpdate.Dropoff_7_Date = load.Dropoff_7_Date;
                        loadToUpdate.Dropoff_7_Time = load.Dropoff_7_Time;
                        loadToUpdate.Dropoff_7_Notes = load.Dropoff_7_Notes;
                        loadToUpdate.DO6_DO7_Miles = load.DO6_DO7_Miles;
                        loadToUpdate.Total_Load_Miles = load.Total_Load_Miles;
                        loadToUpdate.Total_Miles = load.Total_Miles;
                        loadToUpdate.Route_ID = load.Route_ID;
                        loadToUpdate.Updatedby = load.Updatedby;
                        loadToUpdate.Payout = load.Payout;
                        loadToUpdate.Price_Per_Mile = load.Price_Per_Mile;
                        loadToUpdate.Detention_Payout = load.Detention_Payout;
                        loadToUpdate.Lumper_Charge = load.Lumper_Charge;
                        loadToUpdate.Load_Confirmation_URL = load.Load_Confirmation_URL;
                        db_data.SaveChanges();

                        return Json(new { success = true, responseText = "Edit Load was successful" }, JsonRequestBehavior.AllowGet);
                    }
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult AddFuelPurchase([FromBody] tbl_fct_fuel_entry newFuelPurchase)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

                try
                {
                    db_data.tbl_fct_fuel_entry.Add(newFuelPurchase);
                    db_data.SaveChanges();

                    SendFuelPurchaseEmail("fuelPurchase", newFuelPurchase.Driver_Nbr, newFuelPurchase.Driver_Nm, newFuelPurchase.Fuel_Date, newFuelPurchase.Fuel_Amount);

                    return Json(new { success = true, responseText = "Add Load was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult uploadLoadConfirmation(string LoadNbr, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string filename = LoadNbr;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/PortalPage/docs/loads"),
                                               filename + extension);
                    file.SaveAs(path);

                    return Json(new { success = true, responseText = "File Load was successful" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Upload Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }

        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult uploadBOL(string LoadNbr, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string filename = LoadNbr;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/PortalPage/docs/bol"),
                                               filename + extension);

                    file.SaveAs(path);

                    using (cascadiaEntities db_data = new cascadiaEntities())
                    {
                        try
                        {
                            tbl_fct_loads loadToUpdate = db_data.tbl_fct_loads.FirstOrDefault(x => x.Load_Number == LoadNbr);
                            loadToUpdate.BOL_URL = "https://maxx-express.com/Areas/PortalPage/docs/bol/" + filename;
                            loadToUpdate.BOL_FILE_TYPE = extension;
                            db_data.SaveChanges();
                        }

                        catch (DbEntityValidationException e)
                        {
                            var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);
                            return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                        }

                    }

                    return Json(new { success = true, responseText = "BOL upload was successful" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Upload Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }

        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult uploadBOL_Driver(string LoadNbr, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string filename = LoadNbr;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/PortalPage/docs/bol"),
                                               filename + extension);

                    file.SaveAs(path);

                    return Json(new { success = true, responseText = "BOL upload was successful" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Upload Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }

        }

        [System.Web.Mvc.HttpPost]
        [NoCache]
        public ActionResult uploadFuelReceipt(string indentifier, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string filename = indentifier;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/PortalPage/docs/fuelreceipts"),
                                               filename + extension);
                    file.SaveAs(path);

                    return Json(new { success = true, responseText = "File Receipt upload was successful" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "Upload Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }

        }


        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult UpdateLoadStatus(string LoadNbr, string newStatus, string bypassEmailNotification, string BOLattached, string BOLurl, string loadPaidAmount, string detentionAmount)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

                try
                {
                    tbl_fct_load_status loadStatusToUpdate = db_data.tbl_fct_load_status.FirstOrDefault(x => x.Load_Number == LoadNbr);
                    tbl_fct_loads loadToUpdate = db_data.tbl_fct_loads.FirstOrDefault(x => x.Load_Number == LoadNbr);


                    if (loadStatusToUpdate == null)
                    {
                        return Json(new { success = false, responseText = "Record Not Found, sometimes this happens just refresh the browser and it should work when you try again." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (newStatus == "Not Started")
                        {
                            loadStatusToUpdate.Pickup_IND = "0";
                            loadStatusToUpdate.In_Progress_IND = "0";
                            loadStatusToUpdate.Delivered_IND = "0";
                        }
                        if (newStatus == "Picked Up")
                        {
                            loadStatusToUpdate.Pickup_IND = "1";
                            loadStatusToUpdate.In_Progress_IND = "1";
                            loadStatusToUpdate.Delivered_IND = "0";
                            loadStatusToUpdate.Pickup_Status_DNT = DateTime.Now;

                            if (bypassEmailNotification == "no")
                            {
                                SendLoadEmail("statusUpdate", LoadNbr, newStatus);
                            }
                        }
                        if (newStatus == "Delivered" && BOLattached == "yes")
                        {
                            loadStatusToUpdate.Pickup_IND = "1";
                            loadStatusToUpdate.In_Progress_IND = "1";
                            loadStatusToUpdate.Delivered_IND = "1";
                            loadStatusToUpdate.Delivered_Status_DNT = DateTime.Now;
                            loadStatusToUpdate.BOL_SENT_IND = "1";
                            loadStatusToUpdate.BOL_SENT_DNT = DateTime.Now;
                            if (loadToUpdate.BOL_URL == null)
                            {
                                loadToUpdate.BOL_URL = BOLurl;
                            }

                            if (bypassEmailNotification == "no")
                            {
                                SendLoadEmail("statusUpdate", LoadNbr, newStatus);
                            }
                            // Send BOL Submission Email to Admin ie BOL ready to sumbit : TO DO
                        }
                        if (newStatus == "Paid")
                        {
                            if (loadStatusToUpdate.Delivered_IND == "1")
                            {
                                loadStatusToUpdate.Paid_IND = "1";
                                loadToUpdate.Final_Payout = loadPaidAmount;
                                loadToUpdate.Detention_Payout = detentionAmount;
                            }
                        }

                        db_data.SaveChanges();

                        return Json(new { success = true, responseText = "Load Status Update was successful" }, JsonRequestBehavior.AllowGet);
                    }
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.WorkPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public static void SendLoadEmail(string email_type, string loadNBR, string loadStatus)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string Pickup_Name;
                string Pickup_Address;
                string Pickup_City;
                string Pickup_Zip;
                string Pickup_Full_Location;
                string Dropoff_Name;
                string Dropoff_Address;
                string Dropoff_City;
                string Dropoff_Zip;
                string Dropoff_Full_Location;
                int? LTLcheck;
                string BOL_URL = null;
                string BOL_FILE_TYPE = null;
                string BOL_FULL = null;
                string Location;
                string Message;
                string mailBody;
                string to = "maxxexpresstrucks@gmail.com";
                string from = "web@maxx-express.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = email_type == "statusUpdate" ? "Maxx Express Carrier ALERT - Load #" + loadNBR + " STATUS - \"" + loadStatus + "\"" : null;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("maxxexpress-com01b.mail.protection.outlook.com");
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "relay-hosting.secureserver.net";
                client.Credentials = new System.Net.NetworkCredential("web@maxx-express.com", "BlueExpress13!");
                client.EnableSsl = client.Port == 587;

                var db = db_data.vw_loads_detail_open.FirstOrDefault(e => e.Load_Number == loadNBR);

                Pickup_Name = db.Pickup_Name.ToString();
                Pickup_Address = db.Pickup_Address.ToString();
                Pickup_City = db.Pickup_Location.ToString();
                Pickup_Zip = db.Pickup_Zip.ToString();
                Pickup_Full_Location = Pickup_Name + " <br> " + Pickup_Address + " <br> " + Pickup_City + ", " + Pickup_Zip;

                Dropoff_Name = db.Dropoff_1_Name.ToString();
                Dropoff_Address = db.Dropoff_1_Address.ToString();
                Dropoff_City = db.Dropoff_1_Location.ToString();
                Dropoff_Zip = db.Dropoff_1_Zip.ToString();
                try
                {
                    LTLcheck = db.Dropoff_2_Location.Length;
                }
                catch (Exception ex)
                {
                    LTLcheck = null;
                    Console.WriteLine("Exception caught in CreateErrorMessage2(): {0}",
                               ex.ToString());
                }
                if (LTLcheck == null)
                {
                    Dropoff_Full_Location = Dropoff_Name + " <br> " + Dropoff_Address + " <br> " + Dropoff_City + ", " + Dropoff_Zip;
                }
                else
                {
                    Dropoff_Full_Location = "This load had mulitple stops.";
                }

                Location = email_type == "statusUpdate" && loadStatus == "Load Picked Up" ? Pickup_Full_Location : Dropoff_Full_Location;
                Message =
                    email_type == "statusUpdate" && loadStatus == "Picked Up" ? "Load " + loadNBR + " status has been updated to \"" + loadStatus + "\" and is In-Route to the drop off destination." :
                    email_type == "statusUpdate" && loadStatus == "Delivered" ? "Load " + loadNBR + " status has been updated to \"" + loadStatus + "\" and has reached the final drop off destination. The signed BOL confirmation link is above." :
                    null;

                if (loadStatus == "Delivered")
                {

                    try
                    {
                        BOL_URL = db.BOL_URL.ToString();
                        BOL_FILE_TYPE = db.BOL_FILE_TYPE.ToString();
                        BOL_FULL = BOL_URL + BOL_FILE_TYPE;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateErrorMessage2(): {0}", ex.ToString());
                    }

                }

                mailBody =
              "<html>" +
               "<head>" +
                 "<style>" +
                 "@import url(https://fonts.googleapis.com/css?family=Fjalla+One|Oxygen);" +
                 "th, td {padding: 15px;text-align: left; border: 1px solid #ddd;}" +
                 ".bold {font-weight:bold} " +
                 ".left-text {text-align:left} " +
                 ".headers {font-family: 'Fjalla One', sans-serif; font-weight: bold;}" +
                 ".values {font-family: 'Oxygen', sans-serif;}" +
                 ".loadNbr {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".status {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".ldconfirm {font-family: 'Oxygen', sans-serif; font-size: 22px;}" +
                 "</style>" +
              "</head>" +
              "<body>" +
                 "<table style='width: 600px'>" +
                 "<colgroup>" +
                 "<col style='width: 100px'>" +
                 "<col style='width: 300px'>" +
                 "</colgroup>" +
                  " <tr> " +
                      " <th colspan='2'class='headers' style='text-align:left; font-family: 'Fjalla One', sans-serif;'><img src='http://maxx-express.com/images/img-carrier-alert-email-header.png'></th>" +
                   "</tr>" +
                   "<tr>" +
                       " <td class='headers'>Load #</td>" +
                       " <td class='values loadNbr'>" + loadNBR + "</td>" +
                  " </tr>" +
                  " <tr>" +
                       " <td class='headers'>Status</td>" +
                       " <td class='values status'>" + loadStatus + "</td>" +
                  " </tr>";
                if (loadStatus == "Delivered")
                {
                    mailBody = mailBody +
                       " <tr>" +
                       "   <td class='headers'>Delivery Confirmation</td>" +
                       "   <td class='ldconfirm'><a style='padding-top: 10%;' href='" + BOL_FULL + "' /> Load #" + loadNBR + " BOL Confirmation Link" + "</a></td>" +
                    " </tr>" +
                    " <tr>"
                        ;
                };
                mailBody = mailBody +
                  " <tr>" +
                     "   <td class='headers'>Location</td>" +
                     "   <td class='values'>" + Location + "</td>" +
                  " </tr>" +
                  " <tr>" +
                    " <td class='values' colspan='2' rowspan='8'>" +
                    "<span class='headers left-text'>Message: </span>" +
                    "" + Message +
                    "</td>" +
                    " </tr>" +
                  " </table>" +
                 "</body>" +
               "</html>";

                message.Body = mailBody;

                try
                {
                    client.Send(message);
                    updateEmailIndicators(loadNBR, loadStatus);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
            }
        }

        public static void updateEmailIndicators(string loadNBR, string status)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                try
                {
                    tbl_fct_load_status loadStatusToUpdate = db_data.tbl_fct_load_status.FirstOrDefault(x => x.Load_Number == loadNBR);

                    if (status == "Picked Up")
                    {
                        loadStatusToUpdate.In_Progress_MailSent_IND = "1";

                    }
                    if (status == "Delivered")
                    {
                        loadStatusToUpdate.Delivered_MailSent_IND = "1";
                    }
                    if (status == "Paid")
                    {

                    }

                    db_data.SaveChanges();
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);
                }
            }
        }

        public static void SendFuelPurchaseEmail(string email_type, string driverNbr, string driverName, string fuelDate, string fuelAmount)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                string Fuel_Location;
                string Fuel_Amount;
                string Fuel_Gallons;
                string Truck_Nbr;
                string Odometer;
                string Receipt_URL;
                string Message;
                string mailBody;
                string to = "maxxexpresstrucks@gmail.com";
                string from = "web@maxx-express.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = email_type == "fuelPurchase" ? "Fuel Purchase (" + fuelDate + ") - " + driverName + " has entered a fuel purchase for $" + fuelAmount : null;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("maxxexpress-com01b.mail.protection.outlook.com");
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "relay-hosting.secureserver.net";
                client.Credentials = new System.Net.NetworkCredential("web@maxx-express.com", "BlueExpress13!");
                client.EnableSsl = client.Port == 587;

                var db = db_data.tbl_fct_fuel_entry.FirstOrDefault(e => e.Driver_Nbr == driverNbr && e.Fuel_Date == fuelDate);

                Fuel_Location = db.Fuel_Location.ToString();
                Fuel_Amount = db.Fuel_Amount.ToString();
                Fuel_Gallons = db.Fuel_Gallons.ToString();
                Truck_Nbr = db.Truck_Nbr.ToString();
                Odometer = db.Odometer.ToString();
                Receipt_URL = db.Fuel_Receipt_URL.ToString();

                Message =
                    email_type == "fuelPurchase" ? driverName + " has made a fuel purchase at " + Fuel_Location + "." : null;

                mailBody =
              "<html>" +
               "<head>" +
                 "<style>" +
                 "@import url(https://fonts.googleapis.com/css?family=Fjalla+One|Oxygen);" +
                 "th, td {padding: 15px;text-align: left; border: 1px solid #ddd;}" +
                 ".bold {font-weight:bold} " +
                 ".left-text {text-align:left} " +
                 ".headers {font-family: 'Fjalla One', sans-serif; font-weight: bold;}" +
                 ".values {font-family: 'Oxygen', sans-serif;}" +
                 ".loadNbr {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".status {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".ldconfirm {font-family: 'Oxygen', sans-serif; font-size: 22px;}" +
                 "</style>" +
              "</head>" +
              "<body>" +
                 "<table style='width: 600px'>" +
                 "<colgroup>" +
                 "<col style='width: 100px'>" +
                 "<col style='width: 300px'>" +
                 "</colgroup>" +
                  " <tr> " +
                      " <th colspan='2'class='headers' style='text-align:left; font-family: 'Fjalla One', sans-serif;'><img src='http://maxx-express.com/images/img-carrier-alert-email-header.png'></th>" +
                   "</tr>" +
                   "<tr>" +
                       " <td class='headers'>Fuel Date</td>" +
                       " <td class='values loadNbr'>" + fuelDate + "</td>" +
                  " </tr>" +
                  " <tr>" +
                       " <td class='headers'>Location</td>" +
                       " <td class='values '>" + Fuel_Location + "</td>" +
                  " </tr>" +
                  " <tr>" +
                     "   <td class='headers'>Amount</td>" +
                     "   <td class='values'>" + Fuel_Gallons + " gallons - $" + Fuel_Amount + "</td>" +
                  " </tr>" +
                  " <tr>" +
                     "   <td class='headers'>Odometer</td>" +
                     "   <td class='values'>" + String.Format("{0:n}", Odometer) + "</td>" +
                  " </tr>" +
                  " <tr>" +
                       "   <td class='headers'>Fuel Reciept</td>" +
                       "   <td class='ldconfirm'><a style='padding-top: 10%;' href='" + Receipt_URL + "' /> Click for Fuel Receipt </a></td>" +
                  " </tr>" +

                  " <tr>" +
                    " <td class='values' colspan='2' rowspan='8'>" +
                    "<span class='headers left-text'>Message: </span>" +
                    "" + Message +
                    "</td>" +
                    " </tr>" +
                  " </table>" +
                 "</body>" +
               "</html>";

                message.Body = mailBody;

                try
                {
                    client.Send(message);
                    //  updateEmailIndicators(loadNBR, loadStatus);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
            }
        }

        public static void SendSandLoad_Email(string email_type, string loadNBR, string driverID, int routeID, string Pickup_Location, string Dropoff_1_Location, string Load_Confirmation_URL, string BOL_URL, string BOL_FILE_TYPE, string Miles)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                string driverNM = null;
                string routeNM= null;

                string Message;
                string mailBody;

                var dr = db_data.tbl_dim_driver.FirstOrDefault(p => p.Driver_Nbr == driverID);
                driverNM = dr.Name;

                var rt = db_data.vw_sand_payout.FirstOrDefault(y => y.ID == routeID);
                routeNM = rt.Route_NM;

                string to = "maxxexpresstrucks@gmail.com";
                string from = "web@maxx-express.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = email_type == "statusUpdate" ? "Load Completed (ALERT) - " + driverNM + " has delivered Load #" + loadNBR + " - " + routeNM  : null;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("maxxexpress-com01b.mail.protection.outlook.com");
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "relay-hosting.secureserver.net";
                client.Credentials = new System.Net.NetworkCredential("web@maxx-express.com", "BlueExpress13!");
                client.EnableSsl = client.Port == 587;

                Message = email_type == "statusUpdate" ? driverNM + " has delivered Load " + loadNBR + " to the drop off destination. The signed BOL confirmation link is above." : null;

                mailBody =
              "<html>" +
               "<head>" +
                 "<style>" +
                 "@import url(https://fonts.googleapis.com/css?family=Fjalla+One|Oxygen);" +
                 "th, td {padding: 15px;text-align: left; border: 1px solid #ddd;}" +
                 ".bold {font-weight:bold} " +
                 ".left-text {text-align:left} " +
                 ".headers {font-family: 'Fjalla One', sans-serif; font-weight: bold;}" +
                 ".values {font-family: 'Oxygen', sans-serif;}" +
                 ".loadNbr {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".status {font-family: 'Oxygen', sans-serif; font-size: 24px;}" +
                 ".ldconfirm {font-family: 'Oxygen', sans-serif; font-size: 22px;}" +
                 "</style>" +
              "</head>" +
              "<body>" +
                 "<table style='width: 600px'>" +
                 "<colgroup>" +
                 "<col style='width: 100px'>" +
                 "<col style='width: 300px'>" +
                 "</colgroup>" +
                  " <tr> " +
                      " <th colspan='2'class='headers' style='text-align:left; font-family: 'Fjalla One', sans-serif;'><img src='http://maxx-express.com/images/img-carrier-alert-email-header.png'></th>" +
                   "</tr>" +
                   "<tr>" +
                       " <td class='headers'>Load #</td>" +
                       " <td class='values loadNbr'>" + loadNBR + "</td>" +
                  " </tr>" +
                  " <tr>" +
                     "   <td class='headers'>Mileage</td>" +
                     "   <td class='values'>" + Miles + " Miles</td>" +
                  " </tr>" +
                  " <tr>" +
                     "   <td class='headers'>Pickup</td>" +
                     "   <td class='values'>" + Pickup_Location + "</td>" +
                  " </tr>" +
                  " <tr>" +
                     "   <td class='headers'>Dropoff</td>" +
                     "   <td class='values'>" + Dropoff_1_Location + "</td>" +
                  " </tr>" +
                  " <tr>" +
                       " <td class='headers'>Sand Ticket</td>" +
                       "   <td class='ldconfirm'><a style='padding-top: 10%;' href='" + Load_Confirmation_URL + "' /> #" + loadNBR + " Sand Ticket Link" + "</a></td>" +
                  " </tr>" +
                  " <tr>" +
                       "   <td class='headers'>Delivery Confirmation</td>" +
                       "   <td class='ldconfirm'><a style='padding-top: 10%;' href='" + BOL_URL + BOL_FILE_TYPE  + "' /> #" + loadNBR + " BOL Confirmation Link" + "</a></td>" +
                  " </tr>" +
                  " <tr>" +
                    " <td class='values' colspan='2' rowspan='8'>" +
                    "<span class='headers left-text'>Message: </span>" +
                    "" + Message +
                    "</td>" +
                    " </tr>" +
                  " </table>" +
                 "</body>" +
               "</html>"
               ;

                message.Body = mailBody;

                try
                {
                    client.Send(message);
                    updateEmailIndicators(loadNBR, "Delivered");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
            }
        }

        [System.Web.Mvc.HttpDelete]
        [NoCache]
        public ActionResult DeleteLoad(string LoadNbr)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();
                PortalViewModel addLoadVM = new PortalViewModel();

                try
                {
                    var record = db_data.tbl_fct_loads.FirstOrDefault(e => e.Load_Number == LoadNbr);
                    if (record == null)
                    {
                        return Json(new { success = false, responseText = "Load not found" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        db_data.tbl_fct_loads.Remove(record);
                        db_data.SaveChanges();

                        return Json(new { success = true, responseText = "Add Load was successful" }, JsonRequestBehavior.AllowGet);
                    }
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpDelete]
        [NoCache]
        public ActionResult DeleteFuel(int id)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();
                PortalViewModel addLoadVM = new PortalViewModel();

                try
                {
                    var record = db_data.tbl_fct_fuel_entry.FirstOrDefault(e => e.ID == id);
                    if (record == null)
                    {
                        return Json(new { success = false, responseText = "Fuel Purchase not found" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        db_data.tbl_fct_fuel_entry.Remove(record);
                        db_data.SaveChanges();

                        return Json(new { success = true, responseText = "Fuel Purchase delete was successful" }, JsonRequestBehavior.AllowGet);
                    }
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult MarkFuelPaid(string id)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

                var id_val = int.Parse(id);

                try
                {
                    tbl_fct_fuel_entry purchaseToUpdate = db_data.tbl_fct_fuel_entry.FirstOrDefault(x => x.ID.Equals(id_val));


                    if (purchaseToUpdate == null)
                    {
                        return Json(new { success = false, responseText = "Purchase Not Found, sometimes this happens just refresh the browser and it should work when you try again." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        purchaseToUpdate.Fuel_Paid_Ind = "1";
                    }

                    db_data.SaveChanges();

                    return Json(new { success = true, responseText = "Purchase was marked successfully" }, JsonRequestBehavior.AllowGet);
                }


                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.WorkPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);

                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [NoCache]
        public ActionResult GetLoadChangeList(string ChangeAction)
        {
            PortalViewModel portalVM = new PortalViewModel();
            if (ChangeAction == "status")
            {
                ViewData["OpenLoadsStatusList"] = GetOpenLoadsSummary();
                return PartialView("_LoadStatusList", portalVM);
            }
            if (ChangeAction == "delete")
            {
                ViewData["OpenLoadsDeleteList"] = GetOpenLoadsSummary();
                return PartialView("_LoadEditList", ViewData);
            }
            if (ChangeAction == "edit")
            {
                ViewData["OpenLoadsEditList"] = GetOpenLoadsSummary();
                return PartialView("_LoadEditList", ViewData);
            }

            return null;
        }

        // **Page** - Financials Begin
        [NoCache]
        public ActionResult GetFinancialExpensesRO(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> list = GetFinancialsExpenses_RO_Detail(pViewDate);

            financialsVM.vM_financial_expenses_RO_Data = list;

            return PartialView("_FinancialsExpenses_RO_Detail", financialsVM);
        }

        [NoCache]
        public ActionResult GetFinancialExpenses(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            financialsVM.vM_financial_expensesData = GetFinancialsExpensesDetail(pViewDate);

            return PartialView("_FinancialsExpensesDetail", financialsVM);
        }

        [NoCache]
        public ActionResult GetFinancialPayroll(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            financialsVM.vM_financial_PayrollData = GetFinancialsPayrollDetail(pViewDate);

            return PartialView("_FinancialsPayrollDetail", financialsVM);
        }


        [NoCache]
        public ActionResult GetFinancialFuel(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            financialsVM.vM_financial_FuelData = GetFinancialsFuelDetail(pViewDate);

            return PartialView("_FinancialsFuelDetail", financialsVM);
        }

        [NoCache]
        public ActionResult GetFinancialLoadGross(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            financialsVM.vM_financial_LoadData = GetFinancialsLoadGrossDetail(pViewDate);

            return PartialView("_FinancialsLoadDetail", financialsVM);
        }

        [NoCache]
        public ActionResult GetFinancialDeposits(string pViewDate)
        {
            PortalViewModel financialsVM = new PortalViewModel();

            financialsVM.vM_financial_DepositData = GetFinancialsDepositDetail(pViewDate);

            if (financialsVM.vM_financial_DepositData.Count() == 0)
            {
                financialsVM.vM_financial_Deposit_IND = "0";
            }
            else
            {
                financialsVM.vM_financial_Deposit_IND = "1";
            }

            return PartialView("_FinancialsDepositDetail", financialsVM);
        }

        public string reloadFinancials(string pViewDate)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel financialsVM = new PortalViewModel();
                financialsVM.vM_financial_pnl = GetFinancials(pViewDate);

                var Value = "";

                return (Value);

            }
        }

        [NoCache]
        public string reloadFinancials_NET()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                var Value = (from pValue in db_data.tbl_web_monthly_financial
                             select pValue.Net_PNL).SingleOrDefault();

                return ("$" + Value);

            }
        }
        [NoCache]
        public string reloadFinancials_Revenue()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                var Value = (from pValue in db_data.tbl_web_monthly_financial
                             select pValue.Revenue).SingleOrDefault();

                return ("$" + Value);

            }
        }
        public string reloadFinancials_Fuel()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                var Value = (from pValue in db_data.tbl_web_monthly_financial
                             select pValue.Fuel).SingleOrDefault();

                return ("$" + Value);

            }
        }
        public string reloadFinancials_Payroll()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                var Value = (from pValue in db_data.tbl_web_monthly_financial
                             select pValue.Payroll).SingleOrDefault();

                return ("$" + Value);

            }
        }
        public string reloadFinancials_Expenses()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                var Value = (from pValue in db_data.tbl_web_monthly_financial
                             select pValue.Expenses).SingleOrDefault();

                return ("$" + Value);

            }
        }


        [NoCache]
        public ActionResult financials(string pViewDate)
        {
            var SessData = Session;
            if (Session.SessionID != null)
            {
                PortalViewModel financialsVM = new PortalViewModel();

                //Add Load Modal                 

                using (cascadiaEntities db_data = new cascadiaEntities())
                {
                    var CurrMonth_val = (from d in db_data.vw_date_pnl_month
                                         where d.CurrMonth == 1
                                         select d.Month_Nm).SingleOrDefault();

                    IEnumerable<vw_date_pnl_month> PNL_Months = db_data.vw_date_pnl_month.OrderByDescending(s => s.YearMonth).ToList();
                    financialsVM.vM_financial_CurrMonth = CurrMonth_val.ToString();

                    // Return Current Max YearMonth by Default
                    long maxId = (from c in db_data.vw_date_pnl_month select c.ID).Max();
                    var period = (
                                  from tdate in db_data.vw_date_pnl_month
                                  where tdate.ID == maxId
                                  select tdate.YearMonth
                                  ).SingleOrDefault();

                    financialsVM.vM_financial_Deposit_IND = "1";
                    financialsVM.vM_financial_pnl_months = PNL_Months;
                    financialsVM.vM_financial_pnl = GetFinancials(period);

                    return View(financialsVM);

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        // **Page** - Financials END

        // **Page** - Expenses Begin
        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult EditExpensePurpose(List<tbl_fct_expense> exPurposeJSON)
        {

            System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                try
                {
                    foreach (var item in exPurposeJSON)
                    {

                        var getRecord = db_data.tbl_fct_expense.Where(f => f.ID.ToString() == item.ID.ToString()).FirstOrDefault();
                        if (getRecord != null)
                        {
                            getRecord.Purpose = item.Purpose;
                            getRecord.Expense_IND = item.Expense_IND;
                            getRecord.Exclusion_Reason = item.Exclusion_Reason;
                        }

                        db_data.SaveChanges();
                    }

                    return Json(new { success = true, responseText = "Expense Purpose update was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);
                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [NoCache]
        public PartialViewResult GetExpenseDetail(string exID)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel expenseVM = new PortalViewModel();
                expenseVM.vM_ExpensePurposeData = db_data.vw_expenses.Where(a => a.ID.ToString() == exID).ToList();

                return PartialView("_Expenses_PurposeDiv", expenseVM);
            }
        }


        [NoCache]
        public ActionResult GetFuelTotalHeader(string pViewDate, string pEmployee)
        {
            PortalViewModel fuelVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Last_Month = Month.AddMonths(-1).ToString();
            var Last_Month_End = Month.AddDays(-1).ToString();
            var Days_All = "01/01/2018";
            string trans_date = null;
            string view_by_dateText = null;
            string employee = null;
            bool range = false;
            string range_End_Date = null;

            if (pViewDate == "ALL" || pViewDate == "All")
            {
                trans_date = Days_All;
                view_by_dateText = "All Fuel Expenses";
            }
            if (pViewDate == "LastMonth")
            {
                trans_date = Last_Month;
                range = true;
                range_End_Date = Last_Month_End;
                view_by_dateText = "Fuel Last Month";
            }
            if (pViewDate == "Month")
            {
                trans_date = This_Month;
                view_by_dateText = "Fuel Expenses This Month";
            }
            if (pViewDate == "30Days" || pViewDate == null)
            {
                trans_date = Days_30;
                view_by_dateText = "Fuel Last 30 Days";
            }

            if (pEmployee != null)
            {
                employee = pEmployee.Replace("_", " ").ToString();
            }
            else
            {
                employee = null;
            }

            ViewData["FuelSummaryData"] = GetFuelData(trans_date, employee, range, range_End_Date);

            IEnumerable<vw_fuel> listValues;
            listValues = GetFuelData(trans_date, employee, range, range_End_Date);

            decimal total = listValues.Select(x => x.Amount).Sum(v => Decimal.Parse(v));
            List<decimal> listTotal = new List<decimal>();
            listTotal.Add(total);
            List<decimal> distinctTotal = listTotal.Distinct().ToList();


            fuelVM.fuel_Total = distinctTotal.Single().ToString("C");
            fuelVM.fuel_ViewByDate = view_by_dateText;

            return PartialView("_ExpensesFuelTotal_Header", fuelVM);

        }

        [NoCache]
        public ActionResult GetFuelSummary(string pViewDate, string pEmployee)
        {
            PortalViewModel fuelVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Last_Month = Month.AddMonths(-1).ToString();
            var Last_Month_End = Month.AddDays(-1).ToString();
            var Days_All = "01/01/2018";
            string trans_date = null;
            string employee = null;
            bool range = false;
            string range_End_Date = null;

            if (pViewDate == "ALL" || pViewDate == "All")
            {
                trans_date = Days_All;

            }
            if (pViewDate == "LastMonth")
            {
                trans_date = Last_Month;
                range = true;
                range_End_Date = Last_Month_End;
            }
            if (pViewDate == "Month")
            {
                trans_date = This_Month;
            }
            if (pViewDate == "30Days" || pViewDate == null)
            {
                trans_date = Days_30;
            }

            if (pEmployee != null)
            {
                employee = pEmployee.Replace("_", " ").ToString();
            }
            else
            {
                employee = null;
            }

            ViewData["FuelSummaryData"] = GetFuelData(trans_date, employee, range, range_End_Date);

            return PartialView("_ExpensesFuel_Summary", fuelVM);

        }

        [NoCache]
        public ActionResult GetExpenseTotalHeader(string pViewDate, string pExpenseType)
        {
            PortalViewModel expensesVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Last_Month = Month.AddMonths(-1).ToString();
            var Last_Month_End = Month.AddDays(-1).ToString();
            var Days_All = "01/01/2018";
            string trans_date = null;
            string view_by_dateText = null;
            string expenseType = null;
            bool range = false;
            string range_End_Date = null;

            if (pViewDate == "ALL" || pViewDate == "All")
            {
                trans_date = Days_All;
                view_by_dateText = "All Expenses";
            }
            if (pViewDate == "LastMonth")
            {
                trans_date = Last_Month;
                range = true;
                range_End_Date = Last_Month_End;
                view_by_dateText = "Expenses Last Month";
            }
            if (pViewDate == "Month")
            {
                trans_date = This_Month;
                view_by_dateText = "Expenses This Month";
            }
            if (pViewDate == "30Days" || pViewDate == null)
            {
                trans_date = Days_30;
                view_by_dateText = "Expenses Last 30 Days";
            }

            if (pExpenseType != null)
            {
                expenseType = pExpenseType;
            }
            else
            {
                expenseType = null;
            }

            IEnumerable<vw_expenses> listValues;
            listValues = GetExpenseData(trans_date, expenseType, range, range_End_Date);

            decimal total = listValues.Select(x => x.Amount).Sum(v => Decimal.Parse(v));
            List<decimal> listTotal = new List<decimal>();
            listTotal.Add(total);
            List<decimal> distinctTotal = listTotal.Distinct().ToList();


            expensesVM.expense_Total = distinctTotal.Single().ToString("C");
            expensesVM.expense_ViewByDate = view_by_dateText;

            return PartialView("_ExpensesExpensesTotal_Header", expensesVM);

        }

        [NoCache]
        public ActionResult GetExpenseSummary(string pViewDate, string pExpenseType)
        {
            PortalViewModel expensesVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Last_Month = Month.AddMonths(-1).ToString();
            var Last_Month_End = Month.AddDays(-1).ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Days_All = "01/01/2018";
            string trans_date = null;
            string view_by_dateText = null;
            string expenseType = null;
            bool range = false;
            string range_End_Date = null;

            if (pViewDate == "ALL" || pViewDate == "All")
            {
                trans_date = Days_All;
                view_by_dateText = "All Expenses";
            }
            if (pViewDate == "LastMonth")
            {
                trans_date = Last_Month;
                range = true;
                range_End_Date = Last_Month_End;
                view_by_dateText = "Expenses Last Month";
            }
            if (pViewDate == "Month")
            {
                trans_date = This_Month;
                view_by_dateText = "Expenses This Month";
            }
            if (pViewDate == "30Days" || pViewDate == null)
            {
                trans_date = Days_30;
                view_by_dateText = "Expenses Last 30 Days";
            }

            if (pExpenseType != null)
            {
                expenseType = pExpenseType.Replace("_", " ").ToString();
            }
            else
            {
                expenseType = null;
            }

            ViewData["ExpenseSummaryData"] = GetExpenseData(trans_date, expenseType, range, range_End_Date);

            IEnumerable<vw_expenses> listValues;
            listValues = GetExpenseData(trans_date, expenseType, range, range_End_Date);

            decimal total = listValues.Select(x => x.Amount).Sum(v => Decimal.Parse(v));
            List<decimal> listTotal = new List<decimal>();
            listTotal.Add(total);
            List<decimal> distinctTotal = listTotal.Distinct().ToList();

            ViewData["ExpenseTotal"] = distinctTotal.Single().ToString();
            ViewData["ExpenseTotalDate"] = view_by_dateText;

            expensesVM.expense_Total = distinctTotal.Single().ToString("C");
            expensesVM.expense_ViewByDate = view_by_dateText;

            return PartialView("_ExpensesExpense_Summary", expensesVM);

        }


        [NoCache]
        public ActionResult expenses()
        {
            var SessData = Session;
            if (Session.SessionID != null)
            {
                PortalViewModel expensesVM = new PortalViewModel();

                GetExpenseDetail("1");

                expensesVM.expense_Total = "0";
                expensesVM.expense_ViewByDate = "0";

                return View(expensesVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        // **Page** - Expenses END

        // **Page** - Production Begin


        public ActionResult GetProductionModal(string pDataSource, string pPeriod, string pEntity)
        {
            PortalViewModel productionVM = new PortalViewModel();

            if (pDataSource == null && pPeriod == null)
            {
                ViewData["ProductionModalData_Gross"] = GetProductionModal_Gross(pPeriod, pEntity);
                ViewData["ProductionModalData_Miles"] = GetProductionModal_Miles(pPeriod, pEntity);
                ViewData["ProductionModalData_Fuel"] = GetProductionModal_Fuel(pPeriod, pEntity);
                ViewData["ProductionModalData_Payroll"] = GetProductionModal_Payroll(pPeriod);
                productionVM.productionViewBy = "null";
            }
            else if (pDataSource == "gross")
            {
                ViewData["ProductionModalData_Gross"] = GetProductionModal_Gross(pPeriod, pEntity);
                productionVM.productionViewBy = "gross";
            }
            else if (pDataSource == "miles")
            {
                ViewData["ProductionModalData_Miles"] = GetProductionModal_Miles(pPeriod, pEntity);
                productionVM.productionViewBy = "miles";
            }
            else if (pDataSource == "fuel")
            {
                ViewData["ProductionModalData_Fuel"] = GetProductionModal_Fuel(pPeriod, pEntity);
                productionVM.productionViewBy = "fuel";
            }
            else if (pDataSource == "payroll")
            {
                ViewData["ProductionModalData_Payroll"] = GetProductionModal_Payroll(pPeriod);
                productionVM.productionViewBy = "payroll";
            }


            return PartialView("_Production_Modal", productionVM);

        }

        public ActionResult GetProductionWeekly(string pViewDate)
        {
            PortalViewModel productionVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Days_All = "01/01/2018";
            string week_start_date = null;


            if (pViewDate == "ALL" || pViewDate == null)
            {
                week_start_date = Days_All;
            }

            if (pViewDate == "30Days" || pViewDate == "30Days")
            {
                week_start_date = Days_30;

            }

            ViewData["ProductionWeeklyData"] = GetWeeklyProductionData(week_start_date);



            return PartialView("_Production_Summary", productionVM);

        }


        public ActionResult GetProductionDetailWeekly(string pViewDate)
        {
            PortalViewModel productionVM = new PortalViewModel();

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Days_All = "01/01/2018";
            string week_start_date = null;

            if (pViewDate == "ALL" || pViewDate == null)
            {
                week_start_date = Days_All;

            }

            if (pViewDate == "30Days" || pViewDate == "30Days")
            {
                week_start_date = Days_30;

            }

            ViewData["ProductionWeeklyDetailData"] = GetWeeklyProductionDetailData(week_start_date);

            return PartialView("_Production_Detail", productionVM);

        }



        [NoCache]
        public ActionResult production()
        {
            var SessData = Session;
            if (Session.SessionID != null)
            {
                PortalViewModel productionVM = new PortalViewModel();

                GetProductionWeekly("All");
                GetProductionDetailWeekly("All");

                //   productionVM.expense_Total = "0";
                productionVM.expense_ViewByDate = "0";

                return View(productionVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        // **Page** - Production END


        // **Page** - PAYROLL Begin 


        [NoCache]
        public void RunPayroll()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel payrollVM = new PortalViewModel();

                var sumbitDay = (from pPeriod in db_data.vw_payroll_period_summary
                                 select pPeriod.Submit_Day).SingleOrDefault();

                db_data.Database.ExecuteSqlCommand(
                        "sp_Payroll_Monday @Monday_RunDate",
                        new SqlParameter("Monday_RunDate", sumbitDay)
                    );

            }
        }


        [System.Web.Mvc.HttpPut]
        [NoCache]
        public ActionResult EditPayrollAdjustments(List<tbl_fct_payroll_adjustments> adjustmentsJSON)
        {

            System.Net.Http.HttpRequestMessage NewRequest = new System.Net.Http.HttpRequestMessage();

            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                var currentPayDay = (from pPeriod in db_data.vw_payroll_period_summary
                                     select pPeriod.Pay_Day).SingleOrDefault();
                try
                {
                    foreach (var item in adjustmentsJSON)
                    {
                        var getRecord = db_data.tbl_fct_payroll_adjustments.Where(f => f.Pay_Day.Equals(currentPayDay)).Where(p => p.Employee_Number == item.Employee_Number).FirstOrDefault();
                        if (getRecord != null)
                        {
                            getRecord.Pay_Day = item.Pay_Day;
                            getRecord.Employee_Number = item.Employee_Number;
                            getRecord.Detention_Time = item.Detention_Time;
                            getRecord.Detention_Pay = item.Detention_Pay;
                            getRecord.Stop_Count = item.Stop_Count;
                            getRecord.Stop_Pay = item.Stop_Pay;
                            getRecord.Misc_Adjustment_Amount = item.Misc_Adjustment_Amount;
                            getRecord.Misc_Adjustment_Notes = item.Misc_Adjustment_Notes;

                        }

                        db_data.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Edit Payroll Adjustment was successful" }, JsonRequestBehavior.AllowGet);
                }

                catch (DbEntityValidationException e)
                {
                    var newException = new MaxxExpress.Areas.PortalPage.Models.Tools.CustomTools.FormattedDbEntityValidationException(e);
                    return Json(new { success = false, responseText = "Database Error: " + newException }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [NoCache]
        public PartialViewResult GetPayrollAdjustmentDetail()
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {
                PortalViewModel payrollAdjVM = new PortalViewModel();

                var currentPayDay = (from pPeriod in db_data.vw_payroll_period_summary
                                     select pPeriod.Pay_Day).SingleOrDefault();

                var AdjustmentData = (from item in db_data.tbl_fct_payroll_adjustments
                                      where item.Pay_Day.Equals(currentPayDay)
                                      select item).ToList();

                payrollAdjVM.vM_PayrollAdjustmentData = AdjustmentData;
                return PartialView("_Payroll_AdjustmentDiv", payrollAdjVM);
            }
        }

        [NoCache]
        public ActionResult GetPayrollSummaryIndex(string pViewDate, string pEmployee)
        {

            DateTime date = DateTime.Now;
            var Month = new DateTime(date.Year, date.Month, 1);
            var This_Month = Month.ToString();
            var Days_30 = date.AddDays(-30).ToString("MM/dd/yyyy");
            var Days_7 = date.AddDays(0).ToString("MM/dd/yyyy");
            var Days_All = "01/01/2018";
            string pay_date = null;
            string employee = null;

            if (pViewDate == "ALL" || pViewDate == "All")
            {
                pay_date = Days_All;
            }
            if (pViewDate == "Week")
            {
                pay_date = Days_7;
            }
            if (pViewDate == "30Days")
            {
                pay_date = Days_30;
            }
            if (pViewDate == "Month" || pViewDate == null)
            {
                pay_date = This_Month;
            }

            if (pEmployee != null)
            {
                employee = pEmployee.Replace("_", " ").ToString();
            }
            else
            {
                employee = null;
            }

            ViewData["PayrollSummaryData"] = GetPayrollSummary(pay_date, employee);

            return PartialView("_PayrollSummary_Index", ViewData);

        }


        [NoCache]
        public ActionResult payroll()
        {

            var SessData = Session;
            if (Session.SessionID != null)
            {
                using (cascadiaEntities db_data = new cascadiaEntities())
                {
                    PortalViewModel payrollAdjVM = new PortalViewModel();

                    var payroll_IND = (from pPeriod in db_data.vw_payroll_period_summary
                                       select pPeriod.Payroll_Run_Ind).SingleOrDefault();
                    payrollAdjVM.payroll_RunIND = payroll_IND.ToString();

                    ViewData["PayrollDatePeriod"] = GetPayrollPeriod();

                    return View(payrollAdjVM);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // **Page** - PAYROLL End 

        // **Page** - ADMIN AREA Begin 

        [NoCache]
        public void resetLoad(string pLoadNbr, string pResetType)
        {
            using (cascadiaEntities db_data = new cascadiaEntities())
            {

                if (pResetType == "ResetAll")
                {
                    db_data.Database.ExecuteSqlCommand(
                            "sp_ResetLoad_All_Status @LoadNbr",
                            new SqlParameter("LoadNbr", pLoadNbr)
                        );
                } if (pResetType == "ResetPaid")
                {
                    db_data.Database.ExecuteSqlCommand(
                            "sp_ResetLoad_Paid_Status @LoadNbr",
                            new SqlParameter("LoadNbr", pLoadNbr)
                        );
                }
                else if (pResetType == "ResetDelivered")
                {
                    db_data.Database.ExecuteSqlCommand(
                            "sp_ResetLoad_Delivered_Status @LoadNbr",
                            new SqlParameter("LoadNbr", pLoadNbr)
                        );
                }

            }
        }

        [NoCache]
        public ActionResult admin()
        {

            var SessData = Session;
            if (Session.SessionID != null)
            {
                using (cascadiaEntities db_data = new cascadiaEntities())
                {
                    PortalViewModel adminVM = new PortalViewModel();

                    //var payroll_IND = (from pPeriod in db_data.vw_payroll_period_summary
                    //                   select pPeriod.Payroll_Run_Ind).SingleOrDefault();
                    //payrollAdjVM.payroll_RunIND = payroll_IND.ToString();

                    //ViewData["PayrollDatePeriod"] = GetPayrollPeriod();

                    return View(adminVM);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // **Page** - ADMIN AREA End 

        ///  **Page** - Login Section Begin

        private HttpCookie CreateLoginCookie(string user)
        {
            HttpCookie LoginCookies = new HttpCookie("LoginUser");
            LoginCookies.Value = user;
            LoginCookies.Expires = DateTime.Now.AddHours(1);
            return LoginCookies;
        }

        private HttpCookie CreateAdminCookie(string access_level)
        {
            HttpCookie LoginCookies = new HttpCookie("Access");
            LoginCookies.Value = access_level;
            LoginCookies.Expires = DateTime.Now.AddHours(1);
            return LoginCookies;
        }

        private HttpCookie CreateUserTypeCookie(string user_type)
        {
            HttpCookie LoginCookies = new HttpCookie("UserType");
            LoginCookies.Value = user_type;
            LoginCookies.Expires = DateTime.Now.AddHours(1);
            return LoginCookies;
        }

        private HttpCookie CreateUserTypeIDCookie(string user_type_id)
        {
            HttpCookie LoginCookies = new HttpCookie("UserTypeID");
            LoginCookies.Value = user_type_id;
            LoginCookies.Expires = DateTime.Now.AddHours(1);
            return LoginCookies;
        }

        public ActionResult Login()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tbl_web_users u)
        {
            ViewBag.UsernameError = "";
            ViewBag.PasswordError = "";

            ///Comment START Here for DEV TESTING
            if (ModelState.IsValid)
            {
                using (cascadiaEntities cascadia = new cascadiaEntities())
                {
                    var v = cascadia.tbl_web_users.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogUserID"] = v.ID.ToString();
                        Session["LogUserFullName"] = v.Name.ToString();
                        Session["LogUserAccess"] = v.UserType.ToString();
                        Session["LogUserUserTypeID"] = v.UserType_ID.ToString();
                        Session["LogUserPNL_IND"] = v.Financial_PNL_Access.ToString();
                        Session["LogUserExp_IND"] = v.Financial_Expense_Access.ToString();
                        Session["LogUserPR_IND"] = v.Payroll_Access.ToString();
                        Session["LogUserHR_IND"] = v.HR_Access.ToString();

                        if (v.AdminType.ToString() == "Admin")
                        {
                            Session["LogAdminAccess"] = v.AdminType.ToString();
                        }
                        else
                        {
                            Session["LogAdminAccess"] = "Standard-User";
                        }

                        Response.Cookies.Add(CreateLoginCookie(Session["LogUserFullName"].ToString()));
                        Response.Cookies.Add(CreateAdminCookie(Session["LogAdminAccess"].ToString()));
                        Response.Cookies.Add(CreateUserTypeCookie(Session["LogUserAccess"].ToString()));
                        Response.Cookies.Add(CreateUserTypeIDCookie(Session["LogUserUserTypeID"].ToString()));

                        return RedirectToAction("Index");
                    }

                    var user_user = cascadia.tbl_web_users.Where(a => a.UserName.Equals(u.UserName)).FirstOrDefault();
                    var user_pass = cascadia.tbl_web_users.Where(a => a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v == null)
                    {
                        if (user_user == null)
                        {
                            ViewBag.UsernameError = "- Username is incorrect";
                        }
                        if (user_pass == null)
                        {
                            ViewBag.PasswordError = "- Password is incorrect";
                        }
                    }
                }
            }
            return View(u);
            ///Comment END Here for DEV TESTING

            ///Un-Comment Here for DEV TESTING
            //  return RedirectToAction("Index");
            ///Un-Comment Here for DEV TESTING
        }

        ///  **Page** - Login Section End

        public ActionResult Keepalive()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult logout()
        {
            Session.Abandon();
            return View();
        }


    }
}

