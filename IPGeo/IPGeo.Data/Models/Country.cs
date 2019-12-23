using System;
using System.Collections.Generic;

namespace IPGeo.Data.Models
{
    public class Country
    {
        public Country()
        {
            IPs = new HashSet<IP>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<IP> IPs { get; set; }
    }
}
