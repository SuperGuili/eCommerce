using eCommerce.DataAccess.Repository.IRepository;

namespace eCommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Tag = new TagRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }

        public ITagRepository Tag { get; private set;}

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
