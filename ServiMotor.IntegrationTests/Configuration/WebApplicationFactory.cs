using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiMotor.Infraestructure;

namespace ServiMotor.IntegrationTests.Configuration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            builder.ConfigureTestServices(services =>
                services.AddOptions<MongoBookDBContext>(configuration.GetSection("Mongosettings").Value)
            );

            //builder.ConfigureAppConfiguration((host, configurationBuilder) =>
            //{
            //    configurationBuilder.AddInMemoryCollection(
            //        new List<KeyValuePair<string, string?>>
            //        {
            //            new KeyValuePair<string, string?>("Mongosettings", "FromTests")
            //        });
            //});

            builder.ConfigureAppConfiguration((host, configurationBuilder) => { });
        }
    }
}
