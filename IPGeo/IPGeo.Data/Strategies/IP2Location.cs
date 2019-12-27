using System;
using System.Collections.Generic;
using System.Text;

namespace IPGeo.Data.Strategies
{
    class IP2Location
    {
        public uint IPFrom { get; set; }
        public uint IPTo { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
