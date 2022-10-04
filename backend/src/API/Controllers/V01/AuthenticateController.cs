using API.Identity;
using API.Identity.Models;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers.V01
{
    [Route("api/V01/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _iUserService;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserService iUserService)
        {
            _userManager = userManager;  //För authUser
            _roleManager = roleManager;
            _configuration = configuration;
            _iUserService = iUserService;  //För user
        }

        [HttpPost] //Return Token
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                User? userInPuppyDb = CheckIfUserExistsInPuppyDb_ThatMatchAuthUser(user);
                string userInPuppyDbId = "";
                if (userInPuppyDb == null)
                {
                    await CreateUserInPuppyDb_ThatMatchAuthUser(user);
                    userInPuppyDb = CheckIfUserExistsInPuppyDb_ThatMatchAuthUser(user);
                    if (userInPuppyDb == null)
                    {
                        throw new Exception("User does not exist in PuppyDb, and backend failed to create one");
                    }
                }
                
                userInPuppyDbId = userInPuppyDb.Id.ToString();
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    //add id to claim
                    new Claim(ClaimTypes.Anonymous, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                //log in user in puppyDb
                User? puppyUser = await _iUserService.GetByIdAsync(Guid.Parse(userInPuppyDbId));
                if (puppyUser != null)
                {
                    puppyUser.IsLoggedIn = true;
                    await _iUserService.UpdateAsync(puppyUser);
                }

                
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    authUserId = user.Id,
                    userId = userInPuppyDbId
                });
            }
            return Unauthorized();
        }

        [HttpPost] //Ok(new Response { Status = "Success", Message = "User created successfully!" });
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpDelete]
        [Route("deleteAuthUserById/{id:Guid}", Name = "DeleteAuthUserByIdAsync")]
        public async Task<IActionResult> DeleteAuthUserByIdAsync(Guid id)
        {
            Debug.WriteLine("deleteAuthUserById");
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(21),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private User? CheckIfUserExistsInPuppyDb_ThatMatchAuthUser(IdentityUser user)
        {
            {
                User? userFromDb = _iUserService.GetAllAsync(x => x.AuthId == Guid.Parse(user.Id)).Result.FirstOrDefault();
                return userFromDb;
            }
        }

        private async Task CreateUserInPuppyDb_ThatMatchAuthUser(IdentityUser user)
        {
            User newUser = new()
            {
                Alias = "",
                PhoneNr = "",
                IsLoggedIn = false,
                ProfilePictureUrl = "",
                AuthId = Guid.Parse(user.Id),
                Adverts = new List<Advert>(),
            };
            await _iUserService.InsertAsync(newUser);
        }
    }
}

