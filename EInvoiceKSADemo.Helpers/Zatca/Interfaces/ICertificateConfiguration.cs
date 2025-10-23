/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Zatca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca
{
    public interface ICertificateConfiguration
    {
        CertificateDetails GetCertificateDetails(string Facility_ID, string Branch_ID);
        CertificateDetails GetCertificateDetails_Simulation(string Facility_ID, string Branch_ID);

        Task SaveCsrAndPK(InputCsrModel model);

        Task UpdateCertificate(CSIDResultModel model);

    }
}
