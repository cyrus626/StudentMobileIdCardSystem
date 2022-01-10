using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.Entities
{
    #region Abstractions
    public enum AccountKind
    {
        Student,
        Admin
    }
    #endregion

    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }

        [Required] public string MatricNumber { get; set; }
        [Required] public string Gender { get; set; }

        public AccountKind Kind { get; set; }

        [Required] public string Department { get; set; }
        [Required] public string Faculty { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string NextOfKin { get; set; }
        [Required] public string NextOfKinPhoneNumber { get; set; }

        #region Constructors
        public ApplicationUser() { }
        public ApplicationUser(EnrollmentApplication app)
        {
            FirstName = app.FirstName;
            MiddleName = app.MiddleName;
            LastName = app.LastName;
            MatricNumber = app.MatricNumber;
            Gender = app.Gender;
            PhoneNumber = app.PhoneNumber;
            Email = app.Email;
            Department = app.Department;
            Faculty = app.Faculty;
            DateOfBirth = app.DateOfBirth;
            NextOfKin = app.NextOfKin;
            NextOfKinPhoneNumber = app.NextOfKinPhoneNumber;

            UserName = MatricNumber;
        }
        #endregion
    }
}
