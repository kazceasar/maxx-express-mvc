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
    
    public partial class tbl_web_notes
    {
        public int ID { get; set; }
        public string SentFrom { get; set; }
        public string SentTo { get; set; }
        public string Message_Type { get; set; }
        public string Subject { get; set; }
        public string Message_Content { get; set; }
        public string Action_Date { get; set; }
        public string Due_Amount { get; set; }
        public string Attachment_IND { get; set; }
        public string Attachment_1_Path { get; set; }
        public string Attachment_2_Path { get; set; }
        public string Attachment_3_Path { get; set; }
        public string Alert_IND { get; set; }
        public string Alert_Start_Dt { get; set; }
        public string Alert_End_Dt { get; set; }
        public string Message_Type_class { get; set; }
        public string Active_IND { get; set; }
        public string Archive_IND { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
