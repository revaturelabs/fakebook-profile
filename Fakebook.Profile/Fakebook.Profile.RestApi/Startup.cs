using System.Configuration;
using System.IO;
using Azure.Storage.Blobs;
using Fakebook.Profile.DataAccess.EntityModel;
using Fakebook.Profile.DataAccess.Services;
using Fakebook.Profile.DataAccess.Services.Interfaces;
using Fakebook.Profile.Domain.Utility;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Okta.AspNetCore;

namespace Fakebook.Profile.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://dev-7862904.okta.com/oauth2/default";
                    options.Audience = "api://default";
                    // Won't send details outside of dev env
                    if (_env.IsDevelopment())
                    {
                        options.IncludeErrorDetails = true;
                    }
                    options.RequireHttpsMetadata = false;
                    // Add okta auth
                }).AddOktaMvc(new OktaMvcOptions
                {
                    OktaDomain = "https://dev-7862904okta.com/oauth2/default",
                    ClientId = "CLIENT_ID_HERE",
                    ClientSecret = "CLIENT_SECRET_HERE",
                }
                );

            services.AddControllers();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fakebook.ProfileRestApi", Version = "v1" });
            });


            services.AddDbContext<ProfileDbContext>(options
                => options.UseNpgsql(Configuration["FakebookProfile:ConnectionString"]));

            services.AddScoped(x => new BlobServiceClient(Configuration.GetConnectionString("BlobStorage")));
            services.AddTransient<IStorageService, AzureBlobStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fakebook.ProfileRestApi v1"));
            }

            ProfileConfiguration.DefaultUri = "https://publicdomainvectors.org/photos/defaultprofile.png";
            ProfileConfiguration.BlobContainerName = "fakebook"; // a temp value

            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
