﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Db;
using System.Threading;
using ShoppingCart.DAL;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbGallery db;
        private readonly ProductsDAL productsDAL;
        private readonly CartsDAL cartsDAL;
        private readonly WishlistsDAL wishlistsDAL;

        public ProductController(DbGallery db)
        {
            this.db = db;
            productsDAL = new ProductsDAL(db);
            cartsDAL = new CartsDAL(db);
            wishlistsDAL = new WishlistsDAL(db);
        }

        public IActionResult DisplayProduct()
        {
            ProductViewModel productViewModel = new ProductViewModel();

            //create product list to store items details
            List<Product> productlists = productsDAL.GetAllProducts();
            productViewModel.Products = productlists;

            int order = productsDAL.GetNoOfOrders();
            if (order != 0)
            {
                //to get top selling based on past purchases and code implementation
                List<Purchases> topsellingproduct = new List<Purchases>();
                var list = productsDAL.GetTopSellingProduct();
                foreach (var product in list)
                {
                    topsellingproduct.Add(new Purchases
                    {
                        ProductId = product.Key,
                        Quantity = product.TotalQty
                    });
                }

                productViewModel.TopProducts = topsellingproduct;

                //to get list of top 3 unique quantity values
                List<int> topthreeqty = thirdLargest(topsellingproduct);
                productViewModel.Top3Qty = topthreeqty;
            }

            //check if there is any pre-existing item in cart
            int count = cartsDAL.CheckLastInCart(HttpContext.Session.GetString("userid"));

            //get WishList if logged in
            List<Wishlist> wishlist = new List<Wishlist>();
            if (HttpContext.Session.GetString("name") != null)
            {
                wishlist = wishlistsDAL.GetWishList(HttpContext.Session.GetString("userid"));
            }

            //send data to View
            productViewModel.Wishlists = wishlist;
            productViewModel.Count = count;
            ViewBag.a = 1; //indicator to display all products

            //pass name, if any, to HTML
            productViewModel.Name = HttpContext.Session.GetString("name");

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            if (search == null)
                return RedirectToAction("DisplayProduct");

            ProductViewModel productViewModel = new ProductViewModel();

            //list of searched products from db based on description
            List<Product> searchedproductlists = productsDAL.GetProductSearch(search);

            if (searchedproductlists.Count == 0)
            {
                //check if there is any pre-existing item in cart
                int count = cartsDAL.CheckLastInCart(HttpContext.Session.GetString("userid"));

                //send data to View
                productViewModel.Count = count;
                ViewData["search"] = search;
                ViewBag.a = 3; //indicator if search product not found

                //pass username, if any, to HTML
                productViewModel.Name = HttpContext.Session.GetString("name");

                // to highlight "Shopping" as the selected menu-item
                ViewData["Is_Shopping"] = "menu_hilite";
            }
            else
            {
                int order = productsDAL.GetNoOfOrders();
                if (order != 0)
                {
                    //to get top selling based on past purchases and code implementation
                    List<Purchases> topsellingproduct = new List<Purchases>();
                    var list = productsDAL.GetTopSellingProduct();
                    foreach (var product in list)
                    {
                        topsellingproduct.Add(new Purchases
                        {
                            ProductId = product.Key,
                            Quantity = product.TotalQty
                        });
                    }

                    productViewModel.TopProducts = topsellingproduct;

                    //to get list of top 3 unique quantity values
                    List<int> topthreeqty = thirdLargest(topsellingproduct);
                    productViewModel.Top3Qty = topthreeqty;
                }

                //check if there is any pre-existing item in cart
                int count = cartsDAL.CheckLastInCart(HttpContext.Session.GetString("userid"));

                //get WishList if logged in
                List<Wishlist> wishlist = new List<Wishlist>();
                if (HttpContext.Session.GetString("name") != null)
                {
                    wishlist = wishlistsDAL.GetWishList(HttpContext.Session.GetString("userid"));
                }

                //send data to View
                productViewModel.Wishlists = wishlist;
                productViewModel.Count = count;
                ViewData["search"] = search;
                productViewModel.SearchedProducts = searchedproductlists;
                ViewBag.a = 2; //indicator to display searched products

                //pass name, if any, to HTML
                productViewModel.Name = HttpContext.Session.GetString("name");

                // to highlight "Shopping" as the selected menu-item
                ViewData["Is_Shopping"] = "menu_hilite";
            }
            return View("DisplayProduct", productViewModel);
        }

        static List<int> thirdLargest(List<Purchases> topsellingproduct)
        {
            List<int> topthreehighestqty = new List<int>();

            //find first largest quantity value 
            int first = topsellingproduct[0].Quantity;

            for (int i = 1; i < topsellingproduct.Count; i++)
            {
                if (topsellingproduct[i].Quantity > first)
                    first = topsellingproduct[i].Quantity;
            }

            topthreehighestqty.Add(first);

            //find second largest quantity value 
            int second = 0;

            for (int i = 0; i < topsellingproduct.Count; i++)
                if (topsellingproduct[i].Quantity > second && topsellingproduct[i].Quantity < first)
                    second = topsellingproduct[i].Quantity;

            topthreehighestqty.Add(second);

            //find third largest quantity value 
            int third = 0;

            for (int i = 0; i < topsellingproduct.Count; i++)
            {
                if (topsellingproduct[i].Quantity > third && topsellingproduct[i].Quantity < second)
                    third = topsellingproduct[i].Quantity;
            }

            topthreehighestqty.Add(third);

            return topthreehighestqty;
        }
    }
}
