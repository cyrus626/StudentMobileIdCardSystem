using Microsoft.AspNetCore.StaticFiles;
using MobileCard.API.Models.Entities;

namespace MobileCard.API.Extensions
{
    #region Extensions

    public static class ResourceExtensions
    {
        static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
        public static readonly string[] SupportedPhotos = { "jpg", "bmp", "jpeg", "png" };

        public static async Task DownloadAsync(this Resource resource, IFormFile file)
        {
            // TODO: Make sure resource has an extension
            string ext = Path.GetExtension(file.FileName);

            Func<Resource, int, string> getPath = (resource, n) =>
            {
                if (n == 0)
                    return resource.ToStorage().MapTo(resource.GetMeta()) + ext;
                return resource.ToStorage().MapTo(resource.GetMeta()) + $"[{n}]" + ext;
            };

            int index = 0;
            string path = "";
            while (string.IsNullOrWhiteSpace(path) || File.Exists(path))
                path = getPath(resource, index++);

            resource.Path = path;
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                try { Directory.CreateDirectory(directory); }
                catch (Exception ex)
                {
                    Logger.Error(ex, "An error occured while creating a resource directory");
                };

            await Executor.CautiouslyExecuteAsync(async () =>
            {
                using (Stream stream = File.Create(resource.Path, 1024, FileOptions.Asynchronous))
                {
                    await file.CopyToAsync(stream);
                    resource.Size = stream.Length;
                }
            }, "An error occured while creating a file", TimeSpan.FromSeconds(2), 5);
        }

        public static Task<bool> MoveAsync(Resource source, Resource destination)
        {
            IOExtensions.CreateFileDirectory(destination.Path);

            if (IOExtensions.TryCopy(source.Path, destination.Path, out _, true, true))
                return Task.FromResult(false);

            IOExtensions.TryDelete(source.Path, out _, true);

            return Task.FromResult(true);
        }

        public static Task DeleteAsync(this Resource resource)
        {
            if (File.Exists(resource.Path))
                File.Delete(resource.Path);

            return Task.CompletedTask;
        }

        public static bool IsSupportedPhoto(this Resource resource)
        {
            string ext = Path.GetExtension(resource.Path);
            return SupportedPhotos.Contains(ext.Trim('.', ' '));
        }

        public static bool IsSupportedPhoto(string fileName)
        {
            string ext = Path.GetExtension(fileName ?? "");
            return SupportedPhotos.Contains(ext.Trim('.', ' '));
        }

        public static string ToStorage(this Resource resource)
        {
            switch (resource.Purpose)
            {
                case ResourcePurpose.AccountDp:
                    return ResourceTemplates.Storage.ACCOUNT_DP_ROOT;

                case ResourcePurpose.Temporary:
                    return ResourceTemplates.Storage.TEMPORARY_ROOT;
            }

            return "";
        }

        public static string ToEndpoint(this Resource resource)
        {
            if (!resource.IsLocal) return resource.Path;

            string endpoint = ResourceTemplates.Endpoints.BASE
                .Replace("{{Id}}", new ShortGuid(Guid.Parse(resource.Id)));

            endpoint = Core.BASE_URL + endpoint;//?.MapTo(resource.GetMeta());
            return endpoint;
        }

        public static string GetContentType(this Resource resource)
        {
            bool success = new FileExtensionContentTypeProvider()
                .TryGetContentType(resource.Path, out string contentType);

            if (!success)
                return "application/octet-stream";

            return contentType;
        }
    }

    #endregion

    #region Templates
    public static class ResourceTemplates
    {
        public static class Keys
        {
            public const string AssetId = nameof(AssetId);
            public const string AccountId = nameof(AccountId);
            public const string Username = nameof(Username);
            public const string AssetVersion = nameof(AssetVersion);

            public const string GroupId = nameof(GroupId);
        }

        public static class Storage
        {
            public static string ROOT { get; } = Path.Combine(
                Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData),
                "MobileCard", "Resources");

            // TODO: Make usernames folder friendly
            public static string ACCOUNT_DP_ROOT { get; } = Path.Combine(ROOT, "accounts",
                $"{{{{{Keys.Username}}}}}", "display-picture");

            public static string TEMPORARY_ROOT { get; } = Path.Combine(ROOT, "temp", $"{{{{{Keys.AssetId}}}}}");
        }

        public static class Endpoints
        {
            public const string BASE = "/api/res/{{Id}}";
        }
    }
    #endregion
}
