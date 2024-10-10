using FindMyDrug.Entities.Models;

namespace FindMyDrug.Entities.Repositories
{
    public interface IDrugRepository : IGenericRepository<Drug>
    {
        public void Update(Drug drug);
        void ToggleAvailability(int id);
        void ToggleBlockStatus(int id);
    }
}
