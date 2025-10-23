using System.Globalization;
using TextToQRImagePackage;
namespace Logix.MVC.Helpers
{
    public static class QRHelper
    {
        public static string GenerateQRforZATCA(string savePath, long transactionId, string companyName, string vatNumber, decimal totalWithoutVat, decimal discount, decimal vatAmount, string tDate, string code)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string codeText = "";
                decimal total = totalWithoutVat - discount + vatAmount;
                ZATCA objZATCo = new ZATCA(companyName, vatNumber, DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"), (double)total, (double)vatAmount);
                codeText = objZATCo.ToBase64();

                string qrName = transactionId.ToString();
                string ext = "jpg";
                //string currentPath = Directory.GetCurrentDirectory();
                string folderName = savePath;// Path.Combine(currentPath, "Files", "QRCode", "Sales", "ZATCA");
                //string qrCodePath = Path.Combine(folderName, $"{qrName}.{ext}");
                var qrModel = new QRModel(objZATCo.ToBase64(), folderName, qrName, ext: ext);
                var res = QRGenerator.GenerateImage(qrModel);
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GenerateQRforInvoice(string savePath, long transactionId, string companyName, string vatNumber, decimal totalWithoutVat, decimal discount, decimal vatAmount, string tDate, string code)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string codeText = "";
                decimal total = totalWithoutVat - discount + vatAmount;
                codeText = "المورد: " + companyName + "\n" +
                           "رقم تسجيل ضريبة القيمة المضافة للمورد: " + vatNumber + "\n" +
                           "تاريخ ووقت الفاتورة: " + tDate + " - " + DateTime.Now.ToString("hh:mm:ss") + "\n" +
                           "إجمالي ضريبة القيمة المضافة: " + vatAmount + "\n" +
                           "إجمالي الفاتورة مع ضريبة القيمة المضافة: " + total;

                string qrName = transactionId.ToString();
                string ext = "jpg";
                //string currentPath = Directory.GetCurrentDirectory();
                string folderName = savePath;// Path.Combine(currentPath, "Files", "QRCode", "Sales", "Invoice");
                //string qrCodePath = Path.Combine(folderName, $"{qrName}.{ext}");
                var qrModel = new QRModel(codeText, folderName, qrName, ext: ext);
                var res = QRGenerator.GenerateImage(qrModel);
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GenerateQR(string savePath, string qrName, string data, string ext= "jpg")
        { 
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                var qrModel = new QRModel(data, savePath, qrName, ext: ext);
                var res = QRGenerator.GenerateImage(qrModel);
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}