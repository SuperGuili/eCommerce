using eCommerce.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        void Update(Tag model);
    }
}
