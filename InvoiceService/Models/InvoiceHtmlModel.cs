namespace InvoiceService.Models
{
    public class InvoiceHtmlModel
    {
        private string htmlContent;

        public InvoiceHtmlModel(InvoiceModel invoice)
        {
        


            htmlContent = @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>Invoice</title>
<style>
  body {
    font-family: Arial, sans-serif;
    max-width: 800px;
    margin: 40px auto;
    padding: 20px;
    border: 1px solid #eee;
    border-radius: 10px;
  }
  .company-address, .invoice-details {
    margin-bottom: 20px;
  }
  .company-address h2, .invoice-details h2 {
    margin: 0 0 10px 0;
  }
  table {
    width: 100%;
    border-collapse: collapse;
  }
  table, th, td {
    border: 1px solid black;
  }
  th, td {
    padding: 10px;
    text-align: left;
  }
  .signature {
    margin-top: 40px;
  }
  .signature img {
    width: 150px;
  }
  .total-amount {
    text-align: right;
    margin-top: 20px;
  }
</style>
</head>
<body>

<div class=""company-address"">
  <h2>Grøn & Olsen</h2>
  <p>Sønderhøj 30, 8260 Viby J</p>
  <p>Phone: 88 88 88 88</p>
  <p>Email: gronogolsen@gmail.com</p>
</div>

<div class=""invoice-details"">
  <h2>Invoice #12345</h2>
  <p>Invoice Date: 2024-05-07</p>
  <p>Due Date: 2024-05-09</p>
</div>

<table>
  <thead>
    <tr>
      <th>Description</th>
      <th>Quantity</th>
      <th>Price</th>
      <th>Total</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Havetraktor</td>
      <td>2</td>
      <td>$100.00</td>
      <td>$200.00</td>
    </tr>
    <tr>
      <td>Bagger 293</td>
      <td>5</td>
      <td>$20.00</td>
      <td>$100.00</td>
    </tr>
    <tr>
      <td colspan=""3"" style=""text-align:right;""><strong>Grand Total</strong></td>
      <td><strong>$300.00</strong></td>
    </tr>
  </tbody>
</table>

<div class=""total-amount"">
  <h2>Total Due: $300.00</h2>
</div>

<div class=""signature"">
  <p>Authorized Signature</p>
  <img src=""https://raw.githubusercontent.com/ThySeven/MailService/main/Images/2024_05_08_0lp_Kleki.png"" alt=""Signature"">
  <p>Kell Olsen, CEO</p>
</div>

</body>
</html>";
        }

        public string HtmlContent { get => htmlContent; set => htmlContent = value; }
    }
}
