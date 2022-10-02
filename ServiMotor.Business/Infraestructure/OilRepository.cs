using ServiMotor.Business.Features.Oils.Interfaces;
using ServiMotor.Business.Models;
using ServiMotor.Infraestructure;

namespace ServiMotor.Business.Infraestructure
{
    public class OilRepository : BaseRepository<Oil>, IOilRepository
    {
        public OilRepository(IMongoBookDBContext context) : base(context)
        {
        }
    }
}