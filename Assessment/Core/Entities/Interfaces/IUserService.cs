using Assessment.Core.Dtos;

namespace Assessment.Core.Entities.Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<List<string>> GetAllRoleNamesAsync();
        Task<List<object>> GetUsersByUserNameAsync(string username);
        Task<object> GetSalesDataAsync();
        Task<object> GetDevsDataAsync();
        Task<object> GetAdminDataAsync();

    }
}
