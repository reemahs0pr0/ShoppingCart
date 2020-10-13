using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Data;
using Microsoft.AspNetCore.Http;

namespace ShoppingCart.Controllers
{
    public class PurchasesController : Controller
    {
        //when user click 'My Purchases'
        public IActionResult DisplayPurchases()
        {
            // data of past purchases will be added into a list here
            List<Purchases> purchases = FinalList();
            ViewData["purchases"] = purchases;

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            ViewData["total"] = TempData["total"] as string;

            return View();
        }
        public List<Purchases> FinalList()
        {
            //get list of all purchases and activation codes from db
            List<Purchases> purchases = PurchasesData.GetPurchases(HttpContext.Session.GetString("userid"));
            List<ActivationCode> codes = PurchasesData.GetActivationCodes(HttpContext.Session.GetString("userid"));


            // combines the purchases and activation list
            foreach (Purchases purchase in purchases)
            {
                foreach (ActivationCode code in codes)
                {
                    if (code.OrderId == purchase.OrderId && code.ProductId == purchase.ProductId)
                    {
                        purchase.ActivationCode.Add((string)code.Code);
                    }
                }
            }
            return purchases;
        }

        //when user has logged in and click 'checkout'
        public IActionResult DisplayNewPurchases()
        {
            //get records from cart
            List<Cart> cart = CartData.GetCart(HttpContext.Session.GetString("userid"));
            double total = GetTotalPaid(cart);

            //add order and ordetails records based on cart
            PurchasesData.AddOrder(HttpContext.Session.GetString("userid"));
            PurchasesData.AddOrderDetails(HttpContext.Session.GetString("userid"));

            //create activation code for every purchased item and store in db
            foreach (Cart item in cart)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    ActivationCode code = new ActivationCode
                    {
                        Code = Guid.NewGuid().ToString(),
                        ProductId = item.Id
                    };
                    PurchasesData.AddActivationCode(code, HttpContext.Session.GetString("userid"));
                }
            }

            //erase all records from cart
            CartData.DeleteCart(HttpContext.Session.GetString("userid"));

            TempData["total"] = total.ToString("#0.00");

            return RedirectToAction("DisplayPurchases");
        }

        public double GetTotalPaid(List<Cart> cart)
        {
            //create variable to store total price
            double total = 0;

            //add price of each item into total
            foreach (var item in cart)
            {
                total += item.Price * item.Quantity;
            }

            if (HttpContext.Session.GetString("couponcode") == null)
            {
                return total;
            }
            else
            {
                return total * 0.9;
            }
        }
    }
}
