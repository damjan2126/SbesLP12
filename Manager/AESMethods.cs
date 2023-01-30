using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public static class AESMethods
    {
       // public static readonly string keyRoute = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName, "key.txt");
        public static readonly string keyRoute = "C:\\Users\\DamjanTadic\\Desktop\\Faks\\sbes\\Vezba_03_resenje\\key.txt";
        private static string GenerateKey()
        {
            SymmetricAlgorithm symmAlgorithm = AesCryptoServiceProvider.Create();

            return symmAlgorithm == null ? String.Empty : ASCIIEncoding.ASCII.GetString(symmAlgorithm.Key);
        }

        public static void StoreKey(string secretKey, string outFile)
        {
            FileStream fOutput = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buffer = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                fOutput.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("SecretKeys.StoreKey:: ERROR {0}", e.Message);
            }
            finally
            {
                fOutput.Close();
            }
        }

        public static string LoadKey(string inFile)
        {
            FileStream fInput = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)fInput.Length];

            try
            {
                fInput.Read(buffer, 0, (int)fInput.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("SecretKeys.LoadKey:: ERROR {0}", e.Message);
            }
            finally
            {
                fInput.Close();
            }

            return ASCIIEncoding.ASCII.GetString(buffer);
        }

        private static string GetKey()
        {
            if (File.Exists(keyRoute))
                return LoadKey(keyRoute);

            var key = GenerateKey();

            StoreKey(key, keyRoute);

            return key;
        }

        public static byte[] EncryptText(string text)
        {
            byte[] encryptedArray = null;
            byte[] textArray = ASCIIEncoding.UTF8.GetBytes(text);
            string secretKey = GetKey();

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
                
            };
            ICryptoTransform aesEncryptTransform = aesCryptoProvider.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(textArray, 0, textArray.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedArray = memoryStream.ToArray();

                }
            }

            return encryptedArray;
        }

        public static string DecrytpedText(byte[] encryptedArray)
        {
            string decryptedText = null;
            byte[] decryptedArray = null;
            var key = GetKey();

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform aesDecryptTransform = aesCryptoProvider.CreateDecryptor();
            using (MemoryStream memoryStream = new MemoryStream(encryptedArray))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptTransform, CryptoStreamMode.Read))
                {
                    decryptedArray = new byte[encryptedArray.Length];     //decrypted image body - the same lenght as encrypted part
                    cryptoStream.Read(decryptedArray, 0, decryptedArray.Length);
                }
            }

            decryptedText = ASCIIEncoding.UTF8.GetString(decryptedArray);

            return decryptedText;
        }
    }
}
