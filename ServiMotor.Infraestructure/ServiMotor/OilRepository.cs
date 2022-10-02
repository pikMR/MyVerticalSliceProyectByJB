using ServiMotor.Business.Models;
using ServiMotor.Features.Oils;

namespace ServiMotor.Infraestructure
{
    public class OilRepository : BaseRepository<Oil>, IOilRepository
    {
        public OilRepository(IMongoBookDBContext context) : base(context)
        {
        }
    }
}