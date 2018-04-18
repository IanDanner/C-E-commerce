using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class Customer : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public Customer()
        {
            OrdersPlaced = new List<Order>();
        }
        public List<Order> OrdersPlaced { get; set; }
    }
}