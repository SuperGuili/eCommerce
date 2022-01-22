using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;

namespace eCommerce.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product model)
        {
            Product productFromDb = _db.Products.FirstOrDefault(x => x.Id == model.Id);

            if (productFromDb != null)
            {
                productFromDb.ProductName = model.ProductName;
                productFromDb.ProductDescription = model.ProductDescription;
                productFromDb.ListPrice = model.ListPrice;
                productFromDb.Price = model.Price;
                productFromDb.Price10 = model.Price10;
                productFromDb.Price20 = model.Price20;
                productFromDb.StockQuantity = model.StockQuantity;
                productFromDb.CategoryId = model.CategoryId;
                productFromDb.TagId = model.TagId;
                productFromDb.TagId2 = model.TagId2;
                productFromDb.TagId3 = model.TagId3;
                productFromDb.Brand = model.Brand;

                if (model.ImageUrl != null)
                {
                    productFromDb.ImageUrl = model.ImageUrl;
                }
                //_db.Products.Update(productFromDb);
            }

        }
    }
}
