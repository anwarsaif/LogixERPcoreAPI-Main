using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using EInvoiceKSADemo.Helpers.Zatca.Models;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace EInvoiceKSADemo.Helpers.Zatca.Services
{
    public class QrGenerator : IQrGenerator
    {
        public string Generate(string sellerName, string vatRegNumber, string timeStamp, string invoiceTotal, string vatTotal, string hashedXml, byte[] publicKey, string digitalSignature, bool isSimplified, byte[] certificateSignature)
        {
            List<byte> tlvList = new List<byte>();
            tlvList.AddRange(getTLV(1, sellerName));
            tlvList.AddRange(getTLV(2, vatRegNumber));
            tlvList.AddRange(getTLV(3, timeStamp));
            tlvList.AddRange(getTLV(4, invoiceTotal));
            tlvList.AddRange(getTLV(5, vatTotal));
            tlvList.AddRange(getTLV(6, hashedXml));
            tlvList.AddRange(getTLV(7, digitalSignature));
            tlvList.AddRange(getTLV(8, publicKey));


            if (isSimplified)
            {
                tlvList.AddRange(getTLV(9, certificateSignature));
            }
            return ZatcaUtility.ToBase64Encode(tlvList.ToArray());
        }

        private byte[] getTLV(int tag, string value)
        {
            return ZatcaUtility.WriteTlv(tag, Encoding.UTF8.GetBytes(value)).ToArray();
        }
        private byte[] getTLV(int tag, byte[] value)
        {
            return ZatcaUtility.WriteTlv(tag, value).ToArray();
        }
    }
}
