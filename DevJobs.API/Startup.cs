using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;

namespace DevJobs.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DevJobsCs");

            services.AddDbContext<DevJobsContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevJobs.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Guilherme",
                        Email = "contato@gmail.com",
                        Url = new Uri("https://google.com")
                    }
                });

                var xmlFile = "DevJobs.API.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            //var hostBuilder = new HostBuilder();

            //hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    Log.Logger = new LoggerConfiguration()
            //    .Enrich
            //    .FromLogContext()
            //    .WriteTo.MSSqlServer(connectionString,
            //    sinkOptions: new MSSqlServerSinkOptions()
            //    {
            //        AutoCreateSqlTable = true,
            //        TableName = "Logs"
            //    })
            //    //.WriteTo.Console()
            //    .CreateLogger();
            //});//.UseSerilog();


            services.AddScoped<IJobVacancyRepository, JobVacancyRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/cadastro-vaga", "DevJobs.API v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
                
            });
        }
    }
}
