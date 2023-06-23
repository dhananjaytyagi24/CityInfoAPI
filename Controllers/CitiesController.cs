using System;
using AutoMapper;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers
{
    [ApiController]
    [Route("/api/cities")]
    // [Route("/api/[controller]")]
    // controller name replaces controller in above statement
    public class CitiesController : ControllerBase
	{
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities([FromQuery]string? filter, [FromQuery] string? search)
        {
            var cities = await _cityInfoRepository.GetCitiesAsync(filter, search);
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);

            if(city == null)
            {
                return BadRequest("Incorrect city");
            }

            if (includePointOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}

