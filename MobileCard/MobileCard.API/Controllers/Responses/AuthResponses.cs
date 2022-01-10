using MobileCard.API.Models.Contexts;

namespace MobileCard.API.Controllers.Responses
{
    public static class AuthResponses
    {
        public static SingleApiResponse AccountLockout { get; }
            = new SingleApiResponse(nameof(AccountLockout), "The provided account has been locked out.");

        public static SingleApiResponse AccountBan { get; }
            = new SingleApiResponse(nameof(AccountBan), "The provided account has been banned.");

        public static SingleApiResponse AccountNotFound { get; }
            = new SingleApiResponse(nameof(AccountNotFound), "The provided user could not be found on the platform");
    }
}
