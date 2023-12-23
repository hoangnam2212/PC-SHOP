using CMSWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSWebsite.Controllers
{
    public class Web_CartController : Controller
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        // GET: Web_Cart
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = db.CMS_Product.SingleOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                // Xử lý khi không tìm thấy sản phẩm
                return Content("EMPTY");
            }

            var cartItem = new CartItem
            {
                ProductId = product.ProductID,
                ProductName = product.Name,
                Price = product.PriceRetail,
                PriceStr = product.PriceRetailStr,
                Quantity = quantity,
                ProductImg = product.Images,
                ProductCode = product.ProductCode,
                ProductURL = product.URLFriendly
            };

            Cart cart = GetCart();
            cart.AddItem(cartItem);
            SaveCart(cart);

            return View("CheckOut",cart);
        }

        // Các phương thức khác như RemoveFromCart, ViewCart, ...

        private Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            Session["Cart"] = cart;
        }

        public void RemoveFromCart(int proID)
        {
            var cart = GetCart();
            
            cart.RemoveItem(proID);

            SaveCart(cart);

        }
    }
}