using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DGCValidator.Services.CWT;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services.DGC.ValueSet;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace DGCValidator.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public async Task<DSC_TL> RefreshTrustListAsync()
        {
            DSC_TL trustList = new DSC_TL();
            Uri uri = new Uri(Constants.GetTrustListUrl());
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();


                    // Verify signature
                    byte[] payload = Verify(content);
                    if( payload != null)
                    {
                        trustList = DSC_TL.FromJson(Encoding.UTF8.GetString(payload));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR " + ex.Message);
            }

            return trustList;
        }

        private byte[] Verify(string content)
        {
            try
            {
                string[] contents = content.Split(".");
                byte[] headerBytes = Base64UrlDecode(contents[0]);
                byte[] payloadBytes = Base64UrlDecode(contents[1]);
                byte[] signatureBytes = Base64UrlDecode(contents[2]);

                DSC_TL_HEADER header = DSC_TL_HEADER.FromJson(Encoding.UTF8.GetString(headerBytes));

                byte[] x5c = Convert.FromBase64String(header.X5C[0]);

                String x5cString = Encoding.UTF8.GetString(x5c);

                X509CertificateParser parser = new X509CertificateParser();
                X509Certificate cert = parser.ReadCertificate(x5c);

                AsymmetricKeyParameter jwsPublicKey = cert.GetPublicKey();

                ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
                signer.Init(false, jwsPublicKey);

                /* Get the bytes to be signed from the string */
                var msgBytes = Encoding.ASCII.GetBytes(contents[0] + "." + contents[1]);

                /* Calculate the signature and see if it matches */
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                byte[] derSignature = ToDerSignature(signatureBytes);
                bool result = signer.VerifySignature(derSignature);
                if (result)
                {
                    return payloadBytes;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR " + ex.Message);
            }
            return null;
        }

        private static byte[] ToDerSignature(byte[] jwsSig)
        {
            int len = jwsSig.Length / 2;
            byte[] r = new byte[len];
            byte[] s = new byte[len];
            Array.Copy(jwsSig, r, len);
            Array.Copy(jwsSig, len, s, 0, len);

            List<byte[]> seq = new List<byte[]>();
            seq.Add(ASN1.ToUnsignedInteger(r));
            seq.Add(ASN1.ToUnsignedInteger(s));

            byte[] derSeq = ASN1.ToSequence(seq);
            return derSeq;
        }

        private byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }


        public async Task<Dictionary<string, string>> RefreshValueSetAsync()
        {
            Dictionary<string, string> valueSets = new Dictionary<string, string>();
            foreach( string valueset in Constants.ValueSets)
            {
                Uri uri = new Uri(string.Format(Constants.GetValueSetBaseUrl(),valueset));
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        valueSets.Add(valueset, content);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR " + ex.Message);
                }
            }

            return valueSets;
        }
    }

    public class Constants
    {
        private static readonly string TestTrustListRestUrl = "https://dgcg-qa.covidbevis.se/tp/trust-list";
        private static readonly string ProductionTrustListRestUrl = "https://dgcg.covidbevis.se/tp/trust-list";

        private static readonly string TestValueSetRestUrl = "https://dgcg-qa.covidbevis.se/tp/valuesets/{0}";
        private static readonly string ProductionValueSetRestUrl = "https://dgcg.covidbevis.se/tp/valuesets/{0}";
        public static readonly List<string> ValueSets;

        public static readonly string Disesase = "disease-agent-targeted.json";
        public static readonly string TestManufacturer = "test-manf.json";
        public static readonly string TestResult = "test-result.json";
        public static readonly string TestType = "test-type.json";
        public static readonly string VaccineManufacturer = "vaccine-mah-manf.json";
        public static readonly string VaccineProduct = "vaccine-medicinal-product.json";
        public static readonly string VaccineProphylaxis = "vaccine-prophylaxis.json";

        static Constants()
        {
            ValueSets = new List<string> { Disesase, TestManufacturer, TestResult, VaccineManufacturer, VaccineProduct, VaccineProphylaxis, TestType };
        }



        public static string GetTrustListUrl()
        {
            if( Xamarin.Essentials.Preferences.Get("ProductionMode", true)){
                return ProductionTrustListRestUrl;
            }
            else
            {
                return TestTrustListRestUrl;
            }
        }
        public static string GetValueSetBaseUrl()
        {
            if (Xamarin.Essentials.Preferences.Get("ProductionMode", true))
            {
                return ProductionValueSetRestUrl;
            }
            else
            {
                return TestValueSetRestUrl;
            }
        }
    }
}
