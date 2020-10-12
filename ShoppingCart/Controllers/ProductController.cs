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
            //check if logged in
            ViewData["loggedin"] = Int32.TryParse(HttpContext.Session.GetString("userid"),
                out int userId);

            //create product list to store items details
            List<Product> productlists = ProductData.GetAllProducts();

            //get WishList if logged in
            List<Product> wishlist = new List<Product>();
            if (Int32.TryParse(HttpContext.Session.GetString("userid"),
              out int userId2) == true)
            {
                wishlist = WishData.GetWishList(productlists, userId2);
            }

            ViewData["wishlist"] = wishlist;
            //to get top selling based on past purchases and code implementation
            List<Product> topsellingproduct = ProductData.GetTopSellingProduct();

            //first 3 elements are top 3 items purchased
            Product bestsellingproduct = topsellingproduct[0];
            Product secondbestproduct = topsellingproduct[1];
            Product thirdbestproduct = topsellingproduct[2];

            //send top selling products to View
            ViewData["bestsellingproduct"] = bestsellingproduct;
            ViewData["secondbestproduct"] = secondbestproduct;
            ViewData["thirdbestproduct"] = thirdbestproduct;

            //check if there is any pre-existing item in cart
            int count = CartData.CheckLastInCart(HttpContext.Session.GetString("userid"));

            //send data to View
            ViewData["count"] = count;
            ViewData["productlists"] = productlists;
            ViewBag.a = 1; //indicator to display all products

            //pass name, if any, to HTML
            ViewData["name"] = HttpContext.Session.GetString("name");

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            //list of searched products from db based on description
            List<Product> searchedproductlists = ProductData.GetProductSearch(search);

            if (searchedproductlists.Count == 0)
            {
                //to get top selling based on past purchases and code implementation
                List<Product> topsellingproduct = ProductData.GetTopSellingProduct();

                //first 3 elements are top 3 items purchased
                Product bestsellingproduct = topsellingproduct[0];
                Product secondbestproduct = topsellingproduct[1];
                Product thirdbestproduct = topsellingproduct[2];

                //send top selling products to View
                ViewData["bestsellingproduct"] = bestsellingproduct;
                ViewData["secondbestproduct"] = secondbestproduct;
                ViewData["thirdbestproduct"] = thirdbestproduct;

                //check if there is any pre-existing item in cart
                int count = CartData.CheckLastInCart(HttpContext.Session.GetString("userid"));

                //send data to View
                ViewData["count"] = count;
                ViewData["search"] = search;
                ViewBag.a = 3; //indicator if search product not found

                //pass username, if any, to HTML
                ViewData["name"] = HttpContext.Session.GetString("name");

                // to highlight "Shopping" as the selected menu-item
                ViewData["Is_Shopping"] = "menu_hilite";
            }
            else
            {
                //to get top selling based on past purchases and code implementation
                List<Product> topsellingproduct = ProductData.GetTopSellingProduct();

                //first 3 elements are top 3 items purchased
                Product bestsellingproduct = topsellingproduct[0];
                Product secondbestproduct = topsellingproduct[1];
                Product thirdbestproduct = topsellingproduct[2];

                //send top selling products to View
                ViewData["bestsellingproduct"] = bestsellingproduct;
                ViewData["secondbestproduct"] = secondbestproduct;
                ViewData["thirdbestproduct"] = thirdbestproduct;

                //check if there is any pre-existing item in cart
                int count = CartData.CheckLastInCart(HttpContext.Session.GetString("userid"));

                //send data to View
                ViewData["count"] = count;
                ViewData["search"] = search;
                ViewData["foundproducts"] = searchedproductlists;
                ViewBag.a = 2; //indicator to display searched products

                //pass name, if any, to HTML
                ViewData["name"] = HttpContext.Session.GetString("name");

                // to highlight "Shopping" as the selected menu-item
                ViewData["Is_Shopping"] = "menu_hilite";
            }
            return View("DisplayProduct");
        }
    }
}
