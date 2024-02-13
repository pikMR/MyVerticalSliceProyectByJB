using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using NUnit.Framework;
using ServiMotor.Business.Models;
using ServiMotor.Controllers;
using ServiMotor.Features.Extracts;
using ServiMotor.Features.Interfaces;
using ServiMotor.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ServiMotor.Features.Extracts.GetAll.Result;

namespace ServiMotor.IntegrationTests
{
    public class ApiTest
    {
        private IBaseRepository<Business.Models.Extract> _repository;
        private Faker<Business.Models.Extract> fakerExtract;
        private ExtractController controller;
        private HttpClient _client;


        [SetUp]
        public void Setup()
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<Business.Models.Extract>(new MongoBookDBContext(DbFix.DbContextSettings));
            fakerExtract = HelperBogus.GetFakerExtract();
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();

            // create elements for api
            _repository.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            // Libera los recursos después de cada prueba
            _client.Dispose();
        }


        [Test]
        public async Task It_should_get_five_extracts()
        {
            await this.CreateFiveExtracts();
            var response = await _client.GetAsync("/Extract");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var json = await response.Content.ReadAsStringAsync();
            var extractsObjects = JsonSerializer.Deserialize<ExtractContainer>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Esto permite que las propiedades coincidan incluso si tienen diferentes casos.
            });

            Assert.AreEqual(5, extractsObjects.Extracts.Count());
            Assert.True(extractsObjects.Extracts.All(x => x.Id != null));
        }

        [Test]
        public async Task It_should_create_one_extract()
        {
            var newExtract = new Create.Command
            {
                Description = "Test Description",
                BankName = "Test Bank",
                Date = DateTime.Now,
                Balance = 100.50m,
                Detail = "Test Detail",
                BranchOfficeName = "Test Branch Office"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(newExtract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Extract", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getExtract = await _repository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(idResponse);
            Assert.NotNull(getExtract);
            Assert.AreEqual("Test Description", getExtract.Description);
            Assert.AreEqual("Test Bank", getExtract.Bank.Name);
            Assert.AreEqual(100.50m, getExtract.Balance);
            Assert.AreEqual("Test Detail", getExtract.Detail);
            Assert.AreEqual("Test Branch Office", getExtract.BranchOffice.Name);
        }

        [Test]
        public async Task It_should_update_one_extract()
        {
            var exampleExtract = fakerExtract.Generate(1).First();
            await _repository.Create(exampleExtract);

            var updatedExtract = new Update.Command
            {
                Id = exampleExtract._id.ToString(),
                Description = "Test Description 2",
                BankName = "Test Bank 2",
                Date = DateTime.Now,
                Balance = 101.50m,
                Detail = "Test Detail 2",
                BranchOfficeName = "Test Branch Office 2"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedExtract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/Extract", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getExtract = await _repository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(idResponse);
            Assert.NotNull(getExtract);
            Assert.AreEqual("Test Description 2", getExtract.Description);
            Assert.AreEqual("Test Bank 2", getExtract.Bank.Name);
            Assert.AreEqual(101.50m, getExtract.Balance);
            Assert.AreEqual("Test Detail 2", getExtract.Detail);
            Assert.AreEqual("Test Branch Office 2", getExtract.BranchOffice.Name);
        }

        public class ExtractContainer
        {
            public GetAll.Result.Extract[] Extracts { get; set; }
        }

        private async Task  CreateFiveExtracts()
        {
            var exampleExtracts = fakerExtract.Generate(5);
            foreach (var extract in exampleExtracts)
            {
                await _repository.Create(extract);
            }
        }
    }
}
