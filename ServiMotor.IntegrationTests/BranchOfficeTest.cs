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
    public class BranchOfficeRepositoryTest
    {
        private IBaseRepository<BranchOffice> _repository;
        private Faker<BranchOffice> fakerBranchOffice;

        [OneTimeSetUp]
        public void Setup() 
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<BranchOffice>(new MongoBookDBContext(DbFix.DbContextSettings));
            _repository.DeleteAll();
            fakerBranchOffice = HelperBogus.GetFakerBranchOffice();
        }

        [Test, Order(1)]
        public async Task CheckCreateAndGetAsync()
        {
           var branchOffices = fakerBranchOffice.GenerateBetween(10, 20);

            foreach (var branchOffice in branchOffices)
            {
                await _repository.Create(branchOffice);
            }
            
            var data = await _repository.Get();
            Assert.GreaterOrEqual(data.Count(), 10);
            Assert.Pass("Se han creado y obtenido datos de MongoDB");
        }

        [Test, Order(2)]
        public async Task CheckUpdateAndGetAsync()
        {
            var newElement = fakerBranchOffice.Generate(1).First();
            var firstElement = await _repository.GetFirstAsync();
            firstElement.Name = newElement.Name + "_updated";
            await _repository.UpdateAsync(firstElement);
            var updatedElement = await _repository.Get(firstElement._id.ToString());

            // elements that change
            Assert.AreEqual(firstElement.Name, updatedElement.Name);
            // detail not change
            Assert.AreEqual(firstElement._id.ToString(), updatedElement._id.ToString());
        }
    }
}