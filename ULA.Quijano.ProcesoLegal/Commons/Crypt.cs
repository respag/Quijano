using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace ULA.Quijano.ProcesoLegal.Commons
{
    public class Crypt
    {
        private string llave_ = "ultimusPanama";

        public string EncryptString(string Message)
        {
            string retorno = string.Empty;
            byte[] Results = null;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            TripleDESCryptoServiceProvider TDESAlgorithm = null;
            MD5CryptoServiceProvider HashProvider = null;

            try
            {
                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(llave_));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the encoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToEncrypt = UTF8.GetBytes(Message);

                // Step 5. Attempt to encrypt the string            
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            catch (Exception)
            {
                retorno = string.Empty;
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            if (Results != null)
                retorno = Convert.ToBase64String(Results);
            return retorno;
        }

        public string DecryptString(string Message)
        {
            string retorno = string.Empty;
            byte[] Results = null;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = null;
            TripleDESCryptoServiceProvider TDESAlgorithm = null;

            try
            {
                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(llave_));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the decoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToDecrypt = Convert.FromBase64String(Message.Replace(" ", "+").ToString());

                // Step 5. Attempt to decrypt the string
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            catch (Exception)
            {
                retorno = string.Empty;
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            if (Results != null)
                retorno = UTF8.GetString(Results);

            return retorno;
        }
    }
}