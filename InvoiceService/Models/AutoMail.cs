namespace InvoiceService.Models
{
    public class AutoMail
    {
        private string? senderMail;
        private string? receiverMail;
        private string? header;
        private string? content;
        private DateTime dateTime;
        private InvoiceModel? model;

        public string? SenderMail { get => senderMail; set => senderMail = value; }
        public string? ReceiverMail { get => receiverMail; set => receiverMail = value; }

        public string? Header { get => header; set => header = value; }

        public string? Content { get => content; set => content = value; }

        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public InvoiceModel? Model { get => model; set => model = value; }
    }
}
