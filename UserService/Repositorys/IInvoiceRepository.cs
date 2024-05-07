using InvoiceService.Models;

namespace InvoiceService.Repository
{
    public interface IInvoiceRepository
    {
        IEnumerable<InvoiceModel> GetAll();

        InvoiceModel GetById(int id);

        void CreateInvoice (InvoiceModel invoice);
        void DeleteInvoice (int id);

        InvoiceModel UpdateInvoice(InvoiceModel invoice);

        void ValidateInvoice(int id);

        void SendInvoice (InvoiceModel invoice , string mailQueue);

    }
}
