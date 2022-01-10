using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Extensions;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;

namespace MobileCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase, IUserAwareController
    {
        #region Properties
        IMapper Mapper { get; }

        UserManager<ApplicationUser> UserManager { get; }
        UserManager<ApplicationUser> IUserAwareController.UserManager => UserManager;
        #endregion

        #region Constructors
        public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            Mapper = mapper;
            UserManager = userManager;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            return Ok(Mapper.Map<AccountViewModel>(user));
        }
        #endregion
    }
}
