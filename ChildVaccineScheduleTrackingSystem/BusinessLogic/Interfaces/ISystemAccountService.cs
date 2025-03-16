using System.Threading.Tasks;
using BusinessLogic.DTOs.SystemAccountDTOs;

namespace BusinessLogic.Interfaces
{
    public interface ISystemAccountService
    {
        Task<string> Login(LoginDTO loginDto);
        Task<bool> Register(RegisterDTO registerDto);

    }
}
