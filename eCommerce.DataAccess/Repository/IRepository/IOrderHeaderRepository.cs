using eCommerce.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader model);

        void UpdateStatus(int id, string orderStatus, string? paymentStatus=null);

        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
    }
}
