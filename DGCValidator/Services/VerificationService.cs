using System;
using System.IO;
using System.Text;
using DGCValidator.Services.CWT;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using PeterO.Cbor;

namespace DGCValidator.Services
{
    /**
     * A Crypto support class for the reading of the European Digital Green Certificate.
     *
     * @author Henrik Bengtsson (henrik@sondaica.se)
     * @author Martin Lindström (martin@idsec.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class VerificationService
    {
        public VerificationService()
        {
        }

        public static DGC VerifyData(String codeData)
        {
            try {
                // The base45 encoded data shoudl begin with HC1
                if( codeData.StartsWith("HC1"))
                {
                    String base45CodedData = codeData.Substring(3);

                    // Base 45 decode data
                    byte[] base45DecodedData = Base45Decoding(Encoding.GetEncoding("ISO-8859-1").GetBytes(base45CodedData));

                    // zlib decompression
                    byte[] uncompressedData = ZlibDecompression(base45DecodedData);

                    DGC vacProof = new DGC();
                    // Sign and encrypt data
                    byte[] signedData = VerifySignedData(uncompressedData, vacProof);

                    // Get json from CBOR representation of ProofCode
                    Vproof vProof = GetVaccinationProofFromCbor(signedData);
                    vacProof.vProof = vProof;

                    return vacProof;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
            return null;
        }

		protected static byte[] ZlibDecompression(byte[] compressedData)
        {
            if( compressedData[0] == 0x78 && compressedData[1] == 0xDA)
            {
                var outputStream = new MemoryStream();
                using (var compressedStream = new MemoryStream(compressedData))
                using (var inputStream = new InflaterInputStream(compressedStream))
                {
                    inputStream.CopyTo(outputStream);
                    outputStream.Position = 0;
                    return outputStream.ToArray();
                }
            }
            else
            {
                // The data is not compressed
                return compressedData;
            }
        }

		protected static byte[] VerifySignedData(byte[] signedData, DGC vacProof)
        {
            HCertVerifier verifier = new HCertVerifier(new CertificateProvider());
            byte[] hCertData = verifier.Verify(signedData, vacProof);
			return hCertData;
		}

        protected static byte[] Base45Decoding(byte[] encodedData)
        {
			byte[] uncodedData = Base45.Decode(encodedData);
			return uncodedData;
        }

		protected static Vproof GetVaccinationProofFromCbor(byte[] cborData)
		{
            CBORObject cbor = CBORObject.DecodeFromBytes(cborData, CBOREncodeOptions.Default);
            Vproof vacProof = Vproof.FromJson(cbor.ToJSONString());
            return vacProof;
        }


	}
}
