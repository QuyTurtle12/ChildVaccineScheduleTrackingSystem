using BusinessLogic.Interfaces;
using Data.Constants;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetCurrentUserId()
        {
            var user = _contextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized User!");
            }

            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "id");

            if (idClaim == null || string.IsNullOrEmpty(idClaim.Value))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "User ID claim not found or is empty.");
            }

            return idClaim.Value;
        }
    }
}
