/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Zatca.Helpers;
using EInvoiceKSADemo.Helpers.Zatca.Interfaces;
using EInvoiceKSADemo.Helpers.Zatca.Models;
using EInvoiceKSADemo.Helpers.Zatca.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca
{
    public static class ZatcaInvoiceSetup
    {
        public static IServiceCollection AddZatcaServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IZatcaInvoiceSigner, ZatcaInvoiceSigner>();

            services.AddTransient<IQrGenerator, QrGenerator>();
            services.AddTransient<IInvoiceSigner, InvoiceSigner>();
            services.AddTransient<IInvoiceHashingGenerator, InvoiceHashingGenerator>();

            services.AddTransient<ICertificateConfiguration, CertificateConfiguration>();

            services.AddTransient<IXmlInvoiceGenerator, XmlInvoiceGenerator>();
            services.AddTransient<IZatcaAPICaller, ZatcaAPICaller>();
            services.AddTransient<IZatcaReporter, ZatcaReporter>();
           // services.AddTransient<IZatcaCredentials, ZatcaCredentials>();

            services.AddTransient<ICsrReader, ZatcaCsrReader>();
            services.AddTransient<IZatcaCSIDIssuer, ZatcaCSIDIssuer>();


            services.AddTransient<IInvoiceInfoGenerator, InvoiceInfoGenerator>();

            services.AddTransient<IZatcaCsrGenerator, ZatcaCsrGenerator>();

            // services.Configure<AppSettings>(config.GetSection("AppSettings"));

            return services;
        }
    }
}