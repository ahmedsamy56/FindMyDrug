using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyDrug.Entities.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }    
        IPharmacyRepository Pharmacy { get; }
        IDrugRepository Drug { get; }
        int Complete();
    }
}
