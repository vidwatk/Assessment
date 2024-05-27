using Assessment.Core.Dtos;
using Assessment.Core.Entities;
using Assessment.Core.Entities.Interfaces;
using Assessment.Core.RoleManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //Route for seeding roles to db
        [HttpPost]
        [Route("add-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            {
                var seedRoles = await _authService.SeedRolesAsync();

                return Ok(seedRoles);
            }
        }

        //Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var regRes = await _authService.RegisterAsync(registerDto);

            if (regRes.IsSucceeded)
            {
                return Ok(regRes);
            }
            return BadRequest(regRes);
        }

        //Route to login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginRes = await _authService.LoginAsync(loginDto);
            if (loginRes.IsSucceeded)
            {
                return Ok(loginRes);
            }
            return Unauthorized(loginRes);
        }

        //Route for sales to admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var adminRes = await _authService.MakeAdminAsync(updatePermissionDto);
            if (adminRes.IsSucceeded)
            {
                return Ok(adminRes);
            }
            return BadRequest(adminRes);
        }


        //Route for sales to developer
        [HttpPost]
        [Route("make-developer")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var ownerRes = await _authService.MakeDevAsync(updatePermissionDto);
            if (ownerRes.IsSucceeded)
            {
                return Ok(ownerRes);
            }
            return BadRequest(ownerRes);
        }

        [HttpPost]
        [Route("make-sales")]
        public async Task<IActionResult> MakeSales([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var ownerRes = await _authService.MakeSalesAsync(updatePermissionDto);
            if (ownerRes.IsSucceeded)
            {
                return Ok(ownerRes);
            }
            return BadRequest(ownerRes);
        }
    }
}
