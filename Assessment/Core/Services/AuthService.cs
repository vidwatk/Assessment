using Assessment.Core.Dtos;
using Assessment.Core.Entities;
using Assessment.Core.Entities.Interfaces;
using Assessment.Core.RoleManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assessment.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "Invalid Creds"
                };
            }

            var isPaswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPaswordCorrect)
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "Invalid Creds"
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);
            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = token
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "Invalid Creds"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToRemove = currentRoles.Where(role => role != StaticUserRoles.ADMIN).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN))
            {
                await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);
            }

            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = "User is now exclusively an admin"
            };
        }

        public async Task<AuthServiceResponseDto> MakeDevAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "Invalid Username"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Where(role => role != StaticUserRoles.DEVS).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            
            if (!await _userManager.IsInRoleAsync(user, StaticUserRoles.DEVS))
            {
                await _userManager.AddToRoleAsync(user, StaticUserRoles.DEVS);
            }

            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = "User is now exclusively a dev"
            };
        }

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExists = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExists != null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "User Already exists"
                };
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User creation failed due to: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = errorString
                };
            }

            //Add a default user role to add users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.SALES);

            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.SALES);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.DEVS);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = true,
                    Message = "Role Seeding already done"
                };
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.SALES));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.DEVS));

            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = "Role Seeding already done"
            };
        }

        public async Task<AuthServiceResponseDto> MakeSalesAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceeded = false,
                    Message = "Invalid Username"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Where(role => role != StaticUserRoles.SALES).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);


            if (!await _userManager.IsInRoleAsync(user, StaticUserRoles.SALES))
            {
                await _userManager.AddToRoleAsync(user, StaticUserRoles.SALES);
            }

            return new AuthServiceResponseDto()
            {
                IsSucceeded = true,
                Message = "User is now exclusively a Sales guy"
            };
        }
    }
}
