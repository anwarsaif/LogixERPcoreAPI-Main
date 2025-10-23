/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Zatca.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class CertificateConfiguration : ICertificateConfiguration
    {
        private readonly IConfiguration configuration;

        public CertificateConfiguration()
        {
            
        }
        public CertificateConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public CertificateDetails GetCertificateDetails(string Facility_ID, string Branch_ID)
        {
            var certificateDetails = new CertificateDetails();
            certificateDetails = ACC_CertificateSettings(Facility_ID, Branch_ID);

            return certificateDetails;
        }


        public CertificateDetails GetCertificateDetails_Simulation(string Facility_ID, string Branch_ID)
        {
            var certificateDetails = new CertificateDetails();
            certificateDetails = ACC_CertificateSettings_Simulation(Facility_ID, Branch_ID);

            return certificateDetails;
        }
        public async Task SaveCsrAndPK(InputCsrModel model)
        {
            var cert = new CertificateDetails
            {
                CSR = model.CSR,
                PrivateKey = model.PrivateKey,
            };

            //Save Cert to Database
        }

        public async Task UpdateCertificate(CSIDResultModel model, string Facility_ID, string Branch_ID)
        {
            var cert = GetCertificateDetails(Facility_ID, Branch_ID);
            if (cert != null)
            {
                cert.UserName = model.Certificate;
                cert.Certificate = model.Certificate;
                cert.Secret = model.Secret;
                cert.ExpiredDate = model.ExpiredDate;
                cert.StartedDate = model.StartedDate;
            }

            // Update Certificate Details in Database
        }
        private CertificateDetails ACC_CertificateSettings(string Facility_ID, string Branch_ID)
        {
            string ConnectionStrin = GetConnectionString();
            DataTable dataTable = new DataTable();
            CertificateDetails certificateDetails = new CertificateDetails();
            using (SqlConnection objCnn = new SqlConnection(ConnectionStrin))
            {
                string sqlQuery = "SELECT * FROM ACC_CertificateSettings WHERE IsDeleted = 0 AND Facility_ID = @Facility_ID AND Branch_ID = @Branch_ID";

                SqlCommand command = new SqlCommand(sqlQuery, objCnn);
                command.Parameters.AddWithValue("@Facility_ID", Facility_ID);
                command.Parameters.AddWithValue("@Branch_ID", Branch_ID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {

                    certificateDetails.UserName = row["Certificate"].ToString();
                    certificateDetails.Secret = row["Secret"].ToString();
                    certificateDetails.Certificate = row["Certificate"].ToString();
                    certificateDetails.CSR = row["CSR"].ToString();
                    certificateDetails.PrivateKey = row["PrivateKey"].ToString();
                    certificateDetails.StartedDate = Convert.ToDateTime(row["StartedDate"]);
                    certificateDetails.ExpiredDate = Convert.ToDateTime(row["ExpiredDate"]);
                }
            }
            return certificateDetails;
        }


        private CertificateDetails ACC_CertificateSettings_Simulation(string Facility_ID, string Branch_ID)
        {
            string ConnectionStrin = GetConnectionString();
            DataTable dataTable = new DataTable();
            CertificateDetails certificateDetails = new CertificateDetails();
            using (SqlConnection objCnn = new SqlConnection(ConnectionStrin))
            {
                string sqlQuery = "SELECT * FROM ACC_CertificateSettings_Simulation WHERE IsDeleted = 0 AND Facility_ID = @Facility_ID AND Branch_ID = @Branch_ID";

                SqlCommand command = new SqlCommand(sqlQuery, objCnn);
                command.Parameters.AddWithValue("@Facility_ID", Facility_ID);
                command.Parameters.AddWithValue("@Branch_ID", Branch_ID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {

                    certificateDetails.UserName = row["Certificate"].ToString();
                    certificateDetails.Secret = row["Secret"].ToString();
                    certificateDetails.Certificate = row["Certificate"].ToString();
                    certificateDetails.CSR = row["CSR"].ToString();
                    certificateDetails.PrivateKey = row["PrivateKey"].ToString();
                    certificateDetails.StartedDate = Convert.ToDateTime(row["StartedDate"]);
                    certificateDetails.ExpiredDate = Convert.ToDateTime(row["ExpiredDate"]);
                }
            }
            return certificateDetails;
        }
        public  string GetConnectionString()
        {
            string connectionString = configuration.GetConnectionString("LogixLocal");
            return connectionString;
        }



        public Task UpdateCertificate(CSIDResultModel model)
        {
            return null;
        }
    }
}
