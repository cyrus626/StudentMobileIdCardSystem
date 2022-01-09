using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class LoginViewModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
    

    public class AccountEnrollmentViewModel
    {
        [Required] public string FirstName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }
        
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

    public class AccessTokenViewModel
    {
        [Required] public string AccessToken { get; set; }
    }
}
