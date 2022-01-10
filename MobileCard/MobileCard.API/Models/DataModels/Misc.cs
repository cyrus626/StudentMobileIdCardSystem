using Sieve.Models;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class DocumentedSieveModel : SieveModel
    {
        /// <summary>
        /// This field is read-only and is strictly for documentational purposes.
        /// 
        /// It signifies that the above endpoint contains queryable fields for sorting, filtering and pagination.
        /// Please visit https://github.com/Biarity/Sieve#send-a-request for guidance on how they can be used.
        /// </summary>
        public string _ { get; }
    }

    public class EntityIdViewModel
    {
        [Required] public string Id { get; set; }

        public EntityIdViewModel(string id)
        {
            Id = id;
        }
    }
}
