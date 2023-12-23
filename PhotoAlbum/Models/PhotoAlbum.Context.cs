﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PhotoAlbumEntities : DbContext
    {
        public PhotoAlbumEntities()
            : base("name=PhotoAlbumEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CMS_CateProduct> CMS_CateProduct { get; set; }
        public virtual DbSet<CMS_Log> CMS_Log { get; set; }
        public virtual DbSet<CMS_Menu> CMS_Menu { get; set; }
        public virtual DbSet<CMS_MenuRole> CMS_MenuRole { get; set; }
        public virtual DbSet<CMS_Role> CMS_Role { get; set; }
        public virtual DbSet<CMS_UserRole> CMS_UserRole { get; set; }
        public virtual DbSet<CMS_Users> CMS_Users { get; set; }
        public virtual DbSet<Web_Banner> Web_Banner { get; set; }
        public virtual DbSet<Web_Category> Web_Category { get; set; }
        public virtual DbSet<Web_CategoryPortfolio> Web_CategoryPortfolio { get; set; }
        public virtual DbSet<Web_Config> Web_Config { get; set; }
        public virtual DbSet<Web_Menu> Web_Menu { get; set; }
        public virtual DbSet<Web_MenuType> Web_MenuType { get; set; }
        public virtual DbSet<Web_Portfolio> Web_Portfolio { get; set; }
        public virtual DbSet<Web_PortfolioCategory> Web_PortfolioCategory { get; set; }
        public virtual DbSet<Web_Post> Web_Post { get; set; }
        public virtual DbSet<Web_PostCategory> Web_PostCategory { get; set; }
        public virtual DbSet<Web_Tags> Web_Tags { get; set; }
        public virtual DbSet<CMS_Product> CMS_Product { get; set; }
    
        public virtual ObjectResult<store_GetMenuByUserID_Result> store_GetMenuByUserID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetMenuByUserID_Result>("store_GetMenuByUserID", userIDParameter);
        }
    
        public virtual ObjectResult<store_GetPostByCateID_Result> store_GetPostByCateID(Nullable<int> cateID)
        {
            var cateIDParameter = cateID.HasValue ?
                new ObjectParameter("cateID", cateID) :
                new ObjectParameter("cateID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetPostByCateID_Result>("store_GetPostByCateID", cateIDParameter);
        }
    
        public virtual ObjectResult<store_GetPostPopular_Result> store_GetPostPopular()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetPostPopular_Result>("store_GetPostPopular");
        }
    
        public virtual ObjectResult<store_GetPostSection2_Result> store_GetPostSection2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetPostSection2_Result>("store_GetPostSection2");
        }
    
        public virtual ObjectResult<store_GetPostSlider_Result> store_GetPostSlider()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetPostSlider_Result>("store_GetPostSlider");
        }
    
        public virtual ObjectResult<store_GetTags_Result> store_GetTags()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetTags_Result>("store_GetTags");
        }
    
        public virtual ObjectResult<store_GetPostIndex_Result> store_GetPostIndex()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetPostIndex_Result>("store_GetPostIndex");
        }
    
        public virtual ObjectResult<store_GetProductSizeFrame_Result> store_GetProductSizeFrame(Nullable<int> proid)
        {
            var proidParameter = proid.HasValue ?
                new ObjectParameter("proid", proid) :
                new ObjectParameter("proid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetProductSizeFrame_Result>("store_GetProductSizeFrame", proidParameter);
        }
    
        public virtual ObjectResult<store_GetProductTopSaleHomePage_Result> store_GetProductTopSaleHomePage()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetProductTopSaleHomePage_Result>("store_GetProductTopSaleHomePage");
        }
    
        public virtual ObjectResult<store_GetProductSlider_Result> store_GetProductSlider()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetProductSlider_Result>("store_GetProductSlider");
        }
    
        public virtual ObjectResult<store_GetProductFeatureHomePage_Result> store_GetProductFeatureHomePage()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetProductFeatureHomePage_Result>("store_GetProductFeatureHomePage");
        }
    
        public virtual ObjectResult<store_GetProductByCateID_Result> store_GetProductByCateID(string cateID)
        {
            var cateIDParameter = cateID != null ?
                new ObjectParameter("cateID", cateID) :
                new ObjectParameter("cateID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<store_GetProductByCateID_Result>("store_GetProductByCateID", cateIDParameter);
        }
    }
}