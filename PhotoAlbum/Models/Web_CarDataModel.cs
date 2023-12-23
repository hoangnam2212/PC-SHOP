using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class Web_CarDataModel
    {
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string PriceStr { get; set; }
        public int Quantity { get; set; }
        public string ProductCode { get; set; }
        public string ProductImg { get; set; }
        public string ProductURL { get; set; }

    }
    public class Cart
    {
        public List<CartItem> Items { get; private set; }

        public Cart()
        {
            Items = new List<CartItem>();
        }

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public double TotalPrice()
        {
            return Items.Sum(i => i.Price * i.Quantity);
        }
    }
}