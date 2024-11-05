using Microsoft.AspNetCore.Identity;
using TaskMasterBackend.Dto.AppUser;
using TaskMasterBackend.Dto.Task;

namespace TaskMasterBackend.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(AppUserDto userDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
    }
}
