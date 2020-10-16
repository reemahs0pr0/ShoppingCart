using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Db
{
    public class DbSeedData
    {
        private readonly DbGallery db;

        public DbSeedData(DbGallery db)
        {
            this.db = db;
        }

        public void Initialisation()
        {
            AddUsers();
            AddProducts();
            AddCoupons();
        }

        protected void AddUsers()
        {
            User user1 = new User
            {
                Id = "1",
                Username = "john",
                Password = "john",
                Name = "John Tan"
            };
            db.Add(user1);

            User user2 = new User
            {
                Id = "2",
                Username = "jane",
                Password = "jane",
                Name = "Jane Tan"
            };
            db.Add(user2);

            db.SaveChanges();
        }

        protected void AddProducts()
        {
            Product product1 = new Product
            {
                Image = "avast.png",
                Title = "Avast Antivirus",
                Description = "Avast Antivirus is a family of cross-platform internet security applications developed by Avast for Microsoft Windows, macOS, Android and iOS.",
                Price = 101.01,
                Link = "https://www.avast.com/en-eu/index#pc"
            };
            db.Add(product1);

            Product product2 = new Product
            {
                Image = "bitdefender.png",
                Title = "BitDefender Antivirus",
                Description = "Best antivirus protection for Windows. Bitdefender Antivirus packs the next-gen cybersecurity that won the “Product of the Year” award from AV-Comparatives.",
                Price = 101.01,
                Link = "https://www.bitdefender.com/solutions/free.html"
            };
            db.Add(product2);

            Product product3 = new Product
            {
                Image = "mcafee.png",
                Title = "McAfee Antivirus",
                Description = "McAfee provides advanced security solutions that protect data and stop threats with an open, predictive, and intelligent-driven approach to enable you to stay protected.",
                Price = 101.01,
                Link = "https://www.mcafee.com/en-us/for-home.html"
            };
            db.Add(product3);

            Product product4 = new Product
            {
                Image = "norton.png",
                Title = "Norton Antivirus",
                Description = "Norton™ provides industry-leading antivirus and security software for your PC, Mac®, and mobile devices.",
                Price = 101.01,
                Link = "https://sg.norton.com/products/norton-360?inid=nortoncom_nav_norton-360_homepage:home"
            };
            db.Add(product4);

            Product product5 = new Product
            {
                Image = "total_av.png",
                Title = "TotalAV Antivirus",
                Description = "Award winning virus protection from TotalAV. Stay 100% safe from malware and online threats with TotalAV antivirus protection.",
                Price = 101.01,
                Link = "https://www.totalav.com/en/free-antivirus"
            };
            db.Add(product5);

            Product product6 = new Product
            {
                Image = "pc_protect.png",
                Title = "PCProtect Antivirus",
                Description = "Reliability and Security. At its core, PCProtect is still an antivirus, despite all the additional tools and features.",
                Price = 101.01,
                Link = "https://www.pcprotect.com/software"
            };
            db.Add(product6);

            db.SaveChanges();
        }

        protected void AddCoupons()
        {
            for (int i = 1; i < 20; i++)
            {
                Coupon coupon = new Coupon
                {
                    Id = "coupon" + i,
                };
                db.Add(coupon);
            }

            db.SaveChanges();
        }
    }
}
