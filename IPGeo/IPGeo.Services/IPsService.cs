using System;
using System.Linq;
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
            var ipRecord = _context.IPs.First(d => ip >= d.IpFrom && ip <= d.IpTo);

            if (ipRecord.CountryId == null)
                return null;

            _context
                .Entry(ipRecord)
                .Reference(i => i.Country)
                .Load();

            return new CountryDTO(ipRecord.Country);
        }
    }
}
