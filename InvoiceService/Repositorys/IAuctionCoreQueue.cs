using InvoiceService.Models;

namespace InvoiceService.Repositorys
{
    public interface IAuctionCoreQueue
    {
        void Add(AutoMail mail);
        void Update(AutoMail mail);
    }
}
