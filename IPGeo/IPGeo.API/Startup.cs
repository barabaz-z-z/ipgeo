using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IPGeo.Data;
using IPGeo.Services;

namespace IPGeo.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIPGeoContext();
            services.AddMvc();
            services.AddScoped<IPsService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
