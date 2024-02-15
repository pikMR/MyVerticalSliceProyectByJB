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
    public class ExtractRepositoryTest
    {
        private IBaseRepository<Extract> _repository;
        private Faker<Extract> fakerExtract;

        [OneTimeSetUp]
        public void Setup() 
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<Extract>(new MongoBookDBContext(DbFix.DbContextSettings));
            _repository.DeleteAll();
            fakerExtract = HelperBogus.GetFakerExtract();
        }

        [Test, Order(1)]
        public async Task CheckCreateAndGetAsync()
        {
           var extracts = fakerExtract.GenerateBetween(10, 20);

            foreach (var extract in extracts)
            {
                await _repository.Create(extract);
            }
            
            var data = await _repository.Get();
            Assert.GreaterOrEqual(data.Count(), 10);
            Assert.Pass("Se han creado y obtenido datos de MongoDB");
        }

        [Test, Order(2)]
        public async Task CheckUpdateAndGetAsync()
        {
            var newElement = fakerExtract.Generate(1).First();
            var firstElement = await _repository.GetFirstAsync();
            var idFirstElement = firstElement._id.ToString();
            var detailFirstElement = firstElement.Detail;

            firstElement.Bank = newElement.Bank;
            firstElement.Description = newElement.Description;
            await _repository.UpdateAsync(firstElement);
            var updatedElement = await _repository.Get(idFirstElement);

            // elements that change
            Assert.AreEqual(newElement.Description, updatedElement.Description);
            Assert.AreEqual(newElement.Bank, updatedElement.Bank);
            // detail not change
            Assert.AreEqual(detailFirstElement, updatedElement.Detail);
        }
    }
}