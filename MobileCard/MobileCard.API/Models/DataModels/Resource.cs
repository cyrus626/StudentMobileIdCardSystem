using MobileCard.API.Extensions.DataTypes;
using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.DataModels
{
    public class BasicResourceViewModel
    {
        public string Id { get; set; }   
        public long Size { get; set; }
    }
}
