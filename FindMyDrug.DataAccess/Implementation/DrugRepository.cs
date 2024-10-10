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
    public class DrugRepository : GenericRepository<Drug>, IDrugRepository
    {
        private readonly AppDbContext _context;
        public DrugRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public void ToggleAvailability(int id)
        {
           var drugInDb = _context.drugs.FirstOrDefault(x => x.Id == id);
            if (drugInDb != null)
            {
                drugInDb.IsAvailable = !drugInDb.IsAvailable;
            }
        }

        public void ToggleBlockStatus(int id)
        {
            var drugInDb = _context.drugs.FirstOrDefault(x => x.Id == id);
            if (drugInDb != null)
            {
                drugInDb.IsBlocked = !drugInDb.IsBlocked;
            }
        }

        public void Update(Drug drug)
        {
            var drugInDb = _context.drugs.FirstOrDefault(x => x.Id == drug.Id);
            if (drugInDb != null)
            {
                drugInDb.Name = drug.Name;
                drugInDb.Dosage = drug.Dosage;
                drugInDb.Price = drug.Price;
            }
        }


    }
}
