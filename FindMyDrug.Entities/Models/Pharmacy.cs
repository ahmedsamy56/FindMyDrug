using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindMyDrug.Entities.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } 
        public string HotLine { get; set; }

        [ValidateNever]
        public string Img { get; set; }
        [RegularExpression(@"<iframe\s+src=""https:\/\/www\.google\.com\/maps\/embed\?pb=.*?""\s+width=""\d+""\s+height=""\d+""\s+style=""[^""]*""\s+allowfullscreen=""""\s+loading=""lazy""\s+referrerpolicy=""no-referrer-when-downgrade""><\/iframe>",
                               ErrorMessage = "The location must be a valid Google Maps iframe.")]
        public string Location { get; set; }
        [Column(TypeName = "TIME")]
        [ValidateNever]
        public TimeSpan? OpenHour { get; set; }
        [Column(TypeName = "TIME")]
        [ValidateNever]
        public TimeSpan? CloseHour { get; set; }
        public bool IsOpen24Hrs { get; set; } = false;



        [ForeignKey(nameof(ApplicationUser))]
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }
        [ValidateNever]
        public List<Drug>? drugs { get; set; }
    }
}
