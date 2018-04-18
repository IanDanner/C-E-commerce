using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class ProductCreate
    {   
        [Required]
        [MinLength(2)]
        [Display(Name = "Product Name")]      
        public string name { get; set; }
        [Required]
        [Display(Name = "Product Image Url")]
        public string image { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }
        [Required]
        [Display(Name = "Initial Quanity")]
        public int quantity { get; set; }
        
    }
}