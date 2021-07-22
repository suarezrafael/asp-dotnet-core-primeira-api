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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok( CitiesDataStore.Current.Cities );
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
