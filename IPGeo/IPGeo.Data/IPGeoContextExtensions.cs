using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IPGeo.Data
{
    public static class IPGeoContextExtensions
    {
        public static IServiceCollection AddIPGeoContext(this IServiceCollection services)
        {
            return services
                .AddDbContext<IPGeoContext>(o => o.UseNpgsql("host=localhost;database=ipgeo;username=postgres;password=Password1!"));
        }
    }
}
