using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class CMS_PortfolioDataModel
    {
    }
    public class PortfolioCategoryModel
    {
        public List<Web_CategoryPortfolio> LstCate { get; set; }
    }

    public class PortfolioModel
    {
        public string PortfolioDate { get; set; }
        public string PortfolioAddress { get; set; }
        public string ImageGallery { get; set; }
        public string PortfolioId { get; set; }
        public string Title { get; set; }
        public string ContentPortfolio { get; set; }
        public string CatePortfolio { get; set; }
        public string Image { get; set; }
        public string SeoDesc { get; set; }
        public string SeoKeyword { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class PortfolioView
    {
        public string UrlFriendly { get; set; }
        public int PortfolioId { get; set; }
        public int PortfolioAuthorId { get; set; }
        public string PortfolioTitle { get; set; }
        public string PortfolioDate { get; set; }
        public string PortfolioCate { get; set; }
        public string PortfolioAuthor { get; set; }
        public string PortfolioDesc { get; set; }
        public string ImgFace { get; set; }
    }

}