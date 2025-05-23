using System;
using System.IO;
using System.Text;

namespace CryptoSoft
{
    class Encryption
    {
        private byte[] LoadKey()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string configFile = Path.Combine(desktop, "config.txt");

            if (!File.Exists(configFile))
                throw new FileNotFoundException("Configuration file not found.");

            string keyString = File.ReadAllText(configFile).Trim();

            if (keyString.Length < 8)
                throw new Exception("Key must be at least 64 bits (8 characters).");

            return Encoding.UTF8.GetBytes(keyString);
        }

        public void XorEncryptDecrypt(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found.");

            byte[] key = LoadKey();
            int keyLength = key.Length;

            using (FileStream input = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (FileStream output = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                int b;
                int i = 0;
                while ((b = input.ReadByte()) != -1)
                {
                    byte encryptedByte = (byte)(b ^ key[i % keyLength]);
                    output.WriteByte(encryptedByte);
                    i++;
                }
            }
            Console.WriteLine("Encryption/Decryption completed successfully.");
            ;
        }
    }
}