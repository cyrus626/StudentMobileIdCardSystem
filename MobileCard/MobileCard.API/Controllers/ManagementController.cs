using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;
using MobileCard.API.Services;
using Sieve.Services;

namespace MobileCard.API.Controllers
{
    [ApiController]
    [Route("api/manage")]
    public class ManagementController : ControllerBase
    {
        #region Properties
        ISieveProcessor Sieve { get; }
        ApplicationContext DataContext { get; }
        IMapper Mapper { get; }
        UserManager<ApplicationUser> UserManager { get; }
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
        [HttpGet("enrollment/all")]
        public IActionResult GetEnrollmentApplications([FromQuery]DocumentedSieveModel model)
        {
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
            var application = await DataContext.EnrollmentApplications.SingleOrDefaultAsync(x => x.Id == enrollmentId);
            
            if (application == null)
                return NotFound();

            // TODO: Create user account
            return Ok();
        }

        [HttpPost("enrollment/{enrollmentId}/reject")]
        public async Task<IActionResult> RejectApplication([FromRoute]string enrollmentId)
        {
            var application = await DataContext.EnrollmentApplications.SingleOrDefaultAsync(x => x.Id == enrollmentId);

            if (application == null) return Ok();

            DataContext.EnrollmentApplications.Remove(application);
            await DataContext.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
