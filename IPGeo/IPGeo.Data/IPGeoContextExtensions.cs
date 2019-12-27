using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IPGeo.Data
{
    public static class IPGeoContextExtensions
    {
        public static IServiceCollection AddIPGeoContext(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("datasettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("Postgre");

            return services
                .AddDbContext<IPGeoContext>(o => o.UseNpgsql(connectionString));
        }
    }
}
