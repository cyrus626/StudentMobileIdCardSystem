using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Controllers.Responses;
using MobileCard.API.Extensions;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;

namespace MobileCard.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerErrorResponse(nameof(AuthResponses.AccountNotFound))]
    [SwaggerErrorResponse(nameof(AuthResponses.AccountLockout))]
    public class AccountController : ControllerBase, IUserAwareController
    {
        #region Properties
        IMapper Mapper { get; }

        UserManager<ApplicationUser> UserManager { get; }
        UserManager<ApplicationUser> IUserAwareController.UserManager => UserManager;
        
        ApplicationContext DataContext { get; }
        #endregion

        #region Constructors
        public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationContext dataContext)
        {
            Mapper = mapper;
            UserManager = userManager;
            DataContext = dataContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get user account
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOkResponse(typeof(AccountViewModel), "Full account details")]
        public async Task<IActionResult> GetAccount()
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            DataContext.Attach(user);
            await DataContext.Entry(user).Collection(x => x.Resources).SafelyLoadAsync();
            Resource photo = user.Resources.SingleOrDefault(x => x.Purpose == ResourcePurpose.AccountDp);

            var model = Mapper.Map<AccountViewModel>(user);
            model.PhotoUrl = photo?.ToEndpoint(this.GetRootUrl());
            
            return Ok(model);
        }
        #endregion
    }
}
