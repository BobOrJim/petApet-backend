using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;

namespace API.Controllers.V01
{
    [Route("api/V01/[controller]")]
    //[Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;
        public UserController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }
        
        [HttpPost] //Return Ok()
        [Route("AddUser", Name = "AddUserAsync")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User user = new User
                {
                    Alias = userDto.Alias,
                    PhoneNr = userDto.PhoneNr,
                    IsLoggedIn = userDto.IsLoggedIn,
                    ProfilePictureUrl = userDto.ProfilePictureUrl,
                    AuthId = Guid.Parse(userDto.AuthId),
                };
                await _iUserService.InsertAsync(user);
                return Ok();
                //return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet] //Return User
        [Route("GetUserById/{id:Guid}", Name = "GetUserByIdAsync")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            User? user = await _iUserService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet] //Return User[]
        [Route("GetAllUsers", Name = "GetAllUsersAsync")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            IEnumerable<User> users = await _iUserService.GetAllAsync(u => true);
            return Ok(users);
        }

        [HttpPatch] //Return User
        [Route("UpdateUser/{id:Guid}", Name = "UpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User user = new User
                {
                    Id = id,
                    Alias = userDto.Alias,
                    PhoneNr = userDto.PhoneNr,
                    IsLoggedIn = userDto.IsLoggedIn,
                    ProfilePictureUrl = userDto.ProfilePictureUrl,
                    AuthId = Guid.Parse(userDto.AuthId),
                };
                if (await _iUserService.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                await _iUserService.UpdateAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete] //Return Ok()
        [Route("DeleteUserById/{id:Guid}", Name = "DeleteUserByIdAsync")]
        public async Task<IActionResult> DeleteUserByIdAsync(Guid id)
        {
            Debug.WriteLine("1");
            try
            {
                Debug.WriteLine("2");
                var user = await _iUserService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                Debug.WriteLine("3");
                await _iUserService.DeleteAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
