namespace InvoiceService.Models

{
    public class PaymentModel 
    {
        public double price { get; set; } 
        public string CurrencyCode { get; set; }
        
        public string InvoiceNumber { get; set; }
        
        public string Reference { get; set; }
        
        public DateTime InvoiceDate { get; set; }
        
        public string Note { get; set; }
        
        public string Term { get; set; }

        public string Memo { get; set; }
    }
}