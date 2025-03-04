using BusinessLogic.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GetName(string jwtToken)
        {
            return GetClaimValue(jwtToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        }

        public string GetEmail(string jwtToken)
        {
            return GetClaimValue(jwtToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        }

        public string GetRole(string jwtToken)
        {
            return GetClaimValue(jwtToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        }

        public string GetId(string jwtToken)
        {
            return GetClaimValue(jwtToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        private string GetClaimValue(string jwtToken, string claimType)
        {
            if (string.IsNullOrEmpty(jwtToken)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            return jsonToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
