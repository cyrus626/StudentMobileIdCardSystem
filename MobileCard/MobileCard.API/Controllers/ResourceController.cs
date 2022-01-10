using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using MobileCard.API.Controllers.Responses;
using MobileCard.API.Extensions;
using MobileCard.API.Extensions.DataTypes;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;
using IOFile = System.IO.File;

namespace MobileCard.API.Controllers
{
    [ApiController]
    [Route("api/res")]
    public class ResourceController : ControllerBase
    {
        #region Properties
        ApplicationContext DataContext { get; }

        IMapper Mapper { get; }
        #endregion

        #region Constructors
        public ResourceController(ApplicationContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpPost("upload/enrollment/photo")]
        [SwaggerOkResponse(typeof(BasicResourceViewModel), "Basic details about the new resource")]
        public async Task<IActionResult> UploadEnrollmentPhoto([FromForm]IFormFile file)
        {
            // TODO: Automatically delete resource files
            Resource res = new Resource(file, ResourcePurpose.Temporary);
            await ResourceExtensions.DownloadAsync(res, file);

            DataContext.Resources.Add(res);
            await DataContext.SaveChangesAsync();

            
            return Ok(Mapper.Map<BasicResourceViewModel>(res));
        }

        [HttpGet("{resourceId}")]
        public async Task<IActionResult> GetResource([FromRoute]string resourceId)
        {
            Resource res = null;

            if (Guid.TryParse(resourceId, out Guid id))
            {
                string replica = id.ToString();

                res = await DataContext.Resources
                    .SingleOrDefaultAsync(x => x.Id == replica);
            }
            else if (ShortGuid.TryParse(resourceId, out Guid shortId))
            {
                string replica = shortId.ToString();

                res = await DataContext.Resources
                    .SingleOrDefaultAsync(x => x.Id == replica);
            }


            if (res?.IsLocal ?? false) return Redirect(res.Path);
            if (res == null || !IOFile.Exists(res.Path))
                return BadRequest(ResourceResponses.ResourceNotFound);
            

            _ = new FileExtensionContentTypeProvider()
                .TryGetContentType(res.Path, out string contentType);

            return PhysicalFile(res.Path, contentType);
        }

        
        #endregion
    }
}
