
using API.Dto;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
        
        [HttpPost] 
        [Route("AddAdvert", Name = "AddAdvertAsync")]
        public async Task<IActionResult> AddAdvertAsync([FromBody] AdvertDto advertDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                 Advert advert = new Advert {
                    UserId = Guid.Parse(advertDto.UserId),
                    Name = advertDto.Name,
                    Age = advertDto.Age,
                    Race = advertDto.Race,
                    Sex = advertDto.Sex,
                    Personallity = advertDto.Personallity,
                    RentPeriod = advertDto.RentPeriod,
                    Grade = advertDto.Grade,
                    Review = advertDto.Review,
                    ImageUrls = advertDto.ImageUrls,
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

        [HttpGet] 
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

        [HttpGet] 
        [Route("GetAllAdverts", Name = "GetAllAdvertsAsync")]
        public async Task<IActionResult> GetAllAdvertsAsync()
        {
            IEnumerable<Advert> adverts = await _iAdvertService.GetAllAsync(u => true);
            return Ok(adverts);
        }

        [HttpPatch] //PATCH always have a body.
        [Route("UpdateAdvert/{id:Guid}", Name = "UpdateAdvertAsync")]
        public async Task<IActionResult> UpdateAdvertAsync(Guid id, [FromBody] AdvertDto advertDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Advert advert = new Advert
                {
                    Id = id,
                    UserId = Guid.Parse(advertDto.UserId),
                    Name = advertDto.Name,
                    Age = advertDto.Age,
                    Race = advertDto.Race,
                    Sex = advertDto.Sex,
                    Personallity = advertDto.Personallity,
                    RentPeriod = advertDto.RentPeriod,
                    Grade = advertDto.Grade,
                    Review = advertDto.Review,
                    ImageUrls = advertDto.ImageUrls,
                };
                if (await _iAdvertService.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                await _iAdvertService.UpdateAsync(advert);
                return Ok(advert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //ToDo:
        //Plocka ut auth.id ur token, och kolla att detta id kan spåras bakåt via user
        //till advert, och så att det som önskas tas bort tillhör user som lagt till.
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
