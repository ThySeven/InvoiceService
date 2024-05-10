using NLog;
using ILogger = NLog.ILogger;

namespace InvoiceService.Repositories
{
    public class AuctionCoreLogger
    {
        public static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}
