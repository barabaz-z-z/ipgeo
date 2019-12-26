using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IPGeo.Data;
using IPGeo.Services.DTOs;

namespace IPGeo.Services
{
    public sealed class IPsService
    {
        private readonly IPGeoContext _context;

        public IPsService(IPGeoContext context)
        {
            _context = context;
        }

        public CountryDTO GetCountry(string ipString)
        {
            var ip = IPHelper.ConvertToIPNumber(ipString);
            var ipRecord = _context.IPs
                .Include(i => i.Country)
                .First(d => ip >= d.IpFrom && ip <= d.IpTo);

            if (ipRecord.CountryId == null)
                return null;

            return new CountryDTO(ipRecord.Country);
        }
    }
}
