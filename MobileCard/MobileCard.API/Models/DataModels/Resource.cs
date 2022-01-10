using MobileCard.API.Extensions.DataTypes;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class BasicResourceViewModel
    {
        [Required] public string Id { get; set; } = "";

        public BasicResourceViewModel(string id)
        {
            if (ShortGuid.TryParse(id, out ShortGuid sguid))
                Id = sguid;
            else if (Guid.TryParse(id, out Guid guid))
                Id = new ShortGuid(guid);

            if (string.IsNullOrWhiteSpace(Id))
                throw new FormatException("The provided format for the short guid is invalid");
        }
    }
}
