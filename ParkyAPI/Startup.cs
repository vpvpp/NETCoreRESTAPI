using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using ParkyAPI.Mapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("ParkyConnection")));
             
          
            services.AddScoped<INParkRepository, NParkRepository>(); //using this we can acess nationalPark repository in any other controllers.
            services.AddScoped<ITrailRepository, TrailRepository>(); //using this we can acess nationalPark repository in any other controllers.

            services.AddAutoMapper(typeof(ParkyMappings)); //registraring Mapping

            //API version configuration after  installing nuget packages
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);  //this will be visiable in swagger document response header.
                options.ReportApiVersions = true;


            });

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOption>();


            services.AddSwaggerGen();



            //registraraing swagger generator
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("ParkyOpenAPISpec",                 //swagger openapi specification/document configuration.
            //        new Microsoft.OpenApi.Models.OpenApiInfo()
            //        {
            //            Title = "Parky API",
            //            Version = "1",
            //            Description="Udemy course API National Park",
            //            Contact = new Microsoft.OpenApi.Models.OpenApiContact()  //extra feature in api documentation
            //            {
            //                Email="vinod_p7508@outlook.com",
            //                Name="vinod",
            //                Url= new Uri("https://wwww.bhrugen.com"),

            //            },
            //            License = new Microsoft.OpenApi.Models.OpenApiLicense() 
            //                {
            //                Name = "MIT License",
            //                Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //                }
            //            //we can also add extra extension here
            //        });


            //    //options.SwaggerDoc("ParkyOpenAPISpecTrails",                 //swagger openapi specification for trail end points
            //    //new Microsoft.OpenApi.Models.OpenApiInfo()
            //    //{
            //    //    Title = "Parky API(Trails)",
            //    //    Version = "1",
            //    //    Description = "Udemy course API Trails",
            //    //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()  //extra feature in api documentation
            //    //        {
            //    //        Email = "vinod_p7508@outlook.com",
            //    //        Name = "vinod",
            //    //        Url = new Uri("https://wwww.bhrugen.com"),

            //    //    },
            //    //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //    //    {
            //    //        Name = "MIT License",
            //    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //    //    }
            //    //        //we can also add extra extension here
            //    //    });

            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(cmlCommentsFullPath);
            //});

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Configuration for swaggerUI.
            app.UseSwagger();


            //we have uses below swagger UI configuration in after setting 
            app.UseSwaggerUI(options =>
            {
              foreach(var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });


            //initial lectures
            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");  //configuring seperate end point for 
            //    options.RoutePrefix = "";   //to set swaggerUI as default opening window when we run the application.
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
