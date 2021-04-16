using System;
using System.Text;
/*
* MIT License
*
* Copyright 2021 Myndigheten för digital förvaltning (DIGG)
*/
namespace DGCValidator.Services
{
    /**
     * Implementation of Base45 encoding/decoding according to <a href=
     * "https://datatracker.ietf.org/doc/draft-faltstrom-base45/">https://datatracker.ietf.org/doc/draft-faltstrom-base45/</a>.
     *
     * @author Martin Lindström (martin@idsec.se)
     * @author Henrik Bengtsson (extern.henrik.bengtsson@digg.se)
     * @author Henric Norlander (extern.henric.norlander@digg.se)
     */
    public class Base45
    {
        private static readonly char[] Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:".ToCharArray();
        private static readonly int[] DecodingTable = new int[256];

        static Base45()
        {
            for (int i = 0; i < Base45.DecodingTable.Length; i++)
                DecodingTable[i] = -1;
            for (int i = 0; i < 45; i++)
                DecodingTable[Alphabet[i]] = i;
        }


        /**
         * Decodes the supplied input (which is the byte array representation of a Base45 string).
         *
         * @param src
         *          the Base45 string to decode
         * @return an allocated byte array
         */
        public static byte[] Decode(byte[] coded)
        {
            int uncodedLength = ((coded.Length / 3) * 2) + ((coded.Length % 3) / 2);
            byte[] output = new byte[uncodedLength];
            int ip = 0;
            int op = 0;
            while (ip < coded.Length)
            {
                int i0 = coded[ip++];
                int i1 = coded[ip++];
                int i2 = (ip < coded.Length ? coded[ip] : 0);
                if (i0 > 127 || i1 > 127 || i2 > 127)
                    throw new ArgumentException("Illegal character in Base45 encoded data.");
                int b0 = DecodingTable[i0];
                int b1 = (ip <= coded.Length ? DecodingTable[i1] : 0);
                int b2 = (ip < coded.Length ? DecodingTable[i2] : 0);
                if (b0 < 0 || b1 < 0 || b2 < 0)
                    throw new ArgumentException("Illegal character in Base64 encoded data.");
                int value = b0 + 45 * b1 + 45 * 45 * b2;
                int o0 = (value / 256);
                int o1 = (value % 256);
                output[op++] = (op < uncodedLength ? (byte)o0 : (byte)o1);
                if (op < uncodedLength)
                {
                    output[op++] = (byte)o1;
                }
                ip++;
            }
            return output;

        }

        /**
         * Decodes the supplied Base45 string.
         *
         * @param src
         *          the Base45 string to decode
         * @return an allocated byte array
         */
        public byte[] Decode(string src)
        {
            return Decode(Encoding.ASCII.GetBytes(src));
        }

    }



    public class Base45Old
    {
        private static readonly char[] map1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:".ToCharArray();
        private static byte[] map2 = new byte[256];

        /*  The Base45 Alphabet.
        *
        * <pre>
        *   00 0            12 C            24 O            36 Space
        *   01 1            13 D            25 P            37 $
        *   02 2            14 E            26 Q            38 %
        *   03 3            15 F            27 R            39 *
        *   04 4            16 G            28 S            40 +
        *   05 5            17 H            29 T            41 -
        *   06 6            18 I            30 U            42 .
        *   07 7            19 J            31 V            43 /
        *   08 8            20 K            32 W            44 :
        *   09 9            21 L            33 X
        *   10 A            22 M            34 Y
        *   11 B            23 N            35 Z
        * </pre>
        */


        static Base45Old()
        {
            for (int i = 0; i < Base45Old.map2.Length; i++)
                map2[i] = 0;
            for (int i = 0; i < 45; i++)
                map2[map1[i]] = (byte)i;
        }


        /**
         * @return the base 45 encoded value of a string.
         */
        public static char[] Encode(byte[] uncoded)
        {
            int codedLength = ((uncoded.Length / 2) * 3) + ((uncoded.Length % 2) * 2);
            char[] coded = new char[codedLength];
            int ip = 0;
            int op = 0;
            while (ip < uncoded.Length)
            {
                int i0 = uncoded[ip++] & 0xff;
                int i1 = ip < uncoded.Length ? uncoded[ip] & 0xff : 0;
                int output = (ip < uncoded.Length ? i0 * 256 + i1 : i0);
                int o0 = output % 45;
                int o1 = (output / 45) % 45;
                int o2 = (output / 45) / 45;

                coded[op++] = map1[o0];
                coded[op] = (op < codedLength ? map1[o1] : map1[o2]);
                op++;
                if (op < codedLength)
                {
                    coded[op++] = map1[o2];
                }
                ip++;
            }
            return coded;
        }

        /**
         * @return the String value of the base 45 encoded input.
         */
        public static byte[] Decode(byte[] coded)
        {
            int uncodedLength = ((coded.Length / 3) * 2) + ((coded.Length % 3) / 2);
            byte[] output = new byte[uncodedLength];
            int ip = 0;
            int op = 0;
            while (ip < coded.Length)
            {
                int i0 = coded[ip++];
                int i1 = coded[ip++];
                int i2 = (ip < coded.Length ? coded[ip] : 0);
                if (i0 > 127 || i1 > 127 || i2 > 127)
                    throw new ArgumentException("Illegal character in Base45 encoded data.");
                int b0 = map2[i0];
                int b1 = (ip <= coded.Length ? map2[i1] : 0);
                int b2 = (ip < coded.Length ? map2[i2] : 0);
                if (b0 < 0 || b1 < 0 || b2 < 0)
                    throw new ArgumentException("Illegal character in Base64 encoded data.");
                int value = b0 + 45 * b1 + 45 * 45 * b2;
                int o0 = (value / 256);
                int o1 = (value % 256);
                output[op++] = (op < uncodedLength ? (byte)o0 : (byte)o1);
                if (op < uncodedLength)
                {
                    output[op++] = (byte)o1;
                }
                ip++;
            }
            return output;

        }
    }
}

