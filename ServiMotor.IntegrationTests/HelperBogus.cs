using Bogus;
using ServiMotor.Business.Models;

namespace ServiMotor.IntegrationTests
{
    public static class HelperBogus
    {
        public static Faker<Extract> GetFakerExtract()
        {
            return new Faker<Extract>()
                .RuleFor(u => u.Balance, (f, u) => f.Finance.Amount(-1000, 1000, 2))
                .RuleFor(u => u.Date, (f, u) => f.Date.Recent())
                .RuleFor(u => u.Name, (f, u) => f.Commerce.Department())
                .RuleFor(u => u.Detail, (f, u) => f.Lorem.Word())
                .RuleFor(u => u.Bank, (f, u) => new Faker<Bank>()
                    .RuleFor(b => b.Name, (b, s) => b.Company.CompanyName())
                )
                .RuleFor(u => u.Bank, (f, u) => GetFakerBank())
                .RuleFor(u => u.BranchOffice, GetFakerBranchOffice());
        }

        public static Faker<Bank> GetFakerBank()
        {
            return new Faker<Bank>()
                .RuleFor(b => b.Name, (b, s) => b.Company.CompanyName())
                .RuleFor(b => b._id, (b, s) => MongoDB.Bson.ObjectId.GenerateNewId());
        }

        public static Faker<BranchOffice> GetFakerBranchOffice()
        {
            return new Faker<BranchOffice>()
                .RuleFor(b => b.Name, (b, s) => b.Company.CompanyName())
                .RuleFor(b => b._id, (b, s) => MongoDB.Bson.ObjectId.GenerateNewId());
        }
    }
}
