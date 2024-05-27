using Assessment.Core.DbContext;
using Assessment.Core.Entities;
using Assessment.Core.Entities.Interfaces;
using Assessment.Core.RoleManagement;
using Assessment.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ApplicationDbContext context)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            {
                try
                {
                    var users = await _userService.GetAllUsersAsync();
                    return Ok(users);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while fetching users.");
                }
            }
        }

        [HttpGet]
        [Route("getRoles")]
        public async Task<IActionResult> GetAllRoleNames()
        {
            try
            {
                var roleNames = await _userService.GetAllRoleNamesAsync();
                return Ok(roleNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Couldnmt fetch the role names.");
            }
        }

        //Get users by username
        [HttpGet("getUsersByUsername/{username}")]
        public async Task<IActionResult> GetUsersByFirstName(string username)
        {
            var usersWithDetails = await _userService.GetUsersByUserNameAsync(username);
            return usersWithDetails.Any() ? Ok(usersWithDetails) : NotFound();
        }

        [HttpGet("allDataForSales")]
        [Authorize(Roles = StaticUserRoles.SALES)]
        public async Task<IActionResult> GetSalesData()
        {
            if (!User.IsInRole(StaticUserRoles.SALES))
            {
                return Forbid();
            }

            try
            {
                var salesData = await _userService.GetSalesDataAsync();
                return Ok(salesData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching sales data.");
            }
        }

        [HttpGet("allDataForDevs")]
        [Authorize(Roles = StaticUserRoles.DEVS)]
        public async Task<IActionResult> GetDevsData()
        {
            if (!User.IsInRole(StaticUserRoles.DEVS))
            {
                return Forbid();
            }

            try
            {
                var devsData = await _userService.GetDevsDataAsync();
                return Ok(devsData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Couldnt fetch dev data.");
            }
        }

        [HttpGet("allDataForAdmin")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> GetAdminData()
        {
            if (!User.IsInRole(StaticUserRoles.ADMIN))
            {
                return Forbid();
            }

            try
            {
                var adminData = await _userService.GetAdminDataAsync();
                return Ok(adminData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Couldmnt fetch admin data");
            }
        }
    }
}
