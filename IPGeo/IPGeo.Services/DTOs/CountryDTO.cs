using System;
using System.Collections.Generic;
using System.Text;
using IPGeo.Data.Models;

namespace IPGeo.Services.DTOs
{
    public sealed class CountryDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public CountryDTO(Country country)
        {
            Id = country.Id;
            Code = country.Code;
            Name = country.Name;
        }
    }
}
