using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Interfaces
{
    public interface IZatcaCredentials
    {
        string UserName { get; }
        string Password { get; }
    }
}
