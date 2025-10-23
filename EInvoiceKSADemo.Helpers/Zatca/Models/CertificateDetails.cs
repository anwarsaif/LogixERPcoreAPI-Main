using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Models
{
    public class InputCsrModel
    {
        public string CSR { get; set; }
        public string PrivateKey { get; set; }
    }

    public class CertificateDetails
    {
        public string UserName { get; set; }
        public string Secret { get; set; }

        public string Certificate { get; set; }

        public string CSR { get; set; }

        public string PrivateKey { get; set; }
        public string Branch_id { get; set; }
        public string OU { get; set; }
        public string O { get; set; }
        public string CN { get; set; }
        public string SN { get; set; }
        public string UID { get; set; }
        public string SystemVersion { get; set; }
        public string Guid { get; set; }

        public DateTime StartedDate { get; set; }

        public DateTime ExpiredDate { get; set; }
    }
}
