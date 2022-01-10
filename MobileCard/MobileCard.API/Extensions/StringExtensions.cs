using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MobileCard.API.Extensions
{
    public static class StringExtensions
    {
        #region Properties
        static ILogger Logger { get; }
        #endregion

        #region Binding
        public static string MapTo<T>(this string body, IDictionary<string, T> context, string markers = "{{}}") where T : class
        {
            int mid = markers.Length / 2;
            string left = markers.Substring(0, mid);
            string right = markers.Substring(mid);

            Regex regex = new Regex($@"{left}([a-zA-Z]+[0-9]*){right}");

            var matches = regex.Matches(body).Cast<Match>()
                .OrderByDescending(i => i.Index);

            foreach (Match match in matches)
            {
                var fullMatch = match.Groups[0];

                var propName = match.Groups[1].Value;

                object value = string.Empty;

                try
                {
                    context.TryGetValue(propName, out T propValue);

                    value = propValue;
                    if (value == null) value = string.Empty;
                }
                catch (Exception ex)
                {
                    Core.Log.Debug($"A binding error occured while binding type of ({typeof(T)}) to {body}\n{ex}");
                    return body;
                }

                string change = value.ToString();
                // remove substring with pattern
                // use remove instead of replace, since 
                // you may have several the same string
                // and insert what required
                if (!string.IsNullOrWhiteSpace(change))
                    body = body.Remove(fullMatch.Index, fullMatch.Length)
                        .Insert(fullMatch.Index, change);
            }

            return body;
        }

        public static string BindTo<T>(this string body, T model, string markers = "{{}}") where T : class
        {
            int mid = markers.Length / 2;
            string left = markers.Substring(0, mid);
            string right = markers.Substring(mid);

            Regex regex = new Regex($@"{left}([a-zA-Z]+[0-9]*){right}");

            var matches = regex.Matches(body).Cast<Match>()
                .OrderByDescending(i => i.Index);

            foreach (Match match in matches)
            {
                var fullMatch = match.Groups[0];

                var propName = match.Groups[1].Value;

                object value = string.Empty;

                try
                {
                    // use reflection to get property
                    // Note: if you need to use fields use GetField
                    var prop = typeof(T).GetProperty(propName);
                    if (prop == null) continue;

                    value = prop.GetValue(model);

                    if (value == null) value = string.Empty;
                }
                catch (Exception ex)
                {
                    Core.Log.Debug($"A binding error occured while binding type of ({typeof(T)}) to {body}\n{ex}");
                    return body;
                }

                string change = value.ToString();
                // remove substring with pattern
                // use remove instead of replace, since 
                // you may have several the same string
                // and insert what required
                if (!string.IsNullOrWhiteSpace(change))
                    body = body.Remove(fullMatch.Index, fullMatch.Length)
                        .Insert(fullMatch.Index, change);
            }

            return body;
        }

        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s ?? ""));
        }
        #endregion

        public static bool ValidateJSON(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Logger.LogError(ex, "An error occured while validating json");
                return false;
            }
        }

        public static bool AnyIsNullOrEmpty(params string[] strings)
        {
            bool empty = false;

            for (int i = 0; i < strings.Length && !empty; i++)
                empty = !string.IsNullOrEmpty(strings[i]);

            return empty;
        }

        public static string Clean(this string source, params string[] strings)
            => strings.Aggregate(new StringBuilder(source), (current, replacement)
                => current.Replace(replacement, "")).ToString();

        public static string ShortGUID() =>
            Convert.ToBase64String(
            Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .TrimEnd('=');

        /// <summary>
        /// Convert a normal string to base64
        /// </summary>
        /// <param name="text">Original String</param>
        /// <returns></returns>
        /// <remarks>
        /// Original Source: https://stackoverflow.com/a/60738564/8058709
        /// </remarks>
        public static string EncodeToBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text))
                .TrimEnd('=').Replace('+', '-')
                .Replace('/', '_');
        }



        /// <summary>
        /// Convert a base64 string to a normal one
        /// </summary>
        /// <param name="payload">Base64 string</param>
        /// <returns>A normal string</returns>
        /// <remarks>
        /// Original Source: https://stackoverflow.com/a/60738564/8058709
        /// </remarks>
        public static string DecodeFromBase64(this string payload)
        {
            payload = payload.Replace('_', '/').Replace('-', '+');
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        }

        /// <summary>
        /// Creates a url friendly base64 string from byte array
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] payload)
            => Convert.ToBase64String(payload)
                .TrimEnd('=').Replace('+', '-')
                .Replace('/', '_');

        /// <summary>
        /// Creates a byte array from a base64 string
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static byte[] FromBase64(this string payload)
        {
            payload = payload.Replace('_', '/').Replace('-', '+');
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }

            return Convert.FromBase64String(payload);
        }
    }
}
