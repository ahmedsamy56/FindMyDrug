using FindMyDrug.DataAccess.Data;
using FindMyDrug.Entities.Models;
using FindMyDrug.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyDrug.DataAccess.Implementation
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser> , IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
