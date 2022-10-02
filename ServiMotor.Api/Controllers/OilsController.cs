using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiMotor.Features.Oils;
using System.Threading.Tasks;

namespace ServiMotor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OilsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OilsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var oils = await _mediator.Send(new GetAll.Query());
            return Ok(oils);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}