using API.Dto;
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
        
        //CRUD

        [HttpPost]
        [Route("AddUser", Name = "AddUserAsync")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDro userDro)
        {
            Debug.WriteLine("1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Debug.WriteLine("2");
                User user = new User
                {
                    Alias = userDro.Alias,
                    Email = userDro.Email,
                    PhoneNr = userDro.PhoneNr,
                    IsLoggedIn = userDro.IsLoggedIn,
                    ProfilePictureUrl = userDro.ProfilePictureUrl,
                };
                Debug.WriteLine("3");
                await _iUserService.InsertAsync(user);
                return Ok();
                //return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
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

        [HttpGet]
        [Route("GetAllUsers", Name = "GetAllUsersAsync")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            IEnumerable<User> users = await _iUserService.GetAllAsync(u => true);
            return Ok(users);
        }

        [HttpPatch] //PATCH always have a body.
        [Route("UpdateUser/{id:Guid}", Name = "UpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDro userDro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User user = new User
                {
                    Alias = userDro.Alias,
                    Email = userDro.Email,
                    PhoneNr = userDro.PhoneNr,
                    IsLoggedIn = userDro.IsLoggedIn,
                    ProfilePictureUrl = userDro.ProfilePictureUrl,
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

        [HttpDelete]
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
