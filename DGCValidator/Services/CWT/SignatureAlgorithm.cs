using System;
using System.ComponentModel;
using PeterO.Cbor;

namespace DGCValidator.Services.CWT
{
    /**
     * Representation of the supported signature algorithms.
     * 
     * @author Henrik Bengtsson (henrik@sondaica.se)
     * @author Martin Lindström (martin@idsec.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class SignatureAlgorithm
    {
        /** ECDSA with SHA-256. */
        public static readonly CBORObject ES256 = CBORObject.FromObject(-7);

        /** ECDSA with SHA-384. */
        public static readonly CBORObject ES384 = CBORObject.FromObject(-35);

        /** ECDSA with SHA-512. */
        public static readonly CBORObject ES512 = CBORObject.FromObject(-36);

        /** RSASSA-PSS with SHA-256. */
        public static readonly CBORObject PS256 = CBORObject.FromObject(-37);

        /** RSASSA-PSS with SHA-384. */
        public static readonly CBORObject PS384 = CBORObject.FromObject(-38);

        /** RSASSA-PSS with SHA-512. */
        public static readonly CBORObject PS512 = CBORObject.FromObject(-39);


        public static String GetAlgorithmName(CBORObject cborValue)
        {
            switch (cborValue.AsInt32())
            {
                case -7:
                    return "SHA256withECDSA";
                case -35:
                    return "SHA384withECDSA";
                case -36:
                    return "SHA512withECDSA";
                case -37:
                    return "SHA256withRSA/PSS";
                case -38:
                    return "SHA384withRSA/PSS";
                case -39:
                    return "SHA512withRSA/PSS";
                default:
                    break;
            }
            return null;
        }

        ///**
        // * Given a CBOR object the method gets the corresponding {@code SignatureAlgorithmId}.
        // * 
        // * @param value
        // *          the value
        // * @return a SignatureAlgorithmId
        // */
        //public static SignatureAlgorithm fromCborObject(CBORObject value)
        //{
        //    if (value == null)
        //    {
        //        throw new Exception("value must not be null");
        //    }
        //    switch (value)
        //    {
        //        case 
        //    }
        //    for (SignatureAlgorithm a : SignatureAlgorithm.values())
        //    {
        //        if (value.equals(a.value))
        //            return a;
        //    }
        //    throw new IllegalArgumentException("No SignatureAlgorithmID matching " + value);
        //}

        ///**
        // * Given a value the method gets the corresponding {@code SignatureAlgorithmId}.
        // * 
        // * @param value
        // *          the value
        // * @return a SignatureAlgorithmId
        // */
        //public static SignatureAlgorithm fromValue(final int value)
        //{
        //    for (final SignatureAlgorithm a : SignatureAlgorithm.values())
        //    {

        //        if (a.value.AsInt32Value() == value)
        //        {
        //            return a;
        //        }
        //    }
        //    throw new IllegalArgumentException("No SignatureAlgorithmId matching " + value);
        //}

        ///**
        // * Gets the signature identifier as a CBOR object.
        // * 
        // * @return a CBORObject
        // */
        //public CBORObject getCborObject()
        //{
        //    return this.value;
        //}

        ///**
        // * Gets the CBOR value for the signature identifier.
        // * 
        // * @return the value
        // */
        //public int getValue()
        //{
        //    return this.value.AsInt32Value();
        //}

        ///**
        // * Gets the JCA algorithm name for this algorithm.
        // * 
        // * @return the JCA algorithm name
        // */
        //public String getJcaAlgorithmName()
        //{
        //    return this.jcaAlgorithmName;
        //}

    }
}
