using System;
using System.Collections.Generic;

namespace IPGeo.Data.Models
{
    public sealed class IPGeoDataPair
    {
        public long IpFrom { get; set; }
        public long IpTo { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
