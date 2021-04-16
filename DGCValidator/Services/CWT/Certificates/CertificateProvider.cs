using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace DGCValidator.Services.CWT
{
    /**
     * An implementation for finding certificates that may be used to verify a HCERT (see {@link HCertVerifier}).
     * 
     * @author Henrik Bengtsson (henrik@sondaica.se)
     * @author Martin Lindström (martin@idsec.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class CertificateProvider : ICertificateProvider
    {
        private String publicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEFzWPFb4pCXJONcqkz+MsHoHCrEw7FTFpRwDj7w380LPp9U//ddpWvUkMOK888mB6qAviPllcMJJFXAzoo2+gfg==";
        private Jwks Jwks { get; }


        public CertificateProvider()
        {
            Jwks = Jwks.FromJson(File.ReadAllText(@"jwks.json"));
        }

        public List<ECPublicKeyParameters> GetCertificates(string country, byte[] kid)
        {
            List<ECPublicKeyParameters> publicKeys = new List<ECPublicKeyParameters>();

            foreach (Key key in Jwks.Keys)
            {
                string kidStr = Convert.ToBase64String(kid)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", ""); ;
                if (kid == null || key.Kid == null || key.Kid.Equals(kidStr))
                {
                    X9ECParameters x9 = ECNamedCurveTable.GetByName(key.Crv);
                    ECPoint point = x9.Curve.CreatePoint(Base64UrlDecodeToBigInt(key.X), Base64UrlDecodeToBigInt(key.Y));

                    ECDomainParameters dParams = new ECDomainParameters(x9);
                    ECPublicKeyParameters pubKey = new ECPublicKeyParameters(point, dParams);
                    publicKeys.Add(pubKey);
                }
            }
            ECPublicKeyParameters bpubKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            publicKeys.Add(bpubKey);
            return publicKeys;
        }

        private BigInteger Base64UrlDecodeToBigInt(String value)
        {
            value = value.Replace('-', '+'); // 62nd char of encoding
            value = value.Replace('_', '/'); // 63rd char of encoding
            switch (value.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: value += "=="; break; // Two pad chars
                case 3: value += "="; break; // One pad char
                default:
                    throw new Exception("Illegal base64url string!");
            }
            return new BigInteger(1,Convert.FromBase64String(value)); // Standard base64 decoder
        }
    }
}
