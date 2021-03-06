using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using MiPrimerWebApiM3.Helpers;
using MiPrimerWebApiM3.Models;
using MiPrimerWebApiM3.Services;

namespace MiPrimerWebApiM3
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
            //Add Autommaper to Map Entities
            services.AddAutoMapper(configuration => 
            {
                configuration.CreateMap<Autor, AutorDTO>();
                configuration.CreateMap<AutorPostDTO, Autor>();
                configuration.CreateMap<Libro, LibroDTO>();
                configuration.CreateMap<LibroPostDTO, Libro>();
            },typeof(Startup));


            //Add service for MIFiltroAccion filter
            services.AddScoped<MiFiltroDeAccion>();

            //Add conjunto de servicios para guardar informaci�n en cach�.
            services.AddResponseCaching();
            //Add authentication 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            
            services.AddTransient<ClaseB>();
            //Get connectionString
            string connectionString = Configuration.GetConnectionString("DefaultConnectionString");
            //Set dbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddControllers(options =>
            {
                //options.Filters.Add(new MiFIltroDeExcepcion());
                // Si hubiese Inyecci�n de dependencias en el filtro
                //options.Filters.Add(typeof(MiFiltroDeExcepcion)); 
            }).AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseResponseCaching();//a�adir funci�n para cach�
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
