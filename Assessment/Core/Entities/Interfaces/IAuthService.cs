using Assessment.Core.Dtos;

namespace Assessment.Core.Entities.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeDevAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeSalesAsync(UpdatePermissionDto updatePermissionDto);


    }
}
