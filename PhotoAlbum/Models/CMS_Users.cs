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
    using System.Collections.Generic;
    
    public partial class CMS_Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string ImgFace { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> Banned { get; set; }
        public bool Active { get; set; }
    }
}
