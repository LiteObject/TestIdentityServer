using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Demo.Api.Two
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Demo.Api.Two";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // OPTIONA #1: Global policy
            AuthorizationPolicy scopePolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "demoapi.one.read", "demoapi.one.write")
                .Build();

            // applying global policy
            _ = builder.Services.AddControllers(options =>
            {
                // Using option #2
                // options.Filters.Add(new AuthorizeFilter(scopePolicy));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            _ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidIssuer = "https://localhost:5001",
                        // ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidAudiences = new[] { "DemoApiTwo" } /* aud maps to ApiResource.Name in IS4 config */
                    };
                });

            // OPTIONA #2: Named (local) policy to decorate individual controllers and actions ([Authorize("demoapi.weatherforecast.read")])
            _ = builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("read-policy", policy => policy.RequireClaim("scope", "demoapi.one.read"));
                options.AddPolicy("write-policy", policy => policy.RequireClaim("scope", "demoapi.one.write"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}