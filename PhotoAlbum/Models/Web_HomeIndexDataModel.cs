using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class Web_HomeIndexDataModel
    {
        public List<store_GetPostIndex_Result> LstPost { get; set; }
        public List<store_GetProductByCateID_Result> LstPhimCo { get; set; }
        public List<store_GetProductByCateID_Result> LstChuot { get; set; }
        public store_GetProductByCateID_Result SpecialChuot { get; set; }
        public store_GetProductByCateID_Result BestSeller { get; set; }
        public List<store_GetProductSlider_Result> LstSlider { get; set; }
        public List<store_GetProductFeatureHomePage_Result> LstFeaturer { get; set; }
        public List<CMS_CateProduct> LstCate { get; set; }
    }
}