using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiMotor.Features.Banks;
using System.Threading.Tasks;

namespace ServiMotorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var banks = await _mediator.Send(new GetAll.Query());
            return Ok(banks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var result = await _mediator.Send(command);
            return Created("/Bank", result);
        }
    }
}
