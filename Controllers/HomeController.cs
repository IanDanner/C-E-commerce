using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private ECommerceContext _context;
 
        public HomeController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
             
            List<Product> products = _context.products.Include(make=>make.PurchaseOrders).ThenInclude(where=>where.customers).OrderByDescending(when=>when.created_at).ToList();
            List<Order> orders = _context.orders.Include(make=>make.customers).Include(where=>where.products).OrderByDescending(when=>when.created_at).ToList();
            List<Customer> customers = _context.customers.Include(make=>make.OrdersPlaced).ThenInclude(where=>where.products).OrderByDescending(when=>when.created_at).ToList();

            ViewBag.Products = products;
            ViewBag.Orders = orders;
            ViewBag.Customers = customers;

            return View();            
        }

        [HttpGet]
        [Route("products")]
        public IActionResult Products()
        {
            List<Product> products = _context.products.Include(make=>make.PurchaseOrders).ThenInclude(where=>where.customers).OrderByDescending(when=>when.created_at).ToList();
            
            ViewBag.Products = products;

            return View();            
        }

        [HttpGet]
        [HttpPost]
        [Route("create_product")]
        public IActionResult CreateProduct(ProductCreate item)
        {
            List<Product> products = _context.products.Include(make=>make.PurchaseOrders).ThenInclude(where=>where.customers).OrderByDescending(when=>when.created_at).ToList();
            
            ViewBag.Products = products;

            if(ModelState.IsValid)
            {
                Product newProd = new Product{
                name = item.name,
                image = item.image,
                description = item.description,
                quantity = item.quantity,
                };
                _context.products.Add(newProd);
                _context.SaveChanges();
            }
            return View("Products", item);            
        }

        [HttpGet]
        [Route("orders")]
        public IActionResult Orders()
        {   
            List<Product> products = _context.products.Include(make=>make.PurchaseOrders).ThenInclude(where=>where.customers).OrderByDescending(when=>when.created_at).ToList();
            List<Order> orders = _context.orders.Include(make=>make.customers).Include(where=>where.products).OrderByDescending(when=>when.created_at).ToList();
            List<Customer> customers = _context.customers.Include(make=>make.OrdersPlaced).ThenInclude(where=>where.products).OrderByDescending(when=>when.created_at).ToList();

            ViewBag.Products = products;
            ViewBag.Orders = orders;
            ViewBag.Customers = customers;
            
            return View();            
        }

        [HttpGet]
        [HttpPost]
        [Route("create_order")]
        public IActionResult CreateOrder(int custId, int prodId, int amount)
        {
            Product product = _context.products.SingleOrDefault(where=>where.id == prodId);
            Customer customer = _context.customers.SingleOrDefault(where=>where.id == custId);
            
            if(product.quantity < amount){
                TempData["errors"] = "Order Amount exceeds stock of product";
                return RedirectToAction("Orders");
            }

            if(amount == 0){
                TempData["errors"] = "Order Amount cannot be blank";
                return RedirectToAction("Orders");
            }
            
            Order newOrd = new Order{
                productsId = prodId,
                products = product,
                customersId = custId,
                customers = customer,
                quantity = amount,
            };
            _context.orders.Add(newOrd);
            product.quantity -= amount;            
            _context.SaveChanges();

            return RedirectToAction("Orders");            
        }

        [HttpGet]
        [Route("customers")]
        public IActionResult Customers()
        {
            List<Customer> customers = _context.customers.Include(make=>make.OrdersPlaced).ThenInclude(where=>where.products).OrderByDescending(when=>when.created_at).ToList();

            ViewBag.Customers = customers;
           
            return View();            
        }

        [HttpGet]
        [HttpPost]
        [Route("create_customer")]
        public IActionResult CreateCustomer(string custName)
        {
            List<Customer> customers = _context.customers.Include(make=>make.OrdersPlaced).ThenInclude(where=>where.products).OrderByDescending(when=>when.created_at).ToList();
            
            if(custName == null){
                TempData["errors"] = "Cannot add a blank name!";
                return RedirectToAction("Customers");
            }

            foreach(Customer cust in customers){
                if (cust.name == custName){
                    TempData["errors"] = "This exact name is already in the database";
                    return RedirectToAction("Customers");
                }
            }

            Customer newCust = new Customer{
                name = custName,
            };
            _context.customers.Add(newCust);
            _context.SaveChanges();
            
            return RedirectToAction("Customers");            
        }

        [HttpGet]
        [Route("delete_customer/{custId}")]
        public IActionResult DeleteCustomer(int custId)
        {   
            Customer customer = _context.customers.SingleOrDefault(del=>del.id == custId);

            List<Order> orders = _context.orders.Include(stuff=>stuff.products).Where(we => we.customersId == custId).ToList();
            foreach(Order gues in orders){
                int cancel = gues.quantity;
                gues.products.quantity += cancel;
                _context.orders.Remove(gues);
                _context.SaveChanges();
            }

            _context.customers.Remove(customer);
            _context.SaveChanges();            

            return RedirectToAction("Customers");
        }

        [HttpGet]
        [Route("settings")]
        public IActionResult Settings()
        {

            return View();            
        }
    }
}
