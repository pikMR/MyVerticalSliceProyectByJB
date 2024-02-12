using Bogus;
using ServiMotor.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bogus.DataSets.Name;

namespace ServiMotor.IntegrationTests
{
    public static class HelperBogus
    {
        public static Faker<Extract> GetFakerExtract()
        {
            return new Faker<Extract>()
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
