using MobileCard.API.Models.Contexts;

namespace MobileCard.API.Controllers.Responses
{
    public static class ResourceResponses
    {
        public static SingleApiResponse ResourceInvalid { get; }
            = new SingleApiResponse(nameof(ResourceNotFound),
                    "The upload process was not implemented properly. Please contact support.");

        public static SingleApiResponse ResourceNotFound { get; }
            = new SingleApiResponse(nameof(ResourceNotFound),
                "The requested resource could not be located.");

        public static SingleApiResponse ResourceNotSupported { get; }
            = new SingleApiResponse(nameof(ResourceNotSupported),
                "The current resource file is not supported for this purpose");

        public static SingleApiResponse ResourceUploadFailed { get; }
            = new SingleApiResponse(nameof(ResourceUploadFailed),
                "An unexpected error occured during the upload process. Please try again.");
    }
}
