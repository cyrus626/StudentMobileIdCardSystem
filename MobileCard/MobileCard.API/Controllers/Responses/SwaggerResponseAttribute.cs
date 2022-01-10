using MobileCard.API.Models.Contexts;
using NSwag.Annotations;
using System.Net;

namespace MobileCard.API.Controllers.Responses
{
    public class SwaggerOkResponseAttribute : SwaggerResponseAttribute
    {
        public SwaggerOkResponseAttribute(Type type) : base(HttpStatusCode.OK, type) { }

        public SwaggerOkResponseAttribute() : base(HttpStatusCode.OK, null)
        {
            Description = "Operation was successful";
        }

        public SwaggerOkResponseAttribute(string description) : this()
        {
            Description = description;
        }

        public SwaggerOkResponseAttribute(Type type, string description) : this(type)
        {
            Description = description;
        }
    }

    public class SwaggerUnauthorizedResponseAttribute : SwaggerResponseAttribute
    {
        public SwaggerUnauthorizedResponseAttribute(string description)
            : base(HttpStatusCode.Unauthorized, typeof(ApiResponseContextCollection))
        {
            Description = description;
        }

        public SwaggerUnauthorizedResponseAttribute() : this("") { }
    }

    public class SwaggerNotFoundResponseAttribute : SwaggerResponseAttribute
    {
        public SwaggerNotFoundResponseAttribute(string description)
            : base(HttpStatusCode.NotFound, typeof(ApiResponseContextCollection))
        {
            Description = description;
        }

        public SwaggerNotFoundResponseAttribute() : this("") { }
    }

    public class SwaggerErrorResponseAttribute : SwaggerResponseAttribute
    {
        public SwaggerErrorResponseAttribute(string description)
            : base(HttpStatusCode.BadRequest, typeof(ApiResponseContextCollection))
        {
            Description = description;
        }

        public SwaggerErrorResponseAttribute() : this("") { }
    }
}
