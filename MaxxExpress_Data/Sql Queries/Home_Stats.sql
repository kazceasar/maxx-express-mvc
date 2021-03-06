/****** Script for SelectTopNRows command from SSMS  ******/

Declare  @StartDate date = '05-27-2018'; 
Declare  @EndDate date = '06-02-2018'; 

SELECT TOP 1000 [DateID]
      ,[DateValue]
      ,[YearMonthNumber]
      ,[MonthDayNumber]
      ,[YearNumber]
      ,[DayName]
      ,[MonthName]
      ,[Week_of]
      ,[Week_Ending]
  FROM [cascadia].[dbo].[vw_date_week];

 -- if exists (select count(*) from #Mileage)
 -- BEGIN
 --  DROP TABLE #Mileage
 
 --END

 --ELSE 

With CTE_Mileage as (
			SELECT  
		   [DateVal]
		   ,Max(Daily_Accumulated_Distance) as Amount
		   ,Week_of
   
  FROM [cascadia].[dbo].[vw_mileage]
 Group by
 DateVal,
 Week_of
)
SELECT Week_of, Sum(amount) as Amount, 'Miles Driven' as Type 
INTO #Mileage
FROM CTE_Mileage
WHERE Week_of =  @StartDate 
Group by Week_of

SELECT cal.Week_of, Sum(cast(ol.Payout as decimal(10,2))) as Amount, 'Delivered - Not Paid Amount'  as Type
FROM vw_loads_summary_open ol 
Join tbl_fct_loads ld on ld.Load_Number = ol.Load_Number
Join vw_load_status lds on lds.Load_Number = ol.Load_Number
Join vw_date_full cal on cal.DateValue = Cast(Convert(varchar(10),Delivered_Status_DNT,10) as date)
WHERE lds.Status = 'Load Delivered'
and lds.Delivered_Status_DNT between @StartDate and @EndDate
GROUP by cal.Week_of

Union

  
Select cal.Week_of, Sum(cast(f.Amount as decimal(10,2))) as Fuel_Amount, 'Fuel Dollars Used'  as Type
from vw_fuel f join 
vw_date_full cal on 
cal.DateValue = f.Date  WHERE DATE 
between @StartDate and @EndDate
Group By cal.Week_of

Union

SELECT cal.Week_of, count(*) as Amount, 'Delivered - Not Paid Count'  as Type
FROM vw_loads_summary_open ol 
Join tbl_fct_loads ld on ld.Load_Number = ol.Load_Number
Join vw_load_status lds on lds.Load_Number = ol.Load_Number
Join vw_date_full cal on cal.DateValue = Cast(Convert(varchar(10),Delivered_Status_DNT,10) as date)
WHERE lds.Status = 'Load Delivered'
and lds.Delivered_Status_DNT between @StartDate and @EndDate
GROUP by cal.Week_of

UNION

SELECT Week_of, Amount, Type FROM #Mileage;


 
 