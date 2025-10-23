using EInvoiceKSADemo.Helpers.Zatca.Interfaces;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EInvoiceKSADemo.Helpers.Zatca.Models;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Pkcs;
using System.Collections;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaCsrGenerator : IZatcaCsrGenerator
    {
        public ZatcaCsrResult GenerateCsr(InputZatcaCsr csrGenerationDto)
        {
            try
            {
                AsymmetricCipherKeyPair keyPair = GenerateKeyPair();

                var stepResult2 = GenerateCertificate(csrGenerationDto, keyPair);
                var stepResult3 = GeneratePrivateKey(keyPair);

                return new ZatcaCsrResult
                {
                    Success = true,
                    Csr = stepResult2,
                    PrivateKey = stepResult3,
                };
            }
            catch (Exception ex)
            {
                return new ZatcaCsrResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        private string GenerateCertificate(InputZatcaCsr dto, AsymmetricCipherKeyPair keyPair)
        {
            string resultedValue = CreateCertificate(dto, keyPair);
            return resultedValue;
        }

        private string GeneratePrivateKey(AsymmetricCipherKeyPair keyPair)
        {
            string privateKey = GetPrivateKey(keyPair);
            return privateKey;
        }

        private string GetPrivateKey(AsymmetricCipherKeyPair keys)
        {
            StringWriter stringWriter = new StringWriter();
            PemWriter val = new PemWriter(stringWriter);
            val.WriteObject(keys.Private);
            ((PemWriter)val).Writer.Flush();
            string text = stringWriter.ToString();
            return text;
        }

        private AsymmetricCipherKeyPair GenerateKeyPair()
        {
            ECKeyPairGenerator val = new ECKeyPairGenerator();
            ECKeyGenerationParameters val2 = new ECKeyGenerationParameters(SecObjectIdentifiers.SecP256k1, new SecureRandom());
            val.Init((KeyGenerationParameters)(object)val2);
            return val.GenerateKeyPair();
        }

        private string CreateCertificate(InputZatcaCsr model, AsymmetricCipherKeyPair ecKeyPair)
        {
            X509Name val = CreateCertificateSubjectName(model);
            X509Name san = CreateCertificateOtherAttributes(model);
            DerSet val2 = X509ExtensionsGenerator(san, model.IsProduction);
            Pkcs10CertificationRequest val3 = new Pkcs10CertificationRequest("SHA256withECDSA", val, ecKeyPair.Public, (Asn1Set)(object)val2, ecKeyPair.Private);
            StringBuilder stringBuilder = new StringBuilder();
            PemWriter val4 = new PemWriter((TextWriter)new StringWriter(stringBuilder));
            val4.WriteObject((object)val3);
            ((PemWriter)val4).Writer.Flush();
            string text = stringBuilder.ToString();
            return text;
        }

        private X509Name CreateCertificateOtherAttributes(InputZatcaCsr csrGenerationDto)
        {
            List<DerObjectIdentifier> obj = new List<DerObjectIdentifier>
        {
            X509Name.Surname,
            X509Name.UID,
            X509Name.T,
            new DerObjectIdentifier("2.5.4.26"),
            X509Name.BusinessCategory
        };
            List<string> list = new List<string> { csrGenerationDto.SerialNumber, csrGenerationDto.VATNumber, csrGenerationDto.InvoiceType, csrGenerationDto.LocationAddress, csrGenerationDto.BusinessCategory };
            return new X509Name((IList)obj.ToArray(), (IList)list.ToArray());
        }

        private X509Name CreateCertificateSubjectName(InputZatcaCsr csrGenerationDto)
        {
            List<DerObjectIdentifier> obj = new List<DerObjectIdentifier> {
            X509Name.E,
            X509Name.C,
            X509Name.OU,
            X509Name.O,
            X509Name.CN };
            List<string> list = new List<string> { csrGenerationDto.Email, csrGenerationDto.CountryName, csrGenerationDto.BranchName, csrGenerationDto.OrganizationName, csrGenerationDto.CommonName };
            return new X509Name((IList)obj.ToArray(), (IList)list.ToArray());
        }

        private DerSet X509ExtensionsGenerator(X509Name san, bool isProduction)
        {

            string certificateTemplateName = isProduction ? "ZATCA-Code-Signing" : "PREZATCA-Code-Signing";
            X509Extensions val = new X509Extensions((IDictionary)new Dictionary<DerObjectIdentifier, X509Extension>
        {
            {
                new DerObjectIdentifier("1.3.6.1.4.1.311.20.2"),
                new X509Extension(false, (Asn1OctetString)new DerOctetString((Asn1Encodable)new DerPrintableString(certificateTemplateName)))
            },
            {
                X509Extensions.SubjectAlternativeName,
                new X509Extension(false, (Asn1OctetString)new DerOctetString((Asn1Encodable)new DerSequence((Asn1Encodable)new DerTaggedObject(4, (Asn1Encodable)(object)san))))
            }
        });
            return new DerSet((Asn1Encodable)new AttributePkcs(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest, (Asn1Set)new DerSet((Asn1Encodable)(object)val)));
        }
    }
}
