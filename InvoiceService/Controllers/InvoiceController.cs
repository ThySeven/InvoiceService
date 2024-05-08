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
                PaymentModel payment = new PaymentModel
                {
                    Price = invoice.Price,
                    CurrencyCode = "DKK",
                    InvoiceNumber = invoice.Id,
                    Reference = "Ref987654",
                    InvoiceDate = DateTime.Now,
                    Note = "Payment for software subscription.",
                    Term = "Net 30",
                    Memo = "Paid via online bank transfer."
                };
                _invoiceRepository.CreateInvoice(invoice);
                _logger.LogInformation($"{invoice.Id} created");
                string link = _invoiceRepository.CreatePaymentLink(payment);
                _logger.LogInformation($"Payment link created: {link}");
                return Ok(link); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to create invoice invoice: {ex} # model: {JsonSerializer.Serialize(invoice)} ");
                return BadRequest("Bad request");
            }
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetInvoiceById(string id)
        {
            try
            {
                _invoiceRepository.GetById(id);
                _logger.LogInformation($"{id} created");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to get by id: {id} # {ex}");
                return BadRequest("Bad request");
            }
        }
        [HttpGet("getAll")]
        public IActionResult GetAllInvoice()
        {
            try
            {
                var invoice = _invoiceRepository.GetAll();
                _logger.LogInformation($"Fetched all invoices: {JsonSerializer.Serialize(invoice)}");
                return Ok(); // Assuming CreateInvoice returns the created invoice
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to get: {ex}");
                return BadRequest("Bad request");
            }
        }


        [HttpGet("validate/{id}")]
        public IActionResult ValidateInvoice(string id, ParcelModel parcel)
        {
            try
            {
                _invoiceRepository.ValidateInvoice(id);
                _invoiceRepository.SendParcelInformation(parcel);

                _logger.LogInformation($"Parcel validated: {id}");
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
                _logger.LogInformation($"Invoice sent with id: {invoice.Id}");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to send invoice: {ex} # model: {JsonSerializer.Serialize(invoice)} ");
                return BadRequest("Bad request");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateInvoice(InvoiceModel invoice)
        {
            try
            {
                var result = _invoiceRepository.UpdateInvoice(invoice);
                _logger.LogInformation($"Invoice updated with id: {invoice.Id}");
                return Ok(result); // Assuming UpdateInvoice doesn't return anything
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Failed to update invoice: {ex} # model: {JsonSerializer.Serialize(invoice)} ");
                return BadRequest("Bad Request");
            }

        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteInvoice(string id)
        {
            try
            {
                _invoiceRepository.DeleteInvoice(id);
                _logger.LogInformation($"Invoice deleted with id: {id}");
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
                string link = _invoiceRepository.CreatePaymentLink(payment);
                _logger.LogInformation($"Payment link created: {link}");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to send payment link: {ex} # model: {JsonSerializer.Serialize(payment)} ");
                return BadRequest("Bad request");
            }
        }

        [HttpPost("createParcelInfo")]
        public IActionResult SendParcelInformation(ParcelModel parcel)
        {
            try
            {
                _invoiceRepository.SendParcelInformation(parcel);
                _logger.LogInformation($"Parcelinfo sent: {parcel}");
                return Ok(); // Assuming SendInvoice doesn't return anything
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to send parcel info: {ex} # model: {JsonSerializer.Serialize(parcel)} ");
                return BadRequest("Bad request");
            }
        }

    }
}
