using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;

namespace eCommerce.DataAccess.Repository
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _db;

        public TagRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Tag model)
        {
            _db.Tags.Update(model);
        }
    }
}
