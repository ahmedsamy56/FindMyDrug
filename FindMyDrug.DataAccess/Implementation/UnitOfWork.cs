using FindMyDrug.DataAccess.Data;
using FindMyDrug.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyDrug.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        public IApplicationUserRepository ApplicationUser {  get; private set; }

        public IPharmacyRepository Pharmacy { get; private set; }

        public IDrugRepository Drug { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
            ApplicationUser = new ApplicationUserRepository(context);
            Drug = new DrugRepository(context);
            Pharmacy = new PharmacyRepository(context);

        }

        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
