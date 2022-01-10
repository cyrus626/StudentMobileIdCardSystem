using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileCard.API.Controllers.Responses;
using MobileCard.API.Extensions;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;
using Sieve.Services;

namespace MobileCard.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/manage")]
    public class ManagementController : ControllerBase, IUserAwareController
    {
        #region Properties
        ISieveProcessor Sieve { get; }
        ApplicationContext DataContext { get; }
        IMapper Mapper { get; }
        UserManager<ApplicationUser> UserManager { get; }

        UserManager<ApplicationUser> IUserAwareController.UserManager => UserManager;
        #endregion

        #region Constructors
        public ManagementController(ISieveProcessor sieve, ApplicationContext dataContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            Sieve = sieve;
            DataContext = dataContext;
            Mapper = mapper;
            UserManager = userManager;
        }
        #endregion

        #region Methods
        [HttpGet("students/all")]
        public async Task<IActionResult> GetStudents([FromQuery]DocumentedSieveModel model)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var users = DataContext.Users.AsNoTracking()
                .Where(x => x.Kind == AccountKind.Student)
                .ProjectTo<AccountViewModel>(Mapper.ConfigurationProvider);

            users = Sieve.Apply(model, users);

            return Ok(users);
        }

        [HttpGet("enrollment/all")]
        public async Task<IActionResult> GetEnrollmentApplications([FromQuery]DocumentedSieveModel model)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var applications = DataContext
                .EnrollmentApplications
                .AsNoTracking()
                .ProjectTo<AccountViewModel>(Mapper.ConfigurationProvider);

            applications = Sieve.Apply(model, applications);

            return Ok(applications);
        }

        [HttpPost("enrollment/{enrollmentId}/approve")]
        public async Task<IActionResult> ApproveApplication([FromRoute]string enrollmentId)
        {
            var res = await this.GetCurrentUser(out ApplicationUser current);
            if (res != null) return res;

            if (current.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var application = await DataContext.EnrollmentApplications.SingleOrDefaultAsync(x => x.Id == enrollmentId);
            
            if (application == null) return NotFound();

            ApplicationUser user = new ApplicationUser(application);
            await UserManager.CreateAsync(user, user.FirstName.ToLower() + user.LastName.ToLower());
            await DataContext.SaveChangesAsync();

            DataContext.EnrollmentApplications.Remove(application);
            await DataContext.SaveChangesAsync();

            // TODO: Create user account
            return Ok();
        }

        [HttpPost("enrollment/{enrollmentId}/reject")]
        public async Task<IActionResult> RejectApplication([FromRoute]string enrollmentId)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var application = await DataContext.EnrollmentApplications.SingleOrDefaultAsync(x => x.Id == enrollmentId);

            if (application == null) return Ok();

            DataContext.EnrollmentApplications.Remove(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
