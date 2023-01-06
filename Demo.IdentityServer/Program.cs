using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Demo.IdentityServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            Console.Title = "IdentityServer";

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // .AddSigningCredential(new X509Certificate2(@".\certificates\test.pfx", "password"))

            _ = builder.Services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(InMemoryConfigs.IdentityResources())
                .AddInMemoryApiResources(InMemoryConfigs.ApiResources())
                .AddInMemoryClients(InMemoryConfigs.Clients())
                .AddInMemoryApiScopes(InMemoryConfigs.ApiScopes)
                .AddTestUsers(InMemoryConfigs.Users().ToList());


            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }

            // Allows IdentityServer to start intercepting routes and handle requests.
            _ = app.UseIdentityServer();

            _ = app.UseHttpsRedirection();

            app.Run();
        }
    }
}
