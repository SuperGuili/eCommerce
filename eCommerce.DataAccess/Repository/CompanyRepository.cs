using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company> ,ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            _db = applicationDb;
        }

        public void Update(Company model)
        {
            _db.Companies.Update(model);
        }
    }
}
