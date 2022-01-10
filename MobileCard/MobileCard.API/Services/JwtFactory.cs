using Microsoft.Extensions.Options;
using MobileCard.API.Models.Entities;
using MobileCard.API.Models.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using NLog;
using ILogger = NLog.ILogger;

namespace MobileCard.API.Services
{
    #region Abstractions
    public interface IJwtFactory
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
    #endregion

    public class JwtFactory : IJwtFactory
    {
        #region Properties

        #region Services
        ApplicationContext DataContext { get; }

        ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
        #endregion

        #region Options

        JwtIssuerOptions Options { get; }
        #endregion

        #endregion

        #region Constructors
        public JwtFactory(ApplicationContext dataContext, IOptions<JwtIssuerOptions> options)
        {
            DataContext = dataContext;
            Options = options.Value;
        }
        #endregion

        #region Methods

        #region IJwtFactory Implementation
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(user.Id.ToString(), "Token"), new[]
            {
                new Claim(Core.JWT_CLAIM_ID, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await Options.JtiGenerator()),
                new Claim(Core.JWT_CLAIM_VERIFIED, true.ToString()) //(user.EmailConfirmed && user.PhoneNumberConfirmed).ToString())
            });

            DataContext.Attach(user);

            switch (user.Kind)
            {
                case AccountKind.Admin:
                    Options.ValidFor = TimeSpan.FromHours(6);
                    break;

                case AccountKind.Student:
                    Options.ValidFor = TimeSpan.FromDays(14);
                    break;
            }

            return new JwtSecurityTokenHandler().CreateEncodedJwt(
                Options.Issuer, Options.Audience, identity,
                Options.NotBefore, Options.Expiration, Options.IssuedAt,
                Options.SigningCredentials);
        }
        #endregion

        #endregion
    }
}
