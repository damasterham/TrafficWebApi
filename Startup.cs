using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TrafikWebAPI.Models;

namespace TrafikWebAPI
{

    // So at the moment for this case, I cannot really think of any aplicable design patterns
    // other than the ones inhentily used with the MVC framewok, instead of implementing our own singleton for example
    // we could make use of just registering a transient service
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // So for this case i'll just be utilizing EF Core, since ORM's are nice and simple
            // We could of course delve into using different types of DBs within the SQL NoSQL realm
            // either by still utlizing ef core for whatever it can support, or make custom data access possible
            // Either would most likely beneift from an added layer of abstraction making interfaces around 
            // the different data access to allow to for easier swapping of data access or 'services'

            // Adding an in memory database for this case, which from what can suss utilized sqlite
            // We are registering here with the ServiceProvider so we can access it throught this project
            services.AddDbContext<TrafficInfoContext>(options => options.UseInMemoryDatabase("TrafficDb"));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // To ensure Seed Data is created for the LineInfoEntries
            var context = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope()
                .ServiceProvider
                .GetService<TrafficInfoContext>();
            context.Database.EnsureCreated();
        }
    }
}
