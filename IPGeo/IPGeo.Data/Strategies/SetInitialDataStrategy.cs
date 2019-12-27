using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using IPGeo.Data.Models;

namespace IPGeo.Data.Strategies
{
    public sealed class SetInitialDataStrategy : ISetDataStrategy
    {
        private readonly IPGeoContext _context;

        public SetInitialDataStrategy(IPGeoContext context)
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
                foreach (var record in csvReader.GetRecords<IP2Location>())
                {
                    var ip = new IP
                    {
                        IpFrom = record.IPFrom,
                        IpTo = record.IPTo
                    };

                    if (record.CountryCode.Length > 1)
                    {
                        if (!countries.Contains(record.CountryCode))
                        {
                            countries.Add(record.CountryCode);

                            country = new Country
                            {
                                Code = record.CountryCode,
                                Name = record.CountryName
                            };

                            _context.Countries.Add(country);
                        }
                        else
                        {
                            country = _context.Countries.Local.First(c => c.Code == record.CountryCode);
                        }

                        ip.Country = country;
                    }

                    _context.IPs.Add(ip);
                }

                _context.History.Add(new History { UpdatedAt = DateTime.Today });
                _context.SaveChanges();
            }
        }
    }
}
