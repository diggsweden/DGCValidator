using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services.DGC.ValueSet;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Xamarin.Forms;

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
        private readonly IRestService _restService;
        public Dictionary<string, ValueSet> ValueSets { get; private set; }
        public DSC_TL TrustList { get; private set; }
        private readonly string TrustListFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DscTrustList.json");
        private readonly string ValueSetPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public CertificateManager(IRestService service)
        {
            _restService = service;
        }
        public async void RefreshTrustListAsync()
        {
            DSC_TL trustList = await _restService.RefreshTrustListAsync();
            if (trustList != null && trustList.DscTrustList != null && trustList.DscTrustList.Count > 0 && trustList.Exp > GetSecondsFromEpoc())
            {
                TrustList = trustList;
                await File.WriteAllTextAsync(TrustListFileName, DSC_TLSerialize.ToJson(trustList));
            }
        }

        public async void RefreshValueSetsAsync()
        {
            if (ValueSets == null)
            {
                ValueSets = new Dictionary<string, ValueSet>();
            }
            Dictionary<string, string> valueSets = await _restService.RefreshValueSetAsync();
            if (valueSets != null && valueSets.Keys != null && valueSets.Keys.Count > 0)
            {
                foreach(KeyValuePair<string, string> entry in valueSets)
                {
                    _ = File.WriteAllTextAsync(Path.Combine(ValueSetPath, entry.Key), entry.Value);
                    ValueSets[entry.Key] = ValueSet.FromJson(entry.Value);
                }
            }
        }
        public void LoadValueSets()
        {
            if (ValueSets == null)
            {
                ValueSets = new Dictionary<string, ValueSet>();
            }
            foreach (string file in Constants.ValueSets)
            {
                if(File.Exists(Path.Combine(ValueSetPath, file)))
                {
                    ValueSet valueSet = ValueSet.FromJson(File.ReadAllText(Path.Combine(ValueSetPath, file)));
                    ValueSets[file] = valueSet;
                }
                else
                {
                    RefreshValueSetsAsync();
                    break;
                    //if (Device.RuntimePlatform == Device.Android)
                    //{
                    //}
                    //else if (Device.RuntimePlatform == Device.iOS)
                    //{
                    //    // Load default files within package
                    //    ValueSet valueSet = ValueSet.FromJson(File.ReadAllText("ValueSets/" + file));
                    //    ValueSets[file] = valueSet;
                    //}
                }
            }
        }

        public void LoadCertificates()
        {
            if (TrustList == null && File.Exists(TrustListFileName))
            {
                DSC_TL trustList = DSC_TL.FromJson(File.ReadAllText(TrustListFileName));
                // If trustlist hasn´t expired
                if (trustList.Exp > GetSecondsFromEpoc())
                {
                    TrustList = trustList;
                }
            }
            // If trustlist is not set or it´s older than 24 hours refresh it
            if (TrustList == null || (TrustList.Iat + 86400) < GetSecondsFromEpoc())
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
            if( country != null && country.Length > 0 && TrustList.DscTrustList.ContainsKey(country) )
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
                    string kidStr = Convert.ToBase64String(kid);
                        //.Replace('+', '-')
                        //.Replace('/', '_');
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
            return publicKeys;
        }

        private BigInteger Base64UrlDecodeToBigInt(String value)
        {
            value = value.Replace('-', '+');
            value = value.Replace('_', '/');
            switch (value.Length % 4)
            {
                case 0: break;
                case 2: value += "=="; break;
                case 3: value += "="; break;
                default:
                    throw new Exception("Illegal base64url string!");
            }
            return new BigInteger(1,Convert.FromBase64String(value));
        }
    }
}
