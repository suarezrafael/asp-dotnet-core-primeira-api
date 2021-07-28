using AutoMapper;
using Cities.API.Models;
using Cities.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cities.API.Controllers
{
    /// <summary>
    /// Controller api from cities
    /// </summary>
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// GET Point of interest
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing point of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
           if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId}  wasn't found when accessing point of interest.");
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId,id);

            if (pointOfInterest == null)
            {
                _logger.LogInformation($"Point Of Interest with id {id} wasn't found.");
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public IActionResult CreatePointForInterest(int cityId,
            [FromBody]PointOfInterestForCreationDto pointOfInterestCreationDto)
        {

            if (pointOfInterestCreationDto.Description == pointOfInterestCreationDto.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't " +
                    $"found when accessing point of interest.");
                return NotFound();
            }
            // converte Dto para Entity
            var finalPointOfInterest = _mapper
                .Map<Entities.PointOfInterest>(pointOfInterestCreationDto);

            _cityInfoRepository.AddPointOfInterest(cityId, finalPointOfInterest);

            _cityInfoRepository.Save();

            var createdPointOfInterestToReturn = _mapper
                .Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = createdPointOfInterestToReturn.Id },
                createdPointOfInterestToReturn
                );
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId) )
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            
        
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterest(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

           return NoContent();
        }

        /// <summary>
        /// Partial Updade
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody]JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {

            if (!_cityInfoRepository.CityExists(cityId) )
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository
                .GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            // Caso queira usar autoMapper
            //var pointOfInterestToPatch = _mapper
            //    .Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestEntity.Name,
                    Description = pointOfInterestEntity.Description
                };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description",
                    "The provided description should be different from the name");
            }
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            // Caso queira usar autoMapper
            //_mapper.Map(pointOfInterestToPatch,pointOfInterestEntity);
            pointOfInterestEntity.Name = pointOfInterestToPatch.Name;
            pointOfInterestEntity.Description = pointOfInterestToPatch.Description;

            _cityInfoRepository
                .UpdatePointOfInterest(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository
                .GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            _cityInfoRepository.Save();

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted."
                );
            return NoContent();
        }
    }
}
