using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IPGeo.Services;

namespace IPGeo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPsController : ControllerBase
    {
        private readonly IPsService _ipsService;

        public IPsController(IPsService ipsService)
        {
            _ipsService = ipsService;
        }

        [HttpGet("{ipString}/country", Name = "GetCountryByIP")]
        public IActionResult GetCountry([FromRoute] string ipString)
        {
            if (!IPHelper.IsCorrectIP(ipString))
                return BadRequest("ip is not correct");

            var country = _ipsService.GetCountry(ipString);
            if (country == null)
                return Ok("country is not provided for this ip");

            return new JsonResult(country);
        }
    }
}