using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Repository.Models;
using UserAPI.AuthenticationHelper;

namespace UserApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_testingPolicy",
                builder => builder
                    .WithOrigins(
                        "http://localhost:4200", //testing purposes
                        "http://20.45.6.142", //forum
                        "http://20.189.30.176", //review
                        "http://20.189.29.112", //admintools
                        "https://cinephiliacs.org", // frontend
                        "http://20.94.153.81", //movies
                        "https://cinephiliacsapp.azurewebsites.net" // frontend
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            var myConnectionString = Configuration.GetConnectionString("Cinephiliacs_User");
            services.AddDbContext<Cinephiliacs_UserContext>(
                options => options.UseSqlServer(myConnectionString)
            );
            services.AddScoped<BusinessLogic.Interfaces.IUserLogic, BusinessLogic.UserLogic>();
            services.AddScoped<Repository.RepoLogic>();


            // for authentication
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = "scheme";
            })
            .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(
                "scheme", o => { });

            var permissions = new[] {
                // "loggedin", // for signed in
                "manage:forums", // for moderator (is signed in)
                "manage:awebsite", // for admin (is moderator and signed in)
            };
            services.AddAuthorization(options =>
            {
                for (int i = 0; i < permissions.Length; i++)
                {
                    options.AddPolicy(permissions[i], policy => policy.RequireClaim(permissions[i], "true"));
                }
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets/app-secrets.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enables the CORS policty for all controller endpoints. Must come between UseRouting() and UseEndpoints()
            app.UseCors("_testingPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
