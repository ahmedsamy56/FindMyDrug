using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FindMyDrug.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [ValidateNever]
        public List<Pharmacy>? pharmacies { get; set; }
    }
}
