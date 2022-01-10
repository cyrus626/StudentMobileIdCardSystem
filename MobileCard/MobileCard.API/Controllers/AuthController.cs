using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;

namespace MobileCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Properties
        IMapper Mapper { get; }
        ApplicationContext DataContext { get; }
        UserManager<ApplicationUser> UserManager { get; }
        #endregion

        #region Constructors
        public AuthController(IMapper mapper, ApplicationContext dataContext, UserManager<ApplicationUser> userManager)
        {
            Mapper = mapper;
            DataContext = dataContext;
        }
        #endregion

        #region Methods
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            

            return Ok();
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] AccountEnrollmentViewModel model)
        {
            var application = Mapper.Map<EnrollmentApplication>(model);

            DataContext.EnrollmentApplications.Add(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }
        #endregion

    }
}
