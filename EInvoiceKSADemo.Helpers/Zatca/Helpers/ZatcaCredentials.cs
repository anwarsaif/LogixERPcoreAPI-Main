using EInvoiceKSADemo.Helpers.Zatca.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaCredentials : IZatcaCredentials
    {
        public string UserName { get => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException();}
    }
}
