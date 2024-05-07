using InvoiceService.Models;
using Microsoft.AspNetCore.Mvc;
using InvoiceService.Repositorys;
namespace InvoiceService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IConfiguration _config;
        public InvoiceController(ILogger<InvoiceController> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }
         [HttpPost("newInvoice")]
         public async Task<IActionResult> CreateInvoice()
         {


            return Ok();
         }

         [HttpPost("sendInvoice")]
         public async Task<IActionResult> SendInvoice()
         {
            return Ok();
        }
         [HttpPut("updateInvoice")]
         public async Task<IActionResult> UpdateInvoice()
         {
            return Ok();
        }
         [HttpDelete("deleteInvoice")]
         public async Task<IActionResult> DeleteInvoice()
         {
            return Ok();
        }
        
    }
}
