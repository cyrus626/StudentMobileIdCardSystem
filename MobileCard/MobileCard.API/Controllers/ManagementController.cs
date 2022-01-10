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
using IOPath = System.IO.Path;

namespace MobileCard.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/manage")]
    [SwaggerErrorResponse(nameof(AuthResponses.AccountNotFound))]
    [SwaggerErrorResponse(nameof(AuthResponses.AccountLockout))]
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
        /// <summary>
        /// Get list of registered students
        /// </summary>
        [HttpGet("students/all")]
        [SwaggerOkResponse(typeof(IEnumerable<AccountViewModel>), "List of student accounts based on query")]
        public async Task<IActionResult> GetStudents([FromQuery]DocumentedSieveModel model)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var users = await DataContext.Users.AsNoTracking()
                .Include(x => x.Resources)
                .Where(x => x.Kind == AccountKind.Student)
                .ToListAsync();

            string rootUrl = this.GetRootUrl();
            List<AccountViewModel> models = new List<AccountViewModel>();

            foreach (var instance in users)
            {
                var acc = Mapper.Map<AccountViewModel>(instance);
                var photo = instance.Resources.SingleOrDefault(x => x.Purpose == ResourcePurpose.AccountDp);

                acc.PhotoUrl = photo?.ToEndpoint(rootUrl);

                models.Add(acc);
            }


            return Ok(Sieve.Apply(model, models.AsQueryable()));
        }

        /// <summary>
        /// Get list of enrollment applications
        /// </summary>
        [HttpGet("enrollment/all")]
        [SwaggerOkResponse(typeof(IEnumerable<EnrollmentAccountViewModel>), "List of enrollment applications based on query")]
        public async Task<IActionResult> GetEnrollmentApplications([FromQuery]DocumentedSieveModel model)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var applications = await DataContext
                .EnrollmentApplications
                .Include(x => x.Resources)
                .AsNoTracking()
                .ToListAsync();

            string rootUrl = this.GetRootUrl();
            List<EnrollmentAccountViewModel> models = new List<EnrollmentAccountViewModel>();

            foreach (var instance in applications)
            {
                var acc = Mapper.Map<EnrollmentAccountViewModel>(instance);
                var photo = instance.Resources.SingleOrDefault(x => x.Purpose == ResourcePurpose.AccountDp);

                acc.PhotoUrl = photo?.ToEndpoint(rootUrl);

                models.Add(acc);
            }

            return Ok(Sieve.Apply(model, models.AsQueryable()));
        }

        /// <summary>
        /// Approve an enrollment application
        /// </summary>
        [HttpPost("enrollment/{enrollmentId}/approve")]
        [SwaggerOkResponse]
        public async Task<IActionResult> ApproveApplication([FromRoute]string enrollmentId)
        {
            var res = await this.GetCurrentUser(out ApplicationUser current);
            if (res != null) return res;

            if (current.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var application = await DataContext.EnrollmentApplications
                .Include(x => x.Resources)
                .SingleOrDefaultAsync(x => x.Id == enrollmentId);
            
            if (application == null) return NotFound();

            ApplicationUser user = new ApplicationUser(application);
            await UserManager.CreateAsync(user, user.FirstName.ToLower() + user.LastName.ToLower());
            await DataContext.SaveChangesAsync();

            foreach (var oldRes in application.Resources.ToList())
            {
                Resource resource = new Resource()
                {
                    Name = oldRes.Name,
                    Purpose = ResourcePurpose.AccountDp
                };

                resource.SetMeta(meta =>
                {
                    meta[ResourceTemplates.Keys.Username] = user.UserName;
                });

                string ext = IOPath.GetExtension(oldRes.Path);
                resource.Path = resource.ToStorage().MapTo(resource.GetMeta()) + ext;

                if (resource.Path != oldRes.Path)
                    await ResourceExtensions.MoveAsync(oldRes, resource);

                DataContext.Resources.Remove(oldRes);
                await DataContext.SaveChangesAsync();

                user.Resources.Add(resource);
                DataContext.Users.Update(user);
                await DataContext.SaveChangesAsync();
            }

            
            DataContext.EnrollmentApplications.Remove(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Reject an enrollment application
        /// </summary>
        [HttpPost("enrollment/{enrollmentId}/reject")]
        [SwaggerOkResponse]
        public async Task<IActionResult> RejectApplication([FromRoute]string enrollmentId)
        {
            var res = await this.GetCurrentUser(out ApplicationUser user);
            if (res != null) return res;

            if (user.Kind != AccountKind.Admin)
                return Unauthorized(AuthResponses.UnauthorizedRoleAccess);

            var application = await DataContext.EnrollmentApplications
                .Include(x => x.Resources)
                .SingleOrDefaultAsync(x => x.Id == enrollmentId);

            if (application == null) return Ok();

            foreach (var resource in application.Resources)
                DataContext.Resources.Remove(resource);

            await DataContext.SaveChangesAsync();
            DataContext.EnrollmentApplications.Remove(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
