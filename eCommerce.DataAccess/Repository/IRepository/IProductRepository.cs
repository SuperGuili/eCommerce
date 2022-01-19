using eCommerce.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product model);
    }
}
