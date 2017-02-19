using System.Security.Claims;

namespace Authorization.Services
{
    public interface IUserClaimsService
    {
        Claim[] GetUserClaims(string userName);
    }
}
