using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class ProductCategoryModel
    {
        public List<CMS_CateProduct> LstCate { get; set; }
    }

    public class ProductDataModel
    {
        public List<CMS_CateProduct> LstCate { get; set; }
        public List<CMS_CateProduct> LstCateNotParent { get; set; }
        public List<ProductViewModel> LstProView { get; set; }

    }
    public class ViewPhotoFrame
    {
        public List<store_GetProductSizeFrame_Result> LstFrame { get; set; }
        public double Rate { get; set; }
    }
    public class ProductViewModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string ProductCateStr { get; set; }
        public string ProductCate { get; set; }
        public double PricePrimary { get; set; }
        public string PricePrimaryStr { get; set; }
        public double PriceRetail { get; set; }
        public string PriceRetailStr { get; set; }
        public int OnHand { get; set; }
        public string Status { get; set; }
        public string ProductImg { get; set; }
        public bool IsTarget { get; set; }
    }

    public class ProductDataModal
    {
        public List<CMS_CateProduct> LstCate { get; set; }
        public CMS_Product ProEdit{get;set;}
    }

    public class ProductEditData
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
       

        public string ProductDescription { get; set; }

        public string ProductImage { get; set; }
        public int ProductID { get; set; }
        public string HidImg { get; set; }
        public string HidImg2 { get; set; }
        public string Gallery { get; set; }
        public string PriceRetailEdit { get; set; }
        public string Category { get; set; }
        public int IsSlider { get; set; }
        public int IsFeature { get; set; }
        public string ProductSummary { get; set; }
        public string PriceOri { get; set; }

    }

    public class ProductNewModel
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCate { get; set; }
        public string PricePrimary { get; set; }
        public string PriceRetail { get; set; }
        public string PriceWholeSale { get; set; }
        public string UnderOnHand { get; set; }
        public string OverOnHand { get; set; }
        public string OnHand { get; set; }
        public string ProDes { get; set; }
        public string Note { get; set; }
        public string LstAttr { get; set; }
        public string LstAttrValue { get; set; }
        public string LstCountSize { get; set; }
        public string MadeIn { get; set; }
        public string ProductImg { get; set; }
        public string ProductImg2 { get; set; }
        public string ProductGalery { get; set; }
        public string Room { get; set; }
        public string IsTarget { get; set; }
    }

}