using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using ServiMotor.Infraestructure;
using System.Collections.Generic;

namespace ServiMotor.IntegrationTests.Configuration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly Mongosettings _configuration;
        public CustomWebApplicationFactory(Mongosettings configuration)
        {
            _configuration = configuration;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        { $"{nameof(Mongosettings)}:{nameof(_configuration.Connection)}", $"{_configuration.Connection}" },
                        { $"{nameof(Mongosettings)}:{nameof(_configuration.DatabaseName)}", $"{_configuration.DatabaseName}" }
                    }
                );
            });
        }
    }
}
