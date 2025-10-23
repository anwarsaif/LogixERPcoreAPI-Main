using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
namespace Logix.MVC.Helpers
{
    public static class BarCodeHelper
    {
        public static string GenerateBarCode(string code, string savePath, int width = 300, int height = 150)
        {
            try
            {
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(savePath))
                    return "";

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                string barCodeUrl = ""; // to return the path of barcode image
                var barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.CODE_128,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 0
                    }
                };

                var pixelData = barcodeWriterPixelData.Write(code);
                using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                        try
                        {
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }

                        string fileName = $"{code}.png";
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", savePath, fileName);

                        // Ensure the directory exists
                        string directory = Path.GetDirectoryName(filePath) ?? "";
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the barcode image
                        bitmap.Save(filePath, ImageFormat.Png);

                        // Return the path of the saved image
                        barCodeUrl = savePath +"/"+ fileName;
                    }
                }

                return barCodeUrl;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}