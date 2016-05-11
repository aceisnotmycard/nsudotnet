using System;
using System.IO;
using System.Security.Cryptography;

namespace Bogolepov.Nsudotnet.Enigma
{
    public class Cipher
    {
        public static readonly string[] Algorithms = 
        {
            "Aes", "Des", "Rc2", "Rijndael"
        };

        public static void Encrypt(string input, string output, string algorithm)
        {
            var symAlgo = SymmetricAlgorithm.Create(algorithm);
            using (var inputStream = new FileStream(input, FileMode.Open, FileAccess.Read))
            using (var outputStream = new FileStream(output, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(inputStream, symAlgo.CreateEncryptor(), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }

            string key = Path.ChangeExtension(input, string.Concat(".key", Path.GetExtension(input)));
            using (var outputStream = new FileStream(key, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.WriteLine(Convert.ToBase64String(symAlgo.Key));
                streamWriter.WriteLine(Convert.ToBase64String(symAlgo.IV));
            }
        }

        public static void Decrypt(string input, string output, string algorithm, string keys)
        {
            var symAlgo = SymmetricAlgorithm.Create(algorithm);
            using (var inputStream = new FileStream(keys, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(inputStream))
            {
                symAlgo.Key = Convert.FromBase64String(streamReader.ReadLine());
                symAlgo.IV = Convert.FromBase64String(streamReader.ReadLine());
            }
            using (var inputStream = new FileStream(input, FileMode.Open, FileAccess.Read))
            using (var outputStream = new FileStream(output, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(inputStream, symAlgo.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }
        }
    }
}