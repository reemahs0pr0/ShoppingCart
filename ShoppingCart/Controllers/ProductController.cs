using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Data;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult DisplayProduct()
        {
            //create product list to store items details
            List<Product> productlists = ProductData.GetAllProducts();

            int count = CartData.CheckLastInCart(HttpContext.Session.GetString("userid"));

            //send data to View
            ViewData["count"] = count;
            ViewData["productlists"] = productlists;
            ViewBag.a = 1; //indicator to display all products

            //pass username, if any, to HTML
            ViewData["name"] = HttpContext.Session.GetString("name");

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            List<Product> searchedproductlists = ProductData.GetProductSearch(search);
            if (searchedproductlists.Count == 0)
            {
                //pass username, if any, to HTML
                ViewData["name"] = HttpContext.Session.GetString("name");

                //send data to View
                ViewData["search"] = search;
                ViewBag.a = 3; //indicator to display searched products
            }
            else
            {
                //pass name, if any, to HTML
                ViewData["name"] = HttpContext.Session.GetString("name");

                //send data to View
                ViewData["search"] = search;
                ViewData["foundproducts"] = searchedproductlists;
                ViewBag.a = 2; //indicator to display searched products
            }
            return View("DisplayProduct");
        }
    }
}
