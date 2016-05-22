using System;
using System.IO;

namespace Bogolepov.Nsudotnet.Enigma
{
    class Program
    {
        private const string EncryptArg = "encrypt";
        private const string DecryptArg = "decrypt";

        private const bool EncryptMode = false;

        private const string UnsupportedAction = "Please choose what you want to do: encrypt or decrypt";
        private const string CommandsFormat = @"
USAGE:
crypto.exe encrypt {input filename} {algorithm} {output filename}
crypto.exe decrypt {input filename} {algorithm} {keys filename} {output filename}
";
        private const string SupportedAlgorithms = @"
Following algorithms are supported:
* aes
* des
* rc2
* rijndael
";
        private const int EncryptArgsCount = 4;
        private const int DecryptArgsCount = 5;

        private static void Main(string[] args)
        {
            string algorithm;
            bool mode;
            if (CheckArgs(args, out algorithm, out mode))
            {
                if (mode == EncryptMode)
                {
                    Cipher.Encrypt(args[1], args[3], algorithm);
                    Console.WriteLine("Succesfully encrypted file.");    
                }
                else
                {
                    Cipher.Decrypt(args[1], args[4], algorithm, args[3]);
                    Console.WriteLine("Succesfully decrypted file.");
                }
            }
        }

        private static bool CheckArgs(string[] args, out string algorithm, out bool mode)
        {
            algorithm = "";
            mode = EncryptMode;
            if (args.Length != EncryptArgsCount && args.Length != DecryptArgsCount)
            {
                Console.WriteLine(CommandsFormat);
                return false;
            }
            if (args[0] != EncryptArg && args[0] != DecryptArg)
            {
                Console.WriteLine(UnsupportedAction);
                return false;
            }
            if (!Array.Exists(Cipher.Algorithms, s => s.ToLower() == args[2].ToLower()))
            {
                Console.WriteLine(SupportedAlgorithms);
                return false;
            }
       
            algorithm = args[2];

            if (!File.Exists(args[1]))
            {
                Console.WriteLine($"{args[1]} not exists!");
                return false;
            }

            if (args[0] == EncryptArg)
            {
                mode = EncryptMode;
                if (File.Exists(args[3]))
                {
                    Console.WriteLine($"{args[3]} already exists.");
                    return false;
                }
            }
            else
            {
                mode = !EncryptMode;
                if (!File.Exists(args[3]))
                {
                    Console.WriteLine($"{args[3]} not exists!");
                    return false;
                }
                if (File.Exists(args[4]))
                {
                    Console.WriteLine($"{args[4]} already exists.");
                    return false;
                }
            }
            return true;
        }
    }
}