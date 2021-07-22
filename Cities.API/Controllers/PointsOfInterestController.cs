using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cities.API.Models;

namespace Cities.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController: ControllerBase
    {
        [HttpGet]
        public IActionResult GetPointIfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(f => f.Id == cityId);

            if(city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointofInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(f=>f.Id == cityId);

            if(city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(f => f.Id == id);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id == cityId);

            if(city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestCreationDto.Name,
                Description = pointOfInterestCreationDto.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new {cityId, id = finalPointOfInterest.Id},
                finalPointOfInterest
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

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p=>p.Id == id);

            if(pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody]JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            var cityFromStore = CitiesDataStore.Current.Cities
                .FirstOrDefault(c=>c.Id == cityId);
            if (cityFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = cityFromStore.PointsOfInterest
                .FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            patchDoc.ApplyTo(pointOfInterestToPatch,ModelState);

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
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            return NoContent();
        }
    }
}
