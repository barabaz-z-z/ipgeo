using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IPGeo.Data;

namespace IPGeo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IPGeoContext _context;

        public IndexController(IPGeoContext context)
        {
            _context = context;
        }

        [HttpGet("{ip}", Name = "GetCountryByIP")]
        public IActionResult GetCountry([FromRoute] string ip)
        {
            // TODO: add null validation
            // TODO: add correct ip string validation

            var bytes = System.Net.IPAddress.Parse(ip).GetAddressBytes();
            var intIP = BitConverter.ToUInt32(bytes, 0);

            var country = _context.IPs.First(d => intIP >= d.IpFrom && intIP <= d.IpTo);

            return new JsonResult(country);
        }
    }
}