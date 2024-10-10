using FindMyDrug.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyDrug.Entities.Repositories
{
    public interface IPharmacyRepository : IGenericRepository<Pharmacy>
    {
        public void Update(Pharmacy pharmacy);
    }
}
