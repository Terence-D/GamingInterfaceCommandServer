using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GIC.Common
{
    public class CryptoService
    {
        private static readonly string salt = "f-NS5Qnhn@sdC%_7RAD3K3GmYU5ZHW*$";
        private static readonly string passphrase = "WCu8rer-kuJ58UZKcz*ABL$?&=@pw=_d4L$tr^cfFgdad-NxCUXRUz8LL*_5z?Xm^zFMP4FXRUvdEhNH=kwKJp8^-D3Uf8AG?6DtKrQH%HYC*c6KqW&XR2gF7hndfu+u";
        private static readonly string iv = "kYW-*%n^t!JkV3FU";
        private static readonly int iterations = 100;
        private static readonly int size = 32;
        private static readonly RijndaelManaged rijndaelCipher = new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7
        };

        public static string Encrypt(string toEncrypt)
        {
            byte[] toEncryptBytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] encodedSalt = Encoding.ASCII.GetBytes(salt);
            byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
            Rfc2898DeriveBytes rdbInstance = new Rfc2898DeriveBytes(passphrase, encodedSalt, iterations);
            byte[] randomBytes = rdbInstance.GetBytes(size);

            rijndaelCipher.Key = randomBytes;
            rijndaelCipher.IV = ivBytes;

            byte[] encryptedBytes = rijndaelCipher.CreateEncryptor().TransformFinalBlock(toEncryptBytes, 0, toEncryptBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string toDecrypt)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(toDecrypt);
                byte[] encodedSalt = Encoding.ASCII.GetBytes(salt);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                Rfc2898DeriveBytes rdbInstance = new Rfc2898DeriveBytes(passphrase, encodedSalt, iterations);
                byte[] randomBytes = rdbInstance.GetBytes(size);

                rijndaelCipher.Key = randomBytes;
                rijndaelCipher.IV = ivBytes;

                byte[] decryptedBytes = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
