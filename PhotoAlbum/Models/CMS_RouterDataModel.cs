using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class CMS_RouterDataModel
    {
    }
    public class DetailProductModel
    {
        public CMS_Product Product { get; set; }
        public List<store_GetProductByCateID_Result> RelateProduct { get; set; }
        public List<store_GetProductTopSaleHomePage_Result> LstTopSale { get; set; }
        public double Rate { get; set; }
    }

    public class DetailPostModel
    {
        public Web_Post Post { get; set; }
        public List<store_GetPostByCateID_Result> LstRelate { get; set; }
        public List<Web_Category> LstCate { get; set; }
        public Web_Category CatePost { get; set; }
        public Web_Post Next { get; set; }
        public Web_Post Previous { get; set; }
        public CMS_Users Author { get; set; }
        public List<store_GetTags_Result> LstTags { get; set; }

    }
    public class ListProductsModel
    {
        public List<store_GetProductByCateID_Result> LstProducts { get; set; }
        public CMS_CateProduct Cate { get; set; }
    }
    // Danh mục bài viết
    public class ListPostModel
    {
        public List<Web_Post> LstPost { get; set; }
        public Web_Category Cate { get; set; }
        public List<Web_Category> LstCate { get; set; }
    }
}