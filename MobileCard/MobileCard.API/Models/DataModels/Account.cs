using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class AccountViewModel
    {
        [Required] public string Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }

        public string FullName => $"{FirstName}{(string.IsNullOrWhiteSpace(MiddleName) ? "" : " " + MiddleName)} {LastName}";

        [Required] public string MatricNumber { get; set; }
        [Required] public string Gender { get; set; }
        [Required, Phone] public string PhoneNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Department { get; set; }
        [Required] public string Faculty { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string NextOfKin { get; set; }
        [Required] public string NextOfKinPhoneNumber { get; set; }
    }
}
