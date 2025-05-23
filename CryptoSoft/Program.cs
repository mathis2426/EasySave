using System;
using System.Diagnostics;
using System.IO;

namespace CryptoSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vérifie qu'au moins un argument est passé
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: CryptoSoft.exe <sourceFilePath> <destinationFilePath>");
                return;
            }

            string sourceFile = args[0];
            string destinationFile = args[1];

            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"Le fichier source n'existe pas : {sourceFile}");
                return;
            }

            var crypto = new Encryption();

            var stopwatch = Stopwatch.StartNew();
            crypto.XorEncryptDecrypt(sourceFile, destinationFile);
            stopwatch.Stop();

            Console.WriteLine($"Fichier traité en {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
