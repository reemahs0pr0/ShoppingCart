using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Db;
using System.Security.Cryptography.X509Certificates;

namespace ShoppingCart.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly DbGallery db;

        public PurchasesController(DbGallery db)
        {
            this.db = db;
        }

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
            List<Order> orders = db.Orders.Where(x => x.UserId == HttpContext.Session.GetString("userid")).OrderByDescending(x => x.Id).ToList();
            List<OrderDetail> orderDetails;
            List<ActivationCode> activationCodes;
            List<Purchases> purchases = new List<Purchases>();
            List<ActivationCode> activationCodesOverall = new List<ActivationCode>();
            foreach(Order order in orders)
            {
                orderDetails = db.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
                foreach (OrderDetail od in orderDetails)
                {
                    purchases.Add(new Purchases
                    {
                        OrderId = order.Id,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        PurchaseDate = order.PurchaseDate,
                        Image = od.Product.Image,
                        Title = od.Product.Title,
                        Description = od.Product.Description,
                        Link = od.Product.Link
                    });
                }
                activationCodes = db.ActivationCodes.Where(x => x.OrderId == order.Id).ToList();
                foreach (ActivationCode code in activationCodes)
                {
                    activationCodesOverall.Add(new ActivationCode
                    {
                        Id = code.Id,
                        OrderId = order.Id,
                        ProductId = code.ProductId
                    });
                }
            }

            // combines the purchases and activation list
            foreach (Purchases purchase in purchases)
            {
                foreach (ActivationCode code in activationCodesOverall)
                {
                    if (code.OrderId == purchase.OrderId && code.ProductId == purchase.ProductId)
                    {
                        purchase.ActivationCode.Add(code.Id);
                    }
                }
            }
            return purchases;
        }

        //when user has logged in and click 'checkout'
        public IActionResult DisplayNewPurchases()
        {
            //get records from cart
            List<Cart> cart = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid")).ToList();
            double total = GetTotalPaid(cart);

            //add order based on cart
            db.Orders.Add(new Order
            {
                UserId = HttpContext.Session.GetString("userid"),
                PurchaseDate = DateTime.Now.ToString("dd-MM-yyyy")
            });

            int orderId = db.Orders.Where(x => x.UserId == HttpContext.Session.GetString("userid")).Max(x => x.Id);
            foreach(Cart item in cart)
            {
                //add order details based on cart
                db.OrderDetails.Add(new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });

                for (int i = 0; i < item.Quantity; i++)
                {
                    //create activation code for every purchased item and store in db
                    db.ActivationCodes.Add(new ActivationCode
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderId = orderId,
                        ProductId = item.ProductId
                    });
                }

                //erase all records from cart
                db.Carts.Remove(item);
            }
            db.SaveChanges();

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
                total += item.Product.Price * item.Quantity;
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
