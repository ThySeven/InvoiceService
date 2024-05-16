namespace InvoiceService.Models
{
    public class ParcelHtmlModel
    {
        private string htmlContent;

        public ParcelHtmlModel(ParcelModel parcel, string parcelUrl)
        {
            htmlContent = @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>Parcel Details</title>
<style>
  body {
    font-family: Arial, sans-serif;
    max-width: 800px;
    margin: 40px auto;
    padding: 20px;
    border: 1px solid #eee;
    border-radius: 10px;
  }
  .parcel-details {
    margin-bottom: 20px;
  }
  .parcel-details h2 {
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
  .summary {
    text-align: right;
    margin-top: 20px;
  }
  .value-cell {
    text-align: right;
  }
  .pay-now-btn {
    display: block;
    width: 200px;
    margin: 20px auto;
    padding: 15px;
    background-color: #28a745;
    color: white;
    text-align: center;
    text-decoration: none;
    border-radius: 10px;
  }
  .pay-now-btn:hover {
    background-color: #218838;
  }
</style>
</head>
<body>
" + @$"
<div class=""parcel-details"">
  <h2>Parcel Reference: {parcel.Reference}</h2>
  <p><strong>Weight:</strong> {parcel.Weight} kg</p>
  <p><strong>Dimensions:</strong> {parcel.Length} x {parcel.Width} x {parcel.Height} cm ({parcel.WeightCategory})</p>
  <p><strong>Comment:</strong> {parcel.Comment}</p>
</div>

<a href=""{parcelUrl}"" class=""pay-now-btn"">Track here</a>


</body>
</html>";
        }

        public string HtmlContent { get => htmlContent; set => htmlContent = value; }
    }
}
