//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaxxExpress
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_fct_payroll_adjustments
    {
        public int ID { get; set; }
        public System.DateTime Pay_Day { get; set; }
        public System.DateTime Week_Starting { get; set; }
        public System.DateTime Week_Ending { get; set; }
        public System.DateTime Submit_Day { get; set; }
        public Nullable<decimal> Detention_Time { get; set; }
        public Nullable<decimal> Detention_Pay { get; set; }
        public Nullable<decimal> Stop_Count { get; set; }
        public Nullable<decimal> Stop_Pay { get; set; }
        public Nullable<decimal> Misc_Adjustment_Amount { get; set; }
        public string Misc_Adjustment_Notes { get; set; }
        public string Employee_Number { get; set; }
        public string Employee_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
