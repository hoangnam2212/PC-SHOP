//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMSWebsite.Models
{
    using System;
    
    public partial class store_GetProductTopSaleHomePage_Result
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string CateID { get; set; }
        public double PriceRetail { get; set; }
        public string PriceRetailStr { get; set; }
        public string Images { get; set; }
        public string ProDescription { get; set; }
        public string Note { get; set; }
        public bool IsOnSale { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public bool Active { get; set; }
        public string URLFriendly { get; set; }
        public bool IsTarget { get; set; }
        public string ImageHover { get; set; }
        public string Gallery { get; set; }
        public int CountSaled { get; set; }
        public double Rate { get; set; }
    }
}