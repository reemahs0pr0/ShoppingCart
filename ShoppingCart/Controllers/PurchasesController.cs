using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Db;
using ShoppingCart.DAL;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly DbGallery db;
        private readonly OrdersDAL ordersDAL;
        private readonly OrderDetailsDAL orderdetailsDAL;
        private readonly ActivationCodesDAL activationcodesDAL;
        private readonly CartsDAL cartsDAL;

        public PurchasesController(DbGallery db)
        {
            this.db = db;
            ordersDAL = new OrdersDAL(db);
            orderdetailsDAL = new OrderDetailsDAL(db);
            activationcodesDAL = new ActivationCodesDAL(db);
            cartsDAL = new CartsDAL(db);
        }

        //when user click 'My Purchases'
        public IActionResult DisplayPurchases()
        {
            PurchasesViewModel purchasesViewModel = new PurchasesViewModel();

            // data of past purchases will be added into a list here
            List<Purchases> purchases = FinalList();

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";
            purchasesViewModel.Purchases = purchases;
            purchasesViewModel.Total = TempData["total"] as string;

            return View(purchasesViewModel);
        }
        public List<Purchases> FinalList()
        {
            //get list of all purchases and activation codes from db
            List<Order> orders = ordersDAL.GetOrders(HttpContext.Session.GetString("userid"));
            List<OrderDetail> orderDetails;
            List<ActivationCode> activationCodes;
            List<Purchases> purchases = new List<Purchases>();
            List<ActivationCode> activationCodesOverall = new List<ActivationCode>();
            foreach(Order order in orders)
            {
                orderDetails = orderdetailsDAL.GetOrderDetails(order.Id);
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
                activationCodes = activationcodesDAL.GetActivationCodes(order.Id);
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
            List<Cart> cart = cartsDAL.GetCart(HttpContext.Session.GetString("userid"));
            double total = GetTotalPaid(cart);

            //add order based on cart
            ordersDAL.AddOrder(HttpContext.Session.GetString("userid"));

            int orderId = ordersDAL.GetOrderId(HttpContext.Session.GetString("userid"));
            foreach(Cart item in cart)
            {
                //add order details based on cart
                orderdetailsDAL.AddOrderDetails(orderId, item);

                for (int i = 0; i < item.Quantity; i++)
                {
                    //create activation code for every purchased item and store in db
                    activationcodesDAL.AddActivationCode(orderId, item);
                }

                //erase all records from cart
                cartsDAL.RemoveItem(item);
            }

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
