using FindMyDrug.DataAccess.Data;
using FindMyDrug.Entities.Models;
using FindMyDrug.Entities.Repositories;

namespace FindMyDrug.DataAccess.Implementation
{
    public class PharmacyRepository : GenericRepository<Pharmacy>, IPharmacyRepository
    {
        private readonly AppDbContext _context;
        public PharmacyRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public void Update(Pharmacy pharmacy)
        {
            var pharmacyInDb = _context.pharmacies.FirstOrDefault(x => x.Id == pharmacy.Id);
            if (pharmacyInDb != null)
            {
                pharmacyInDb.Name = pharmacy.Name;
                pharmacyInDb.Address = pharmacy.Address;
                pharmacyInDb.Location = pharmacy.Location;
                pharmacyInDb.HotLine = pharmacy.HotLine;
                pharmacyInDb.OpenHour = pharmacy.OpenHour;
                pharmacyInDb.CloseHour = pharmacy.CloseHour;
                pharmacyInDb.IsOpen24Hrs = pharmacy.IsOpen24Hrs;
                pharmacyInDb.Img = pharmacy.Img;
            }
        }
    }
}
