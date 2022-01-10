using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileCard.API.Controllers.Responses;
using MobileCard.API.Extensions;
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
            UserManager = userManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Login to the platform
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Enroll for an account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] AccountEnrollmentViewModel model)
        {
            var application = Mapper.Map<EnrollmentApplication>(model);

            var oldRes = await DataContext.Resources.SingleOrDefaultAsync(x => x.Id == model.PhotoId && x.Purpose == ResourcePurpose.Temporary);

            if (oldRes == null) return BadRequest(ResourceResponses.ResourceNotFound);

            Resource res = new Resource()
            {
                Name = oldRes.Name,
                Purpose = ResourcePurpose.AccountDp
            };

            res.SetMeta(meta =>
            {
                meta[ResourceTemplates.Keys.Username] = model.MatricNumber;
            });

            await ResourceExtensions.MoveAsync(oldRes, res);
            
            DataContext.Resources.Remove(oldRes);
            await DataContext.SaveChangesAsync();


            DataContext.EnrollmentApplications.Add(application);
            await DataContext.SaveChangesAsync();

            application.Resources.Add(res);
            DataContext.EnrollmentApplications.Update(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }
        #endregion

    }
}
