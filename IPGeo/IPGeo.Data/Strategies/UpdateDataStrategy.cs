using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using IPGeo.Data.Models;

namespace IPGeo.Data.Strategies
{
    public sealed class UpdateDataStrategy : ISetDataStrategy
    {
        private readonly IPGeoContext _context;

        public UpdateDataStrategy(IPGeoContext context)
        {
            _context = context;
        }

        public void SetData(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader))
            {
                var countries = new HashSet<string>();
                Country country;

                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Configuration.Delimiter = ",";

                _context.IPs
                    .Include(i => i.Country)
                    .Load();

                foreach (var record in csvReader.GetRecords<IP2Location>())
                {
                    var ip = _context.IPs.Find(record.IPFrom, record.IPTo);

                    if (ip == null)
                    {
                        country = _context.Countries.First(c => c.Code == record.CountryCode);

                        var newIP = new IP
                        {
                            IpFrom = record.IPFrom,
                            IpTo = record.IPTo,
                            Country = country,
                            CountryId = country.Id
                        };

                        _context.IPs.Add(newIP);

                        continue;
                    }

                    var countryCode = record.CountryCode.Length < 2 ? null : record.CountryCode;

                    if (countryCode == null && ip.CountryId == null)
                        continue;

                    if (countryCode == null)
                    {
                        ip.Country = null;
                        ip.CountryId = null;
                    }
                    else if (ip.CountryId == null || ip.Country.Code != countryCode)
                    {
                        country = _context.Countries.First(c => c.Code == record.CountryCode);

                        ip.Country = country;
                        ip.CountryId = country.Id;
                    }
                }

                if (_context.ChangeTracker.HasChanges())
                {
                    var history = _context.History.LastOrDefault();
                    history.UpdatedAt = DateTime.Today;

                    _context.SaveChanges();
                }
            }
        }
    }
}
