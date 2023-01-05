using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Demo.Api
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
            _ = services.AddControllers();

            _ = services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.Api", Version = "v1" });
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.Api v1"));
                IdentityModelEventSource.ShowPII = true;
            }

            _ = app.UseHttpsRedirection();

            _ = app.UseRouting();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });
        }
    }
}
