using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Services
{
    public class UserClaimService : IUserClaimsService
    {
        public Claim[] GetUserClaims(string userName)
        {
            var claimsList = new List<Claim>
            {
                new Claim("sub", userName),
                new Claim("name", userName),
            };

            switch (userName)
            {
                case "Tony":
                    claimsList.Add(new Claim("role", "Admin"));
                    break;
                case "Alice":
                    claimsList.Add(new Claim("role", "Sales"));
                    claimsList.Add(new Claim("region", "UK"));
                    claimsList.Add(new Claim("clearance", "Secret"));
                    break;
                case "Bob":
                    claimsList.Add(new Claim("role", "Sales"));
                    claimsList.Add(new Claim("region", "US"));
                    claimsList.Add(new Claim("clearance", "TopSecret"));
                    break;
                case "Julian":
                    claimsList.Add(new Claim("clearance", "WikiLeaks"));
                    break;
            }
            return claimsList.ToArray();
        }
    }
}
