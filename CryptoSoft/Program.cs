using System;
using System.IO;
using System.Collections;

namespace CryptoSoft
{
    class Program
    {
        static int Main(string[] args)
        {
            int argsSize = args.Length;
            string src = "";
            string dst = "";

            for (int i = 0; i < argsSize; i++)
            {
                if (args[i] == "source" && i + 1 < argsSize)
                {
                    src = args[i + 1];
                    i++;
                }
                else if (args[i] == "destination" && i + 1 < argsSize)
                {
                    dst = args[i + 1];
                    i++;
                }
            }

            if (src.Length == 0 || dst.Length == 0)
            {
                Console.WriteLine("Missing arguments");
                return -1;
            }
            else if (!File.Exists(src))
            {
                Console.WriteLine("Source file doesn't exist.");
                return -1;
            }

            try
            {
                DateTime startTimeFile = DateTime.Now;

                byte[] byteToEncrypt = File.ReadAllBytes(src);
                BitArray bitToEncrypt = new BitArray(byteToEncrypt);

                byte[] byteKey = new byte[8] { 12, 255, 102, 147, 8, 52, 157, 235 };
                BitArray bitKey = new BitArray(byteKey);

                byte[] byteCrypted = new byte[byteToEncrypt.Length];
                BitArray bitCrypted = new BitArray(bitToEncrypt.Length);

                int j = 0;

                for (int i = 0; i < bitToEncrypt.Length; i++)
                {
                    j = i % byteKey.Length;
                    bitCrypted[i] = bitToEncrypt[i] ^ bitKey[j];
                }

                bitCrypted.CopyTo(byteCrypted, 0);

                File.WriteAllBytes(dst, byteCrypted);
                TimeSpan cryptTime = DateTime.Now - startTimeFile;
                return (int)cryptTime.TotalMilliseconds;
            }
            catch
            {
                Console.WriteLine("Cannot crypt this file.");
                return -1;
            }
        }
    }
}
