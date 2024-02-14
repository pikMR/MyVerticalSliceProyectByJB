using NUnit.Framework;
using System.Linq;
using ServiMotor.Business.Models;
using ServiMotor.Infraestructure;
using System.Threading.Tasks;
using ServiMotor.Features.Extracts;
using ServiMotor.Features.Interfaces;
using Bogus;
using static Bogus.DataSets.Name;

namespace ServiMotor.IntegrationTests
{
    public class BankRepositoryTest
    {
        private IBaseRepository<Bank> _repository;
        private Faker<Bank> fakerBank;

        [OneTimeSetUp]
        public void Setup() 
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<Bank>(new MongoBookDBContext(DbFix.DbContextSettings));
            _repository.DeleteAll();
            fakerBank = HelperBogus.GetFakerBank();
        }

        [Test, Order(1)]
        public async Task CheckCreateAndGetAsync()
        {
           var banks = fakerBank.GenerateBetween(10, 20);

            foreach (var bank in banks)
            {
                await _repository.Create(bank);
            }
            
            var data = await _repository.Get();
            Assert.GreaterOrEqual(data.Count(), 10);
            Assert.Pass("Se han creado y obtenido datos de MongoDB");
        }

        [Test, Order(2)]
        public async Task CheckUpdateAndGetAsync()
        {
            var newElement = fakerBank.Generate(1).First();
            var firstElement = await _repository.GetFirstAsync();
            var idFirstElement = firstElement._id.ToString();
            var nameFirstElement = firstElement.Name;
            firstElement.Name = newElement.Name + "_updated";
            _repository.Update(firstElement);
            var updatedElement = await _repository.Get(idFirstElement);

            // elements that change
            Assert.AreEqual(firstElement.Name, updatedElement.Name);
            // detail not change
            Assert.AreEqual(idFirstElement, updatedElement._id.ToString());
        }
    }
}