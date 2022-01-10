using System.ComponentModel.DataAnnotations;

namespace MobileCard.API.Models.Contexts
{
    public interface IApiResponseContext
    {
        string Code { get; set; }
        string Description { get; set; }
    }

    public interface IJobError
    {
        string Code { get; }
        string Description { get; }
    }

    public class ApiResponse : IApiResponseContext, IJobError
    {
        #region Properties
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        #endregion

        #region Constructors
        public ApiResponse() { }
        public ApiResponse(string code, string message)
        {
            Code = code;
            Description = message;
        }
        #endregion
    }

    public class ApiResponseContextCollection : List<ApiResponse> { }

    public class SingleApiResponse : List<ApiResponse>
    {
        public SingleApiResponse(string code, string message)
        {
            Add(new ApiResponse(code, message));
        }
    }
}
