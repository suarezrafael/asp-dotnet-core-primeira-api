using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cities.API.Context;
using Cities.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cities.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddMvcOptions(o =>
            {
                o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

#if DEBUG
            services.AddTransient<IMailService,LocalMailService>();
#else
            services.AddTransient<IMailService,CloudMailService>();
#endif
            var connectionString = _configuration["connectionStrings:cityInfoDBConnectionString"];//@"Server=(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=True;";

            services.AddDbContext<CityInfoContext>(o=> {
                o.UseSqlServer(connectionString);
            });

            services.AddScoped<ICityInfoRepository,CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            app.UseStatusCodePages();

            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
