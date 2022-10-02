using NUnit.Framework;
using ServiMotor.Business.Features.Oils.Interfaces;
using ServiMotor.Business.Infraestructure;
using System.Linq;
using ServiMotor.Business.Models;
using ServiMotor.Infraestructure;
using System.Threading.Tasks;

namespace ServiMotor.IntegrationTests
{
    public class IntegrationRepositoryTest
    {
        private IOilRepository _repository;

        [SetUp]
        public void Setup()
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new OilRepository(new MongoBookDBContext(DbFix.DbContextSettings));
        }

        [Test]
        public async Task CheckCreateAndGetAsync()
        {
            await _repository.Create(new Oil());
            var data = await _repository.Get();
            Assert.GreaterOrEqual(data.Count(),1);
            Assert.Pass("Se han creado y obtenido datos de MongoDB");
        }
    }
}