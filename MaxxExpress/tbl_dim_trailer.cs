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
    
    public partial class tbl_dim_trailer
    {
        public int ID { get; set; }
        public string Trailer_Nbr { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Vin { get; set; }
        public string Plate { get; set; }
        public string Liability_Policy_Number { get; set; }
        public string Active_IND { get; set; }
        public Nullable<int> Sort_Order { get; set; }
        public Nullable<System.DateTime> Purchase_Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
