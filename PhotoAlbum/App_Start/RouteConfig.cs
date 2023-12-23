using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "ViewProducts",
                url: "san-pham/{seourl}-{id}",
                defaults: new
                {
                    controller = "Web_Router",
                    action = "RouterProduct",
                    seourl = UrlParameter.Optional,
                    id = UrlParameter.Optional
                }
           );


            routes.MapRoute(
                name: "ViewListProducts",
                 url: "danh-muc/{seourl}",
                 defaults: new
                 {
                     controller = "Web_Router",
                     action = "RouterListProduct",
                     seourl = UrlParameter.Optional
                 }
            );

            routes.MapRoute(
              name: "Tin tuc",
              url: "tin-tuc/{seourl}-{id}",
              defaults: new
              {
                  controller = "Web_Router",
                  action = "Post",
                  seourl = UrlParameter.Optional,
                  id = UrlParameter.Optional
              }
          );
            routes.MapRoute(
            name: "DanhMucBaiViet",
            url: "blog/{seourl}",
            defaults: new
            {
                controller = "Web_Router",
                action = "ListPost",
                seourl = UrlParameter.Optional
            }
        );
            routes.MapRoute(
                name: "Admin",
                url: "dang-nhap",
                defaults: new { controller = "CMS_Home", action = "Dashboard", id = UrlParameter.Optional }
            );

            routes.MapRoute(
           name: "RouterPage",
           url: "pcshop/{seourl}",
           defaults: new
           {
               controller = "Web_Router",
               action = "Index",
               seourl = UrlParameter.Optional
           }
       );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Web_Home", action = "Index", id = UrlParameter.Optional }
                 //defaults: new { controller = "CMS_Home", action = "Dashboard", id = UrlParameter.Optional }
            );
        }
    }
}
