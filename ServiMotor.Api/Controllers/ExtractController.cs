using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiMotor.Features.Extracts;
using System.Threading.Tasks;

namespace ServiMotor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExtractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var extracts = await _mediator.Send(new GetAll.Query());
            return Ok(extracts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var result = await _mediator.Send(command);
            return Created("/Extract",result);
        }
    }
}