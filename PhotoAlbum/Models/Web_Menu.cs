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
    
    public partial class Web_Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Icon { get; set; }
        public Nullable<int> MenuTypeId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> MenuOrder { get; set; }
        public string MenuTarget { get; set; }
    }
}
