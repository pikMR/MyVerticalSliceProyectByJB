using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiMotor.Features.BranchOffices;
using System.Threading.Tasks;

namespace ServiMotorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BranchOfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchOfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branchsOffices = await _mediator.Send(new GetAll.Query());
            return Ok(branchsOffices);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var result = await _mediator.Send(command);
            return Created("/BranchOffice", result);
        }
    }
}
