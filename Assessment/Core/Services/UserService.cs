using Assessment.Core.DbContext;
using Assessment.Core.Dtos;
using Assessment.Core.Entities;
using Assessment.Core.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            return await _roleManager.Roles.Select(role => role.Name).ToListAsync();
        }


        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<object>> GetUsersByUserNameAsync(string username)
        {
            var userProfiles = await _userManager.Users
            .Where(u => u.UserName == username)
            .ToListAsync();

            return userProfiles.Select(up => new
            {
                LastName = up.LastName,
                FirstName = up.FirstName
            }).ToList<object>();
        }

        public async Task<object> GetSalesDataAsync()
        {
            var manuals = await _context.Manuals.ToListAsync();
            var drawings = await _context.Drawings.ToListAsync();
            var reports = await _context.Reports.ToListAsync();

            return new
            {
                Manuals = manuals,
                Drawings = drawings,
                Reports = reports
            };
        }

        public async Task<object> GetDevsDataAsync()
        {
            var manuals = await _context.Manuals.ToListAsync();
            var pictures = await _context.Pictures.ToListAsync();

            return new
            {
                Manuals = manuals,
                Pictures = pictures
            };
        }

        public async Task<object> GetAdminDataAsync()
        {
            var manuals = await _context.Manuals.ToListAsync();
            var pictures = await _context.Pictures.ToListAsync();
            var drawings = await _context.Drawings.ToListAsync();
            var reports = await _context.Reports.ToListAsync();

            return new
            {
                Manuals = manuals,
                Pictures = pictures,
                Drawings = drawings,
                Reports = reports
            };
        }
    }
}
