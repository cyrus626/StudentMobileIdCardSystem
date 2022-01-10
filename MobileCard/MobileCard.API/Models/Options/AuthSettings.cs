using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MobileCard.API.Models.Options
{
    public class AuthSettings
    {
        // TODO: This is for demonstrations purposes only. Such a key should not be present in code
        public static AuthSettings Instance { get; } = new AuthSettings
        {
            Key = "U2Nob29sUHJvamVjdFdhc0Z1bg=="
        };

        public const string _KEY = "Authentication";
        public string Key { get; set; } = "";

        public byte[] Bytes => Encoding.ASCII.GetBytes(Key);

        SymmetricSecurityKey symmetricKey = null;
        public SymmetricSecurityKey SymmetricKey
        {
            get
            {
                if (symmetricKey != null)
                    return symmetricKey;
                try
                {
                    symmetricKey = new SymmetricSecurityKey(Bytes);
                }
                catch (Exception ex)
                {
                    Core.Log.Error($"Failed to attain bytes from authentication key.\n{ex}");
                }

                return symmetricKey;
            }
        }
    }
}
