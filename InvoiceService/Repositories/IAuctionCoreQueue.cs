using InvoiceService.Models;

namespace InvoiceService.Repositories
{
    public interface IAuctionCoreQueue
    {
        void Add(MailModel mail);
    }
}
