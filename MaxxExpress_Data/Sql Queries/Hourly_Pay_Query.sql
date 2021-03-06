/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [DateVal], d.Driver_Nbr
      ,Min([Time]) as Clock_IN
	  ,Max([Time]) as Clock_Out
	  ,Cast(Round(DATEDIFF(MINUTE,Min([Time]),Max([Time])) / 60.00,2) as decimal(10,2)) as OnDuty_Hours 
      
  FROM [cascadia].[dbo].[vw_mileage] m 
  Join tbl_dim_driver d 
  on d.Driver_Nbr = m.Driver_Number
  and d.Pay_Type = 'Hourly'
  and m.Ignition = 'Ignition On'

  GROUP BY [DateVal], Driver_Nbr 
  Order by 1

 

