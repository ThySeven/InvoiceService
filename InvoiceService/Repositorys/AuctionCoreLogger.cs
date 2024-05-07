using NLog;
using ILogger = NLog.ILogger;

namespace InvoiceService.Repositorys
{
    public class AuctionCoreLogger
    {
        public static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}
