using System.Collections.Generic;
using InvoiceService.Models;

namespace InvoiceService.Repository
{
    public interface IInvoiceRepository
    {
        IEnumerable<InvoiceModel> GetAll();

        InvoiceModel GetById(string id);

        void CreateInvoice (InvoiceModel invoice);
        void DeleteInvoice (string id);

        InvoiceModel UpdateInvoice(InvoiceModel invoice);

        void ValidateInvoice(string id);

        void SendInvoice (InvoiceModel invoice);

        string CreatePaymentLink(PaymentModel payment);

        string SendParcelInformation(ParcelModel parcel);
    }
}
