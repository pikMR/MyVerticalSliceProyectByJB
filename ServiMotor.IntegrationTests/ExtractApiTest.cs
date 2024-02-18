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
    public class ExtractApiTest
    {
        private IBaseRepository<Extract> _extractRepository;
        private IBaseRepository<Bank> _bankRepository;
        private IBaseRepository<BranchOffice> _branchRepository;
        private Faker<Extract> fakerExtract;
        private ExtractController controller;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _extractRepository = new BaseRepository<Extract>(new MongoBookDBContext(DbFix.DbContextSettings));
            _bankRepository = new BaseRepository<Bank>(new MongoBookDBContext(DbFix.DbContextSettings));
            _branchRepository = new BaseRepository<BranchOffice>(new MongoBookDBContext(DbFix.DbContextSettings));
            fakerExtract = HelperBogus.GetFakerExtract();
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();

            // delete all elements
            _bankRepository.DeleteAll();
            _branchRepository.DeleteAll();
            _extractRepository.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            // Libera los recursos después de cada prueba
            _client.Dispose();
        }

        [Test]
        public async Task It_should_get_two_extracts_filter_by_bank_and_branchoffice()
        {
            var bank = HelperBogus.GetFakerBank().Generate(1).First();
            var branchOffice = HelperBogus.GetFakerBranchOffice().Generate(1).First();
            await _bankRepository.Create(bank);
            await _branchRepository.Create(branchOffice);
            await this.CreateTwoExtractsWithBankAndBranchOffice(bank, branchOffice);
            await this.CreateFiveExtractsWithBank(bank);
            await this.CreateFiveExtracts();

            var response = await _client.GetAsync($"Extract/Bank/{bank._id}/BranchOffice/{branchOffice._id}");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var json = await response.Content.ReadAsStringAsync();
            var extractsObjects = JsonSerializer.Deserialize<ExtractContainer>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Esto permite que las propiedades coincidan incluso si tienen diferentes casos.
            });

            Assert.AreEqual(2, extractsObjects.Extracts.Count());
            Assert.True(extractsObjects.Extracts.All(x => x.Id != null));
        }

        [Test]
        public async Task It_should_get_five_extracts_filter_by_bank()
        {
            var bank = HelperBogus.GetFakerBank().Generate(1).First();
            await _bankRepository.Create(bank);
            await this.CreateFiveExtractsWithBank(bank);
            await this.CreateFiveExtracts();

            var response = await _client.GetAsync($"Extract/Bank/{bank._id}");
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
            var bank = HelperBogus.GetFakerBank().Generate(1).First();
            var branchOffice = HelperBogus.GetFakerBranchOffice().Generate(1).First();

            await _bankRepository.Create(bank);
            await _branchRepository.Create(branchOffice);

            var newExtract = new Create.Command
            {
                Description = "Test Description",
                Date = DateTime.Now,
                Balance = 100.50m,
                Detail = "Test Detail",
                Bank = new Features.Banks.Create.Command()
                {
                    Id = bank._id.ToString(),
                    Name = bank.Name,
                },
                BranchOffice = new Features.BranchOffices.Create.Command()
                {
                    Id = branchOffice._id.ToString(),
                    Name = branchOffice.Name,
                },
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(newExtract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Extract", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getExtract = await _extractRepository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(idResponse);
            Assert.NotNull(getExtract);
            Assert.AreEqual("Test Description", getExtract.Description);
            Assert.AreEqual(100.50m, getExtract.Balance);
            Assert.AreEqual("Test Detail", getExtract.Detail);
            Assert.AreEqual(bank.Name, getExtract.Bank.Name);
            Assert.AreEqual(bank._id.ToString(), getExtract.Bank._id.ToString());
            Assert.AreEqual(branchOffice.Name, getExtract.BranchOffice.Name);
            Assert.AreEqual(branchOffice._id.ToString(), getExtract.BranchOffice._id.ToString());
        }

        [Test]
        public async Task It_should_update_one_extract_with_new_bank_and_branch()
        {
            // create bank and branch to update
            var bank = HelperBogus.GetFakerBank().Generate(1).First();
            var branchOffice = HelperBogus.GetFakerBranchOffice().Generate(1).First();
            await _branchRepository.Create(branchOffice);
            await _bankRepository.Create(bank);

            // create new extract with random bank and branch
            var exampleExtract = fakerExtract.Generate(1).First();
            await _extractRepository.Create(exampleExtract);

            var updatedExtract = new Update.Command
            {
                Id = exampleExtract._id.ToString(),
                Description = "Test Description 2",
                Date = DateTime.Now,
                Balance = 101.50m,
                Detail = "Test Detail 2",
                Bank = new Features.Banks.Create.Command()
                {
                    Id = bank._id.ToString(),
                    Name = "this name dont should updated",
                },
                BranchOffice = new Features.BranchOffices.Create.Command()
                {
                    Id = branchOffice._id.ToString(),
                    Name = "this name dont should updated",
                },
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedExtract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/Extract", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getExtract = await _extractRepository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getExtract);
            Assert.AreEqual("Test Description 2", getExtract.Description);
            Assert.AreEqual(101.50m, getExtract.Balance);
            Assert.AreEqual("Test Detail 2", getExtract.Detail);
            Assert.AreEqual(bank.Name, getExtract.Bank.Name);
            Assert.AreEqual(bank._id, getExtract.Bank._id);
            Assert.AreEqual(branchOffice.Name, getExtract.BranchOffice.Name);
            Assert.AreEqual(branchOffice._id, getExtract.BranchOffice._id);
        }

        public async Task It_should_update_one_extract_and_bank_and_branchOffice()
        {
            var bank = HelperBogus.GetFakerBank().Generate(1).First();
            var branchOffice = HelperBogus.GetFakerBranchOffice().Generate(1).First();
            var exampleExtract = fakerExtract.Generate(1).First();
            await _extractRepository.Create(exampleExtract);

            var updatedExtract = new Update.Command
            {
                Id = exampleExtract._id.ToString(),
                Description = "Test Description 2",
                Date = DateTime.Now,
                Balance = 101.50m,
                Detail = "Test Detail 2",
                Bank = new Features.Banks.Create.Command()
                {
                    Name = bank.Name,
                },
                BranchOffice = new Features.BranchOffices.Create.Command()
                {
                    Name = branchOffice.Name,
                },
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedExtract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/Extract", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getExtract = await _extractRepository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getExtract);
            Assert.AreEqual("Test Description 2", getExtract.Description);
            Assert.AreEqual(101.50m, getExtract.Balance);
            Assert.AreEqual("Test Detail 2", getExtract.Detail);
            Assert.AreEqual(bank.Name, getExtract.Bank.Name);
            Assert.NotNull(getExtract.Bank._id);
            Assert.AreEqual(branchOffice.Name, getExtract.BranchOffice.Name);
            Assert.NotNull(getExtract.BranchOffice._id);
        }

        public class ExtractContainer
        {
            public ExtractDto[] Extracts { get; set; }
        }

        private async Task  CreateFiveExtracts()
        {
            var exampleExtracts = fakerExtract.Generate(5);
            foreach (var extract in exampleExtracts)
            {
                await _extractRepository.Create(extract);
            }
        }

        private async Task CreateFiveExtractsWithBank(Bank bank)
        {
            var exampleExtracts = fakerExtract.Generate(5);
            foreach (var extract in exampleExtracts)
            {
                extract.Bank = bank;
                await _extractRepository.Create(extract);
            }
        }

        private async Task CreateTwoExtractsWithBankAndBranchOffice(Bank bank, BranchOffice branchOffice)
        {
            var exampleExtracts = fakerExtract.Generate(2);
            foreach (var extract in exampleExtracts)
            {
                extract.Bank = bank;
                extract.BranchOffice = branchOffice;
                await _extractRepository.Create(extract);
            }
        }
    }
}
