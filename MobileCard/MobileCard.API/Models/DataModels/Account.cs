using MobileCard.API.Models.Entities;
using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class EnrollmentAccountViewModel
    {
        [Required] 
        public string Id { get; set; }
        
        [Required, Sieve(CanFilter = true, CanSort = true)] 
        public string FirstName { get; set; }
        
        [Required, Sieve(CanFilter = true, CanSort = true)]
        public string MiddleName { get; set; }
        [Required, Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName}{(string.IsNullOrWhiteSpace(MiddleName) ? "" : " " + MiddleName)} {LastName}";

        [Required, Sieve(CanFilter = true, CanSort = true)]
        public string MatricNumber { get; set; }
        
        [Required, Sieve(CanSort = true)]
        public string Gender { get; set; }
        
        [Required, Phone, Sieve(CanFilter = true, CanSort = true)] 
        public string PhoneNumber { get; set; }
        
        [Required, EmailAddress, Sieve(CanFilter = true, CanSort = true)]
        public string Email { get; set; }
        
        [Required, Sieve(CanFilter = true, CanSort = true)]
        public string Department { get; set; }
        [Required, Sieve(CanFilter = true, CanSort = true)]
        public string Faculty { get; set; }
        [Required, Sieve(CanSort = true)]
        public DateTime DateOfBirth { get; set; }
        
        [Required, Sieve(CanFilter = true, CanSort = true)] public string NextOfKin { get; set; }
        [Required, Sieve(CanFilter = true, CanSort = true)] public string NextOfKinPhoneNumber { get; set; }

        public string PhotoUrl { get; set; }
    }

    public class AccountViewModel : EnrollmentAccountViewModel 
    {
        public AccountKind Kind { get; set; }
    }
}
