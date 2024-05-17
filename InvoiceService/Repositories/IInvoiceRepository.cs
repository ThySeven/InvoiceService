using System.Collections.Generic;
using InvoiceService.Models;

namespace InvoiceService.Repository
{
    public interface IInvoiceRepository
    {
        IEnumerable<InvoiceModel> GetAll();

        InvoiceModel GetById(string id);

        Task CreateInvoice (InvoiceModel invoice);
        void DeleteInvoice (string id);

        InvoiceModel UpdateInvoice(InvoiceModel invoice);

        void ValidateInvoice(string id);

        Task SendInvoice (InvoiceModel invoice);

        Task<string> CreatePaymentLink(PaymentModel payment);

        string SendParcelInformation(ParcelModel parcel);
    }
}
