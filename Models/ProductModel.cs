using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        
        public Product()
        {
            PurchaseOrders = new List<Order>();
        }
        public List<Order> PurchaseOrders { get; set; }
    }
}