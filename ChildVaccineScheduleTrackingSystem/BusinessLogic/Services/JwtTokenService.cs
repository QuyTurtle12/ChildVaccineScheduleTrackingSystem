using BusinessLogic.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessLogic.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GetName(string jwtToken)
        {
            return GetClaimValue(jwtToken, "unique_name");
        }

        public string GetEmail(string jwtToken)
        {
            return GetClaimValue(jwtToken, "email");
        }

        public string GetRole(string jwtToken)
        {
            return GetClaimValue(jwtToken, "role");
        }

        public string GetId(string jwtToken)
        {
            return GetClaimValue(jwtToken, "nameid");
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
