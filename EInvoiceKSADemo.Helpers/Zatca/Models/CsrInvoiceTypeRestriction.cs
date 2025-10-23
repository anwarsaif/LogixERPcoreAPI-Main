/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Models
{
    public class CsrInvoiceTypeRestriction
    {
        public bool StandardAllowed { get; set; }
        public bool SimplifiedAllowed { get; set; }
        public string VatRegNumber { get; set; }
    }
}
