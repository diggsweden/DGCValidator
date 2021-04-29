using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services.DGC.ValueSet;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace DGCValidator.Services
{
    /**
     * An implementation for finding certificates that may be used to verify a HCERT (see {@link HCertVerifier}).
     * 
     * @author Henrik Bengtsson (henrik@sondaica.se)
     * @author Martin Lindström (martin@idsec.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class CertificateManager : ICertificateProvider
    {
        private String _publicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEFzWPFb4pCXJONcqkz+MsHoHCrEw7FTFpRwDj7w380LPp9U//ddpWvUkMOK888mB6qAviPllcMJJFXAzoo2+gfg==";
        private String _publicKey2 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAphzVQe/ih0K3foSww+KGvqY+DF54mZKHK0g8lv0ZW7tbTCvbtcb5D+9IecEnwXMGtZA+3VTHP6ywCOR660SAhKpnMkfm7+at/OUcZQs/9QXbA9vZ3Ek5J3I6U/sOfPTuldrHpdTsOtqvIT4XQHZlG5Js4m21Rje/T5UM3OTo/cMHVDKFuHbyKbZvdCS4aqkp4cVGSSepGYjVC0fqX3WUu4OYyANrDRFq/8vvI/GwPzG40VbN5IL0psXb+vpGig2OATRt6Q2PeH2WEYf/hq7aXfn10cHwT47XARXJIXKwoDCnykHB/UNfLirSuStbVWSEDJoIgCj+zK8VTtwepccOqwIDAQAB";
        private readonly IRestService _restService;
        public Dictionary<string, ValueSet> ValueSets { get; private set; }
        public DSC_TL TrustList { get; private set; }
        private readonly string FileName = "Trusts/DscTrustList.json";

        public CertificateManager(IRestService service)
        {
            _restService = service;
        }
        public async void RefreshTrustListAsync()
        {
            DSC_TL trustList = await _restService.RefreshTrustListAsync();
            if (trustList != null && trustList.DscTrustList != null && trustList.DscTrustList.Count > 0)
            {
                TrustList = trustList;
                await File.WriteAllTextAsync(FileName, DSC_TLSerialize.ToJson(trustList));
            }
        }

        public async void RefreshValueSetAsync()
        {
            Dictionary<string, ValueSet> valueSets = await _restService.RefreshValueSetAsync();
            if (valueSets != null && valueSets.Keys != null && valueSets.Keys.Count > 0)
            {
                ValueSets = valueSets;
            }
        }

        private void LoadCertificates()
        {
            if (TrustList == null && File.Exists(FileName))
            {
                DSC_TL trustList = DSC_TL.FromJson(File.ReadAllText(FileName));
                TrustList = trustList;
            }
            // If trustlist is not set or it´s older than 24 hours refresh it
            if( TrustList == null || (TrustList.Iat+86400)< GetSecondsFromEpoc() )
            {
                RefreshTrustListAsync();
            }
        }

        private long GetSecondsFromEpoc()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public List<AsymmetricKeyParameter> GetCertificates(string country, byte[] kid)
        {
            LoadCertificates();
            List<AsymmetricKeyParameter> publicKeys = new List<AsymmetricKeyParameter>();

            // No TrustList means no keys to match with
            if( TrustList == null)
            {
                return publicKeys;
            }

            List<DscTrust> trusts=new List<DscTrust>();
            if( country != null)
            {
                DscTrust dscTrust = TrustList.DscTrustList.GetValueOrDefault(country);
                if( dscTrust != null)
                {
                    trusts.Add(dscTrust);
                }
            }
            else
            {
                trusts.AddRange(TrustList.DscTrustList.Values);
            }

            foreach( DscTrust trust in trusts)
            {
                foreach (Key key in trust.Keys)
                {
                    string kidStr = Convert.ToBase64String(kid)
                        .Replace('+', '-')
                        .Replace('/', '_')
                        .Replace("=", ""); ;
                    if (kid == null || key.Kid == null || key.Kid.Equals(kidStr))
                    {
                        if( key.Kty.Equals("EC"))
                        {
                            X9ECParameters x9 = ECNamedCurveTable.GetByName(key.Crv);
                            ECPoint point = x9.Curve.CreatePoint(Base64UrlDecodeToBigInt(key.X), Base64UrlDecodeToBigInt(key.Y));

                            ECDomainParameters dParams = new ECDomainParameters(x9);
                            ECPublicKeyParameters pubKey = new ECPublicKeyParameters(point, dParams);
                            publicKeys.Add(pubKey);
                        }
                        else if( key.Kty.Equals("RSA"))
                        {
                            RsaKeyParameters pubKey = new RsaKeyParameters(false, Base64UrlDecodeToBigInt(key.N), Base64UrlDecodeToBigInt(key.E));
                            publicKeys.Add(pubKey);
                        }
                    }
                }
            }
            ECPublicKeyParameters bpubKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(_publicKey));
            publicKeys.Add(bpubKey);
            AsymmetricKeyParameter dkKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(_publicKey2));
            publicKeys.Add(dkKey);
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
