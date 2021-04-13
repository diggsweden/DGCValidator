using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;

/**
* A HCert verifier class.
*
* @author Henrik Bengtsson (henrik@sondaica.se)
* @author Martin Lindström (martin@idsec.se)
* @author Henric Norlander (extern.henric.norlander@digg.se)
*/
namespace DGCValidator.Services.CWT
{
    public class HCertVerifier
    {
        private readonly ICertificateProvider certificateProvider;

        public HCertVerifier(ICertificateProvider certificateProvider)
        {
            this.certificateProvider = certificateProvider;
        }

        /**
         * Verifies the supplied signed HCERT. If verification is successful the method returns the contained HCERT
         * (eu_hcert_v1) in its binary representation.
         * 
         * @throws SignatureException
         *           if signature validation fails
         * @throws CertificateExpiredException
         *           if the HCERT has expired
         */
        public byte[] Verify(byte[] signedHCert, DGC vacProof){
            CoseSign1_Object obj = CoseSign1_Object.Decode(signedHCert);

            byte[] kid = obj.GetKeyIdentifier();
            String country = obj.GetCwt().GetIssuer();

            vacProof.issuingCountry = country;

            if (kid == null && country == null) {
                throw new Exception("Signed object does not contain kid or country - cannot find certificate");
            }

            List<ECPublicKeyParameters> certs = this.certificateProvider.GetCertificates(country, kid);

            foreach (ECPublicKeyParameters cert in certs) {
                Console.WriteLine("Attempting HCERT signature verification using certificate");// '{0}'", cert.Subject);//getSubjectX500Principal().getName()) ;

                try {
                    byte[] key = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(cert).GetEncoded();
                    obj.VerifySignature(key);
                    Console.WriteLine("HCERT signature verification succeeded using certificate");// '{0}'", cert.Subject); //getSubjectX500Principal().getName());
                }
                catch (Exception e)
                {
                    Console.WriteLine("HCERT signature verification failed using certificate '{0}' - {1}",
                    cert, e.Message, e);
                    continue;
                }

                // OK, before we are done - let's ensure that the HCERT hasn't expired.
                CWT cwt = obj.GetCwt();

                DateTime expiration = cwt.GetExpiration();
                if (expiration != null) {
                    if (DateTime.Now.CompareTo(expiration)>=0) {
                        throw new CertificateExpiredException("Signed HCERT has expired");
                    }
                }
                else
                {
                    Console.WriteLine("Signed HCERT did not contain an expiration time - assuming it is valid");
                }
                vacProof.expirationDate = expiration;
                vacProof.issuedDate = cwt.GetIssuedAt();
                // OK, everything looks fine - return the HCERT
                return cwt.GetHCertv1();
            }

            if (certs.Count<=0)
            {
                throw new Exception("No signer certificates could be found");
            }
            else
            {
                throw new Exception("Signature verification failed for all attempted keys");
            }
        }
    }
}
