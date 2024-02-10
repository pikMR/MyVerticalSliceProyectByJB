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
    public class IntegrationRepositoryTest
    {
        private IBaseRepository<Extract> _repository;
        private Faker<Extract> fakerExtract;

        [SetUp]
        public void Setup()
        {
            DbFixture DbFix = new();
            DbFix.Dispose();
            _repository = new BaseRepository<Extract>(new MongoBookDBContext(DbFix.DbContextSettings));
            fakerExtract = GetFakerExtract();
        }

        [Test]
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

        [Test]
        public async Task CheckUpdateAndGetAsync()
        {

        }

        private Faker<Extract> GetFakerExtract()
        {
            return = new Faker<Extract>()
                .RuleFor(u => u.Balance, (f, u) => f.Finance.Amount(-1000, 1000, 2))
                .RuleFor(u => u.Date, (f, u) => f.Date.Recent())
                .RuleFor(u => u.Description, (f, u) => f.Commerce.Department())
                .RuleFor(u => u.Detail, (f, u) => f.Lorem.Word())
                .RuleFor(u => u.Bank, (f, u) => new Faker<Bank>()
                    .RuleFor(b => b.Name, (b, s) => b.Company.CompanyName())
                )
                .RuleFor(u => u.Bank, (f, u) => new Faker<Bank>()
                    .RuleFor(b => b.Name, (b, s) => b.Company.CompanyName()))
                .RuleFor(u => u.BranchOffice, (f, u) => new Faker<BranchOffice>()
                    .RuleFor(b => b.Name, (b, s) => b.Name.FullName(Gender.Male)));
        }
    }
}