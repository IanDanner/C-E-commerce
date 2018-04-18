using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class Order : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public int quantity { get; set; }
        public int productsId { get; set; }
        public Product products { get; set; }
        public int customersId { get; set; }
        public Customer customers { get; set; }
        
    }
}