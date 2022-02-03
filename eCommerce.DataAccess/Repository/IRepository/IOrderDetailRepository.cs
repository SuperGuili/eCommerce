using eCommerce.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail model);
    }
}
