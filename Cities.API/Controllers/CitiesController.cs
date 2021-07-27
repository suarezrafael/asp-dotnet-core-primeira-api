using Cities.API.Models;
using Cities.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.API.Controllers
{
    /// <summary>
    /// Comment
    /// </summary>
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCities()
        {
            //return Ok( CitiesDataStore.Current.Cities );
            var citiesEntities = _cityInfoRepository.GetCities();

            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach (var cityEntity in citiesEntities)
            {
                results.Add(
                    new CityWithoutPointsOfInterestDto() {
                        Id = cityEntity.Id,
                        Name = cityEntity.Name,
                        Description = cityEntity.Description
                    });
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToResult =  CitiesDataStore.Current.Cities.FirstOrDefault(f=>f.Id == id);

            if(cityToResult == null)
            {
                return NotFound();
            }

            return Ok(cityToResult);
        }

    }
}
