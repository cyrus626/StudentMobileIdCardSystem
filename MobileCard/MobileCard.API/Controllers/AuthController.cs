using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Controllers.Responses;
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
        IJwtFactory JwtFactory { get; }
        #endregion

        #region Constructors
        public AuthController(IMapper mapper, ApplicationContext dataContext, UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory)
        {
            Mapper = mapper;
            DataContext = dataContext;
            JwtFactory = jwtFactory;
        }
        #endregion

        #region Methods
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(model.Username);

            if (user == null) return NotFound(AuthResponses.AccountNotFound);
            bool success = await UserManager.CheckPasswordAsync(user, model.Password);
            
            if (!success) return BadRequest(AuthResponses.IncorrectPassword);
            if (user.LockoutEnabled) return BadRequest(AuthResponses.AccountLockout);

            
            return Ok(new AccessTokenViewModel
            {
                AccessToken = await JwtFactory.GenerateToken(user)
            });
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
