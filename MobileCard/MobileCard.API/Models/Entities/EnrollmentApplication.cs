using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCard.API.Models.Entities
{
    public class EnrollmentApplication
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public string Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string MatricNumber { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Department { get; set; }
        [Required] public string Faculty { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string NextOfKin { get; set; }
        [Required] public string NextOfKinPhoneNumber { get; set; }
    }
}
