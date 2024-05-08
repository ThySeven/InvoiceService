using InvoiceService.Models;
using Microsoft.AspNetCore.Mvc;
using InvoiceService.Repositorys;
using InvoiceService.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using NLog;
using NLog.Web;
using System.Text.Json;

namespace InvoiceService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceController> _logger;
        private readonly IConfiguration _config;

        public InvoiceController(ILogger<InvoiceController> logger, IConfiguration config, IInvoiceRepository invoiceRepo)
        {
            _config = config;
            _logger = logger;
            _invoiceRepository = invoiceRepo;

            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var _ipaddr = ips.First().MapToIPv4().ToString();
            _logger.LogDebug(1, $"XYZ Service responding from {_ipaddr}");
        }

        [HttpPost("create")]
        public IActionResult CreateInvoice(InvoiceModel invoice)
        {
            try
            {
                _invoiceRepository.CreateInvoice(invoice);
                _logger.LogError($"{invoice.Id} created");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{JsonSerializer.Serialize(invoice)} failed to created: {ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetInvoiceById(string id)
        {
            try
            {
                _invoiceRepository.GetById(id);
                _logger.LogError($"{id} created");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to get by id: {JsonSerializer.Serialize(id)} error: {ex}");
                return BadRequest("Bad request");
            }
        }
        [HttpGet("getAll")]
        public IActionResult GetAllInvoice()
        {
            try
            {
                var invoice = _invoiceRepository.GetAll();
                _logger.LogError($"{invoice} created");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to get: {ex}");
                return BadRequest("Bad request");
            }
        }


        [HttpGet("validate/{id}")]
        public IActionResult ValidateInvoice(string id)
        {
            try
            {
                _invoiceRepository.ValidateInvoice(id);
                _logger.LogError($"{id} created");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to validate Invoice {id}: {ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpPost("send")]
        public IActionResult SendInvoice(InvoiceModel invoice)
        {
            try
            {
                _invoiceRepository.SendInvoice(invoice);
                _logger.LogInformation($"{invoice.Id} sent");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{JsonSerializer.Serialize(invoice)} failed to created: {ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateInvoice(InvoiceModel invoice)
        {
            try
            {
                var result = _invoiceRepository.UpdateInvoice(invoice);
                _logger.LogInformation($"{invoice.Id} updated");
                return Ok(result); // Assuming UpdateInvoice doesn't return anything
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"{JsonSerializer.Serialize(invoice)} failed to update: {ex}");
                return BadRequest("Bad Request");
            }

        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteInvoice(string id)
        {
            try
            {
                _invoiceRepository.DeleteInvoice(id);
                return Ok(); // Assuming UpdateInvoice doesn't return anything
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Failed to delete id {id}: {ex.Message}");
                return BadRequest("Bad Request");
            }
        }
        [HttpPost("createPaymentLink")]
        public IActionResult CreatePaymentLink(PaymentModel payment)
        {
            try
            {
                _invoiceRepository.CreatePaymentLink(payment);
                _logger.LogInformation($"{payment} sent");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{JsonSerializer.Serialize(payment)} failed to send payment link: {ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpPost("createPaymentLink")]
        public IActionResult SendParcelInformation(ParcelModel parcel)
        {
            try
            {
                _invoiceRepository.SendParcelInformation(parcel);
                _logger.LogInformation($"{parcel} sent");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{JsonSerializer.Serialize(parcel)} failed to send parcel: {ex}");
                return BadRequest("Bad request");
            }
        }

    }
}
