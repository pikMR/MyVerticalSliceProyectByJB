using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        [HttpGet("Bank/{idBank}")]
        public async Task<IActionResult> GetByBank(string idBank)
        {
            ObjectId.TryParse(idBank, out ObjectId filterParamIdBank);
            var query = new GetAll.Query()
            {
                Filter = (x) => x.Bank._id == filterParamIdBank
            };

            var extracts = await _mediator.Send(query);
            return Ok(extracts);
        }

        [HttpGet("Bank/{idBank}/BranchOffice/{idBranchOffice}")]
        public async Task<IActionResult> GetByBankAndBranchOffice(string idBank, string idBranchOffice)
        {
            ObjectId.TryParse(idBank, out ObjectId filterParamIdBank);
            ObjectId.TryParse(idBranchOffice, out ObjectId filterParamIdBranchOffice);
            var query = new GetAll.Query()
            {
                Filter = (x) => 
                    x.Bank._id == filterParamIdBank &&
                    x.BranchOffice._id == filterParamIdBranchOffice
            };

            var extracts = await _mediator.Send(query);
            return Ok(extracts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command commanda)
        {
            var result = await _mediator.Send(commanda);
            return Created("/Extract",result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Update.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}