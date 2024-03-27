using NUnit.Framework;
using System.Linq;
using ServiMotor.Business.Models;
using ServiMotor.Infraestructure;
using System.Threading.Tasks;
using Bogus;
using ServiMotor.Business.Shared;

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
                await _repository.CreateAsync(extract);
            }
            
            var data = await _repository.GetAllAsync();
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
            firstElement.Name = newElement.Name;
            await _repository.UpdateAsync(firstElement);
            var updatedElement = await _repository.GetAsync(idFirstElement);

            // elements that change
            Assert.AreEqual(newElement.Name, updatedElement.Name);
            Assert.AreEqual(newElement.Bank, updatedElement.Bank);
            // detail not change
            Assert.AreEqual(detailFirstElement, updatedElement.Detail);
        }
    }
}