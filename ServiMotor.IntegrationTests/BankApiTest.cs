using Bogus;
using NUnit.Framework;
using ServiMotor.Business.Shared;
using ServiMotor.Features.Banks;
using ServiMotor.Infraestructure;
using ServiMotor.IntegrationTests.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiMotor.IntegrationTests
{
    public class BankApiTest
    {
        private IBaseRepository<Business.Models.Bank> _repository;
        private Faker<Business.Models.Bank> fakerBank;
        private HttpClient _client;


        [SetUp]
        public void Setup()
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<Business.Models.Bank>(new MongoBookDBContext(DbFix.DbContextSettings));
            fakerBank = HelperBogus.GetFakerBank();
            _client = new CustomWebApplicationFactory(DbFix.DbContextSettings).CreateClient();
            _repository.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            // Libera los recursos después de cada prueba
            _client.Dispose();
        }


        [Test]
        public async Task It_should_get_five_banks()
        {
            await this.CreateFiveBanks();
            var response = await _client.GetAsync("/Bank");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var json = await response.Content.ReadAsStringAsync();
            var banksObjects = JsonSerializer.Deserialize<BankContainer>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Esto permite que las propiedades coincidan incluso si tienen diferentes casos.
            });

            Assert.AreEqual(5, banksObjects.Banks.Count());
            Assert.True(banksObjects.Banks.All(x => x.Id != null));
        }

        [Test]
        public async Task It_should_create_one_bank()
        {
            var newBank = new Create.Command
            {
                Name = "Test Bank",
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(newBank), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Bank", jsonContent);
            var idResponse = await response.Content.ReadAsStringAsync();
            var getBank = await _repository.GetFirstAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(idResponse);
            Assert.NotNull(getBank);
            Assert.AreEqual("Test Bank", getBank.Name);
            Assert.NotNull(getBank._id);
        }

        public class BankContainer
        {
            public GetAll.Result.BankDto[] Banks { get; set; }
        }

        private async Task CreateFiveBanks()
        {
            var exampleBanks = fakerBank.Generate(5);
            foreach (var bank in exampleBanks)
            {
                await _repository.CreateAsync(bank);
            }
        }
    }
}
