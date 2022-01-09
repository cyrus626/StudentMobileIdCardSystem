using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Models.DataModels;

namespace MobileCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Properties

        #endregion

        #region Methods
        public Task<IActionResult> Login([FromBody]LoginViewModel model)
        {

        }

        [HttpPost("enroll")]
        public Task<IActionResult> Enroll([FromBody] AccountEnrollmentViewModel model)
        {

        }
        #endregion

    }
}
