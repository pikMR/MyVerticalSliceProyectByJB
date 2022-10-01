using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiMotor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OilsController : ControllerBase
    {
        private readonly ILogger<OilsController> _logger;

        public OilsController(ILogger<OilsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // TODO
            return null;
        }
    }
}
