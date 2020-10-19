using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class ActivationCodesDAL
    {
        private readonly DbGallery db;

        public ActivationCodesDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<ActivationCode> GetActivationCodes(int orderId)
        {
            List<ActivationCode> activationCodes = db.ActivationCodes.Where(x => x.OrderId == orderId).ToList();

            return activationCodes;
        }

        public void AddActivationCode(int orderId, Cart item)
        {
            db.ActivationCodes.Add(new ActivationCode
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                ProductId = item.ProductId
            });
        }
    }
}
