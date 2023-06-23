using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IPointOfInterestRepository pointOfInterestRepository, IMapper mapper)
        {
            _logger = logger ?? throw new Exception("Logger not found on PointsOfInterest");
            _mailService = mailService ?? throw new Exception("Mail Service not found on PointsOfInterest");
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _pointOfInterestRepository = pointOfInterestRepository ?? throw new ArgumentNullException(nameof(pointOfInterestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id: {cityId} not found when accessing points of interest.");
                return NotFound();
            }

            var points = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(points));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterestbyId(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id: {cityId} not found when accessing points of interest.");
                return NotFound();
            }

            var point = await _cityInfoRepository.GetPointOfInterestbyIdAsync(cityId, pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(point));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, CreatePointOfInterestDto createPointOfInterestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(await _cityInfoRepository.CityExistsAsync(cityId)))
            {
                return NotFound();
            }

            var point = await _pointOfInterestRepository.CreatePointOfInterestAsync(cityId, createPointOfInterestDto);
            var pointDto = _mapper.Map<PointOfInterestDto>(point);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = pointDto.Id
                }, pointDto);
        }

        [HttpPut("{pointofinterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointofinterestId, UpdatePointOfInterestDto updatePointOfInterestDto)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = await _cityInfoRepository.GetPointOfInterestbyIdAsync(cityId, pointofinterestId);
            if(point == null)
            {
                return NotFound();
            }

            _mapper.Map(updatePointOfInterestDto, point);

            await _pointOfInterestRepository.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{pointofinterestId}")]
        public async Task<ActionResult> PatchPointOfInterest(int cityId, int pointofinterestId, JsonPatchDocument<UpdatePointOfInterestDto> updatePointOfInterestPatchDoc)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = await _cityInfoRepository.GetPointOfInterestbyIdAsync(cityId, pointofinterestId);
            if (point == null)
            {
                return NotFound();
            }

            // Since patch is on UpdatePointOfInterestDto, can apply the updatePointOfInterestPatchDoc on UpdatePointOfInterestDto
            // and not directly on PointOfInterest entity. So get the entity > map to UpdatePointOfInterestDto
            // and apply updatePointOfInterestPatchDoc on the mapped var
            var pointEntityUpdateDto = _mapper.Map<UpdatePointOfInterestDto>(point);

            updatePointOfInterestPatchDoc.ApplyTo(pointEntityUpdateDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(pointEntityUpdateDto, point);

            await _pointOfInterestRepository.SaveChangesAsync();

            return NoContent();



            /*
            // Pass model state to check if the patch request was valid
            updatePointOfInterest.ApplyTo(updatePoint, ModelState);

            // We have to manually check the model state here
            // cause patch doc is applied here the automated data anotations check on deserialization and automatic return of bad request
            // cause of api controller don't have affect on it
            // Ex, this validates if the user is trying to update an invalid prop that does not exists
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // This because we need to validate the patched model whether it violates properties such as required, maxlength etc
            // The above validates the model state for json patch doc of updatePointDto and not updatePointDto itself
            // Ex, this checks for attributes on the updatePoint
            if (!TryValidateModel(updatePoint))
            {
                return BadRequest(ModelState);
            }

            point.Name = updatePoint.Name;
            point.Description = updatePoint.Description;

            return NoContent();
            */
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = await _cityInfoRepository.GetPointOfInterestbyIdAsync(cityId, pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            await _pointOfInterestRepository.DeletePointOfInterestAsync(cityId, point);
            _mailService.SendEmail($"{point.Name} Deleted", $"{point.Name} is deleted from the city");
            return NoContent();

            /*
            var city = await _cityInfoRepository.GetCityAsync(cityId, true);
            if (city == null)
            {
                return BadRequest();
            }

            var pointOfInterest = city.PointsOfInterest?.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            city.PointsOfInterest?.Remove(pointOfInterest);
            _mailService.SendEmail($"{pointOfInterest.Name} Deleted", $"{pointOfInterest.Name} is deleted from the city {city.Name}");
            return NoContent();
            */
        }
    }
}

