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
    
    public partial class vw_expenses
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DateValue { get; set; }
        public string Trans_Item { get; set; }
        public string Amount { get; set; }
        public string Method { get; set; }
        public string Purpose { get; set; }
        public string Exclusion_Reason { get; set; }
        public int Reoccurring_IND { get; set; }
        public string Reoccurring_DT { get; set; }
        public int Expense_IND { get; set; }
        public string Authorized_By { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string DateValue_str { get; set; }
    }
}
