using MobileCard.API.Models.Contexts;

namespace MobileCard.API.Controllers.Responses
{
    public static class AuthResponses
    {
        public static SingleApiResponse IncorrectPassword { get; }
            = new SingleApiResponse(nameof(IncorrectPassword), "The provided password is incorrect.");

        public static SingleApiResponse AccountLockout { get; }
            = new SingleApiResponse(nameof(AccountLockout), "The provided account has been locked out.");

        public static SingleApiResponse AccountBan { get; }
            = new SingleApiResponse(nameof(AccountBan), "The provided account has been banned.");

        public static SingleApiResponse AccountNotFound { get; }
            = new SingleApiResponse(nameof(AccountNotFound), "The provided user could not be found on the platform");

        public static SingleApiResponse UnauthorizedRoleAccess { get; }
            = new SingleApiResponse(nameof(UnauthorizedRoleAccess),
                "Your account is not authorized to perform this action");
    }
}
