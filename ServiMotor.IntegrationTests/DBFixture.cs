using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ServiMotor.Infraestructure;
using System;
using System.IO;

namespace ServiMotor.IntegrationTests
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Test.json").Build();
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
