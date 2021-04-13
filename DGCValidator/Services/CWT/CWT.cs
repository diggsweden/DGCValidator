using System;
using System.Collections.Generic;
using PeterO.Cbor;

namespace DGCValidator.Services.CWT
{

    /**
     * A representation of a CWT according to <a href="https://tools.ietf.org/html/rfc8392">RFC 8392</a>.
     * 
     * @author Henrik Bengtsson (henrik@sondaica.se)
     * @author Martin Lindström (martin@idsec.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class CWT
    {
        /** HCERT message tag. */
        public static int HCERT_CLAIM_KEY = -65537;

        /** The message tag for eu_hcert_v1 that is added under the HCERT claim. */
        public static int EU_HCERT_V1_MESSAGE_TAG = 1;

        /** For handling of DateTime. */
        private static CBORDateTimeConverter dateTimeConverter = new CBORDateTimeConverter();

        /** The CBOR representation of the CWT. */
        private CBORObject cwtObject;

        /**
            * Constructor creating an empty CWT.
            */
        public CWT() {
            cwtObject = CBORObject.NewMap();
        }

        /**
            * Constructor creating a CWT from a supplied encoding.
            * 
            * @param data
            *          the encoding
            * @throws CBORException
            *           if the supplied encoding is not a valid CWT
            */
        public CWT(byte[] data){
            CBORObject obj = CBORObject.DecodeFromBytes(data);
            if (obj.Type != CBORType.Map) {
                throw new CBORException("Not a valid CWT");
            }
            cwtObject = obj;
        }

        /**
            * Decodes the supplied data into a Cwt object.
            *
            * @param data
            *          the encoded data
            * @return a Cwt object
            * @throws CBORException
            *           if the supplied encoding is not a valid CWT
            */
        public static CWT Decode(byte[] data){
            return new CWT(data);
        }

        /**
            * Gets the binary representation of the CWT.
            * 
            * @return a byte array
            */
        public byte[] Encode() {
            return cwtObject.EncodeToBytes();
        }

        /**
            * Gets the "iss" (issuer) claim.
            * 
            * @return the issuer value, or null
            */
        public String GetIssuer() {
            return cwtObject[1].AsString();
        }

        /**
         * Gets the "sub" (subject) claim.
         * 
         * @return the subject value, or null
         */
        public String GetSubject() {
            return cwtObject[2].AsString();
        }

            ///**
            // * Gets the values for the "aud" claim
            // * 
            // * @return the value, or null
            // */
            //public List<String> getAudience() {
            //  CBORObject aud = cwtObject[3];
            //  if (aud == null) {
            //    return null;
            //  }
            //  if (aud.Type == CBORType.Array) {
            //    Collection<CBORObject> values = aud.Values;
            //    return values.stream().map(CBORObject::AsString).collect(Collectors.toList());
            //  }
            //  else {
            //    return Arrays.asList(aud.AsString());
            //  }
            //}

        /**
        * Gets the value of the "exp" (expiration time) claim.
        * 
        * @return the instant, or null
        */
        public DateTime GetExpiration() {
        return dateTimeConverter.FromCBORObject(cwtObject[4]);
        }

        /**
         * Gets the value of the "nbf" (not before) claim.
         * 
         * @return the instant, or null
         */
        public DateTime GetNotBefore() {
        return dateTimeConverter.FromCBORObject(cwtObject[5]);
        }

  
        /**
         * Gets the value of the "iat" (issued at) claim.
         * 
         * @return the instant, or null
         */
        public DateTime GetIssuedAt() {
        return dateTimeConverter.FromCBORObject(cwtObject[6]);
        }

 
        /**
         * Gets the value of the "cti" (CWT ID) claim.
         * 
         * @return the ID, or null
         */
        public byte[] GetCwtId() {
        return cwtObject[7].GetByteString();
        }

        /**
         * Gets the binary representation of a EU HCERT v1 structure.
         * 
         * @return the binary representation of a EU HCERT or null
         */
        public byte[] GetHCertv1()
        {
            CBORObject hcert = cwtObject[HCERT_CLAIM_KEY];
            if (hcert == null)
            {
                return null;
            }
            return hcert[EU_HCERT_V1_MESSAGE_TAG].GetByteString();
        }


        ///**
        // * Gets the claim identified by {@code claimKey}.
        // * 
        // * @param claimKey
        // *          the claim key
        // * @return the claim value (in its CBOR binary encoding), or null
        // */
        //public byte[] getClaim(int claimKey) {
        //    return Optional.ofNullable(cwtObject.get(claimKey)).map(CBORObject::GetByteString).orElse(null);
        //  }

        //  /**
        //   * Gets the claim identified by {@code claimKey}.
        //   * 
        //   * @param claimKey
        //   *          the claim key
        //   * @return the claim value (in its CBOR binary encoding), or null
        //   */
        //  public byte[] getClaim(String claimKey) {
        //    return Optional.ofNullable(cwtObject[claimKey]).map(CBORObject::GetByteString).orElse(null);
        //  }

        /** {@inheritDoc} */
        public override String ToString() {
            return cwtObject.ToString();
        }
    }
}
