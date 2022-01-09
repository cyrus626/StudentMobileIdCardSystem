using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCard.API.Models.Entities
{
    public enum ResourcePurpose
    {
        Temporary,
        AccountDp,
    }

    [Table("Resources")]
    public class Resource : MetaEntity<Resource>
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public ResourcePurpose Purpose { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsLocal { get; set; } = true;

        /// <summary>
        /// Total resource length in bytes
        /// </summary>
        public long Size { get; set; }

        #region Constructors
        public Resource() { }
        public Resource(string name, ResourcePurpose purpose)
        {
            Name = name;
            Purpose = purpose;
        }

        public Resource(IFormFile file, ResourcePurpose purpose)
        {
            Name = file.FileName;
            Purpose = purpose;
        }

        public Resource(Uri uri, ResourcePurpose purpose)
        {
            Path = uri.OriginalString;
            Name = System.IO.Path.GetFileName(Path);
            Purpose = purpose;
            IsLocal = false;
        }
        #endregion
    }
}
