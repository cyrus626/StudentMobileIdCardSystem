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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string MatricNumber { get; set; }
        public string Gender { get; set; }

        public AccountKind Kind { get; set; }

        public string Department { get; set; }
        public string Faculty { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }

        public List<Resource> Resources { get; } = new List<Resource>();

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
