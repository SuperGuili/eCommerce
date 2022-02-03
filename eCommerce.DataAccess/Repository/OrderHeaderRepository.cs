using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader model)
        {
            _db.OrderHeaders.Update(model);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            OrderHeader orderHeaderFromDB = _db.OrderHeaders.FirstOrDefault(o => o.Id == id);

            if (orderHeaderFromDB != null)
            {
                orderHeaderFromDB.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderHeaderFromDB.PaymentStatus = paymentStatus;
                }
            }
        }
    }
}
