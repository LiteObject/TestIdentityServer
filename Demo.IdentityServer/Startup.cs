using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Demo.IdentityServer
{
    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The configure services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // .AddSigningCredential(new X509Certificate2(@".\certificates\test.pfx", "password"))
            _ = services.AddLogging(
                    loggingBuilder =>
                        {
                            _ = loggingBuilder.AddConsole();
                            _ = loggingBuilder.AddDebug();
                        })
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(InMemoryConfigs.IdentityResources())
                .AddInMemoryApiResources(InMemoryConfigs.ApiResources())
                .AddInMemoryClients(InMemoryConfigs.Clients())
                .AddInMemoryApiScopes(InMemoryConfigs.ApiScopes)
                .AddTestUsers(InMemoryConfigs.Users().ToList());
        }

        /// <summary>
        /// The configure. This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        public void Configure(WebApplication app)
        {
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
        }
    }
}
