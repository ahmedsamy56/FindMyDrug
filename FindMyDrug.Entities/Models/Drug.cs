using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FindMyDrug.Entities.Models
{
    public class Drug
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Dosage { get; set; }
        [Required]
        public int Price { get; set; }
        public bool IsBlocked { get; set; } = false;

        [Required]
        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(Pharmacy))]
        public int? PharmacyId { get; set; }
        [ValidateNever]
        public Pharmacy Pharmacy { get; set; }

    }
}
