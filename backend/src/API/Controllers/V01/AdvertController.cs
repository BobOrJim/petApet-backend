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
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _iAdvertService;
        public AdvertController(IAdvertService iAdvertService)
        {
            _iAdvertService = iAdvertService;
        }
        
        //CRUD
        [HttpPost] //Funkar
        [Route("AddAdvert", Name = "AddAdvertAsync")]
        public async Task<IActionResult> AddAdvertAsync([FromBody] AdvertDro advertDro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Advert advert = new Advert
                {
                    UserId = Guid.Parse(advertDro.UserId),
                    Name = advertDro.Name,
                    Age = advertDro.Age,
                    Race = advertDro.Race,
                    Sex = advertDro.Sex,
                    Personallity = advertDro.Personallity,
                    RentPeriod = advertDro.Grade,
                    Review = advertDro.Review,
                    ImageUrls = advertDro.ImageUrls
                };
                await _iAdvertService.InsertAsync(advert);
                return Ok();
                //return CreatedAtRoute("GetAdvertById", new { id = advert.Id }, advert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet] //Funkar
        [Route("GetAdvertById/{id:Guid}", Name = "GetAdvertByIdAsync")]
        public async Task<IActionResult> GetAdvertByIdAsync(Guid id)
        {
            Console.WriteLine("GetAdvertById");
            Debug.WriteLine("GetAdvertById");
            Advert? Advert = await _iAdvertService.GetByIdAsync(id);
            if (Advert == null)
            {
                return NotFound();
            }
            return Ok(Advert);
        }

        [HttpGet] //Funkar
        [Route("GetAllAdverts", Name = "GetAllAdvertsAsync")]
        public async Task<IActionResult> GetAllAdvertsAsync()
        {
            IEnumerable<Advert> adverts = await _iAdvertService.GetAllAsync(u => true);
            return Ok(adverts);
        }

        [HttpPatch] //PATCH always have a body.
        [Route("UpdateAdvertById/{id:Guid}", Name = "UpdateAdvertByIdAsync")]
        public async Task<IActionResult> UpdateAdvertByIdAsync(Guid id, [FromBody] AdvertDro advertDro)
        {
            Debug.WriteLine("1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Debug.WriteLine("2");

                if (await _iAdvertService.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                Advert advert = new Advert
                {
                    UserId = Guid.Parse(advertDro.UserId),
                    Name = advertDro.Name,
                    Age = advertDro.Age,
                    Race = advertDro.Race,
                    Sex = advertDro.Sex,
                    Personallity = advertDro.Personallity,
                    RentPeriod = advertDro.RentPeriod,
                    Grade = advertDro.Grade,
                    Review = advertDro.Review,
                    ImageUrls = advertDro.ImageUrls
                };
                Debug.WriteLine("3");

                await _iAdvertService.UpdateAsync(advert);
                return Ok(advert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpDelete] //Funkar
        [Route("DeleteAdvertById/{id:Guid}", Name = "DeleteAdvertByIdAsync")]
        public async Task<IActionResult> DeleteAdvertByIdAsync(Guid id)
        {
            Console.WriteLine("DeleteAdvert");
            Debug.WriteLine("DeleteAdvert");
            try
            {
                var advert = await _iAdvertService.GetByIdAsync(id);
                if (advert == null)
                {
                    return NotFound();
                }
                await _iAdvertService.DeleteAsync(advert);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
