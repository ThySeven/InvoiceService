using System;

namespace InvoiceService.Models
{
    public class InvoiceModel
    {

        public InvoiceModel() 
        {

        }

        private string id = Guid.NewGuid().ToString();
        private bool paidStatus = false;
        private double? price;
        private string description;
        private string address;
        private string email;
        private DateTime createdAt = DateTime.Now;

        public string Id { get => id; set => id = value; }
        public bool PaidStatus { get => paidStatus; set => paidStatus = value; }
        public double? Price { get => price; set => price = value; }
        public string? Description { get => description; set => description = value; }
        public string? Address { get => address; set => address = value; }
        public string? Email { get => email; set => email = value; }
        public DateTime CreatedAt { get => createdAt; }
    }
}
