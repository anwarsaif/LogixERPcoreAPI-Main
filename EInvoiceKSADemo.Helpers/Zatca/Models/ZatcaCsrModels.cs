using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Models
{
    public class InputZatcaCsr
    {
        public bool IsProduction { get; set; }
        public string SerialNumber { get; set; }
        public string BusinessCategory { get; set; }
        public string InvoiceType { get; set; }
        public string LocationAddress { get; set; }
        public string VATNumber { get; set; }
        public string CountryName { get; set; }
        public string BranchName { get; set; }
        public string OrganizationName { get; set; }
        public string Email { get; set; }
        public string CommonName { get; set; }
    }

    public class ZatcaCsrResult
    {
        public bool Success { get; set; }
        public string Csr { get; set; }
        public string PrivateKey { get; set; }
        public string ErrorMessage { get; set; }
    }
}
