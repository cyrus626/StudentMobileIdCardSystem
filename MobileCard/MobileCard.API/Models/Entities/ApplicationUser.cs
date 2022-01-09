using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.Entities
{
    #region Abstractions
    public enum ApplicationUserKind
    {
        Student,
        Admin
    }
    #endregion

    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }

        [Required] public string MatricNumber { get; set; }
        [Required] public string Gender { get; set; }

        public ApplicationUserKind Kind { get; set; }
    }
}
