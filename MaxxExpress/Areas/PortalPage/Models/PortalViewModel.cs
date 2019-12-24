using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace MaxxExpress.Areas.PortalPage.Models
{
    public class PortalViewModel
    {
        //ReUseable Objects
        public class KeyVal_Object
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        //Add Load Modal
        public tbl_fct_loads vM_AddLoadData { get; set; }
        public IEnumerable<KeyVal_Object> AddLoad_Weekof_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_Route_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_Customer_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_Driver_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_Truck_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_Trailer_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddLoad_DayofWeek_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public string AddLoad_LoadofWeek_Val { get; set; }
        public IEnumerable<KeyVal_Object> AddLoad_LoadofWeek_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "1", Value = "1" }, 
                    new PortalViewModel.KeyVal_Object { Id = "2", Value = "2" }, 
                    new PortalViewModel.KeyVal_Object { Id = "3", Value = "3" }, 
                    new PortalViewModel.KeyVal_Object { Id = "4", Value = "4" }, 
                    new PortalViewModel.KeyVal_Object { Id = "5", Value = "5" }, 
                    new PortalViewModel.KeyVal_Object { Id = "6", Value = "6" },
                    new PortalViewModel.KeyVal_Object { Id = "7", Value = "7" },
                    new PortalViewModel.KeyVal_Object { Id = "8", Value = "8" },
                    new PortalViewModel.KeyVal_Object { Id = "9", Value = "9" },
                    new PortalViewModel.KeyVal_Object { Id = "10", Value = "10" },
                    new PortalViewModel.KeyVal_Object { Id = "11", Value = "11" },
                    new PortalViewModel.KeyVal_Object { Id = "12", Value = "12" },
                    new PortalViewModel.KeyVal_Object { Id = "13", Value = "13" },
                    new PortalViewModel.KeyVal_Object { Id = "14", Value = "14" },
                    new PortalViewModel.KeyVal_Object { Id = "15", Value = "15" },
                    new PortalViewModel.KeyVal_Object { Id = "16", Value = "16" }, 
                    new PortalViewModel.KeyVal_Object { Id = "17", Value = "17" }, 
                    new PortalViewModel.KeyVal_Object { Id = "18", Value = "18" }, 
                    new PortalViewModel.KeyVal_Object { Id = "19", Value = "19" }, 
                    new PortalViewModel.KeyVal_Object { Id = "20", Value = "20" }, 
                    new PortalViewModel.KeyVal_Object { Id = "21", Value = "21" },
                    new PortalViewModel.KeyVal_Object { Id = "22", Value = "22" },
                    new PortalViewModel.KeyVal_Object { Id = "23", Value = "23" },
                    new PortalViewModel.KeyVal_Object { Id = "24", Value = "24" },
                    new PortalViewModel.KeyVal_Object { Id = "25", Value = "25" },
                    new PortalViewModel.KeyVal_Object { Id = "26", Value = "26" },
                    new PortalViewModel.KeyVal_Object { Id = "27", Value = "27" },
                    new PortalViewModel.KeyVal_Object { Id = "28", Value = "28" },
                    new PortalViewModel.KeyVal_Object { Id = "29", Value = "29" },
                    new PortalViewModel.KeyVal_Object { Id = "30", Value = "30" } 
                };
        public IEnumerable<KeyVal_Object> AddLoad_LTL_IND_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "No", Value = "No" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Yes", Value = "Yes" }                
                };

        public string AddLoad_LoadFileName { get; set; }
        public HttpPostedFileBase AddLoad_Load_Confirmation_Upload { get; set; }
        //End Add Load Modal

        //Edit Load Modal
        public vw_loads_detail_open vM_EditLoadData { get; set; }
        public string EditLoad_Weekof_Current { get; set; }
        public string EditLoad_Customer_ID { get; set; }
        public string EditLoad_Customer_NM { get; set; }
        public string EditLoad_Driver_Nbr { get; set; }
        public string EditLoad_Truck_Nbr { get; set; }
        public string EditLoad_Trailer_Nbr { get; set; }
        public string EditLoad_Load_Date { get; set; }
        public IEnumerable<KeyVal_Object> EditLoad_Weekof_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_Customer_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_Route_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_Driver_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_Truck_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_Trailer_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> EditLoad_DayofWeek_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public bool EditLoad_HasLoad_LoadConfirmation { get; set; }
        public bool EditLoad_HasDeliveryConfirmation { get; set; }
        public bool EditLoad_MulitpleStops { get; set; }
        public HttpPostedFileBase EditLoad_Load_Confirmation_Upload { get; set; }
        public IEnumerable<KeyVal_Object> EditLoad_LoadofWeek_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "1", Value = "1" }, 
                    new PortalViewModel.KeyVal_Object { Id = "2", Value = "2" }, 
                    new PortalViewModel.KeyVal_Object { Id = "3", Value = "3" }, 
                    new PortalViewModel.KeyVal_Object { Id = "4", Value = "4" }, 
                    new PortalViewModel.KeyVal_Object { Id = "5", Value = "5" }, 
                    new PortalViewModel.KeyVal_Object { Id = "6", Value = "6" },
                    new PortalViewModel.KeyVal_Object { Id = "7", Value = "7" },
                    new PortalViewModel.KeyVal_Object { Id = "8", Value = "8" },
                    new PortalViewModel.KeyVal_Object { Id = "9", Value = "9" },
                    new PortalViewModel.KeyVal_Object { Id = "10", Value = "10" },
                    new PortalViewModel.KeyVal_Object { Id = "11", Value = "11" },
                    new PortalViewModel.KeyVal_Object { Id = "12", Value = "12" },
                    new PortalViewModel.KeyVal_Object { Id = "13", Value = "13" },
                    new PortalViewModel.KeyVal_Object { Id = "14", Value = "14" },
                    new PortalViewModel.KeyVal_Object { Id = "15", Value = "15" } 
                };
        public IEnumerable<KeyVal_Object> EditLoad_LTL_IND_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "No", Value = "No" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Yes", Value = "Yes" }                
                };
        //End Edit Load Modal

        // Status Update Modal 
        public string Status { get; set; }
        public IEnumerable<SelectListItem> vM_UpdateStatus_StatusList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem> { 
                    new SelectListItem() { Text = "Not Started", Value = "Not Started" }, 
                    new SelectListItem() { Text = "Picked Up", Value = "Picked Up" }, 
                    new SelectListItem() { Text = "Delivered", Value = "Delivered" },
                    new SelectListItem() { Text = "Paid", Value = "Paid" } 
                };
                return list.Select(l => new SelectListItem { Selected = (l.Value == Status), Text = l.Text, Value = l.Value });
            }
        }
        //End Status Update Modal 

        // Financials
        public string vM_financial_Deposit_IND { get; set; }
        public string vM_financial_CurrMonth { get; set; }
        public IEnumerable<vw_date_pnl_month> vM_financial_pnl_months { get; set; }
        public IEnumerable<tbl_web_monthly_financial_revenue_load> vM_financial_LoadData { get; set; }
        public IEnumerable<tbl_web_monthly_financial_revenue_deposit> vM_financial_DepositData { get; set; }
        public IEnumerable<tbl_web_monthly_financial_fuel> vM_financial_FuelData { get; set; }
        public IEnumerable<tbl_web_monthly_financial_payroll> vM_financial_PayrollData { get; set; }
        public IEnumerable<tbl_web_monthly_financial_expenses_reoccurring> vM_financial_expenses_RO_Data { get; set; }
        public IEnumerable<tbl_web_monthly_financial_expenses> vM_financial_expensesData { get; set; }
        public IEnumerable<tbl_web_monthly_financial> vM_financial_pnl { get; set; }

        // End Financials


        //Expenses 

        public string expense_ViewByDate { get; set; }
        public string expense_Total { get; set; }
        public string fuel_ViewByDate { get; set; }
        public string fuel_Total { get; set; }
        public List<vw_expenses> vM_ExpensePurposeData { set; get; }

        // End Expenses 

        //Production 
        public string productionViewBy { get; set; }
        // End Production 

        //Payroll 
        public List<tbl_fct_payroll_adjustments> vM_PayrollAdjustmentData { set; get; }
        public string payroll_RunIND { get; set; }

        // End Payroll 


        public PortalViewModel()
        {
            vM_ExpensePurposeData = new List<vw_expenses>();
            vM_PayrollAdjustmentData = new List<tbl_fct_payroll_adjustments>();

        }


        //Create Note Drawer

        public IEnumerable<tbl_web_notes> vM_ViewNotes { get; set; }
        public tbl_web_notes vM_CreateNote { get; set; }
        public string vM_CreateNote_SentFrom { get; set; }
        public IEnumerable<KeyVal_Object> vM_CreateNote_SendTo_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> vM_CreateNote_Message_Type_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "Reminder", Value = "Reminder" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Bill Due", Value = "Bill Due" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Task", Value = "Task" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Message", Value = "Message" }, 
                    
                };

        //End Create Note Drawer

        //Add Fuel Entty Modal
        public tbl_fct_fuel_entry vM_AddFuelData { get; set; }

        public IEnumerable<KeyVal_Object> AddFuel_Driver_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddFuel_Truck_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddFuel_DayofWeek_List = new List<KeyVal_Object> { new KeyVal_Object { Id = "", Value = "" } };
        public IEnumerable<KeyVal_Object> AddFuel_FuelType_List = new List<KeyVal_Object> {
                    new PortalViewModel.KeyVal_Object { Id = "Regular/Diesel", Value = "Regular/Diesel" }, 
                    new PortalViewModel.KeyVal_Object { Id = "Def", Value = "Def" }                 
                };
    }
}