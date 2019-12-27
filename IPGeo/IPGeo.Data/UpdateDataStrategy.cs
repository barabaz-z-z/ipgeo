using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using IPGeo.Data.Models;

namespace IPGeo.Data
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
            throw new NotImplementedException();
        }
    }
}
