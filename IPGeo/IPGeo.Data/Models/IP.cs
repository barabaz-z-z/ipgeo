using System;
using System.Collections.Generic;

namespace IPGeo.Data.Models
{
    public partial class IP
    {
        public UInt32 IpFrom { get; set; }
        public UInt32 IpTo { get; set; }
        public int? CountryId { get; set; }

        public Country Country { get; set; }
    }
}
