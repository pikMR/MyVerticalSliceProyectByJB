using Microsoft.Extensions.Configuration;
using ServiMotor.Infraestructure;
using System;
using MongoDB.Driver;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace ServiMotor.IntegrationTests
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            DbContextSettings = new();
            config.GetSection(DbContextSettings.GetType().Name).Bind(DbContextSettings);
        }

        public Mongosettings DbContextSettings { get; }

        public void Dispose()
        {
            var client = new MongoClient(this.DbContextSettings.Connection);
            client.DropDatabase(this.DbContextSettings.DatabaseName);
        }
    }
}
