using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AntiDump
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: AntiDump.exe <filepath>");
                Console.ReadLine();
                return;
            }
            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Couldnt find file");
                Console.ReadLine();
                return;
            }
            var directoryName = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath) + "_antidump";
            var fullFileName = fileName + Path.GetExtension(filePath);
            var newName = Path.Combine(directoryName, fullFileName);
            var fileBytes = File.ReadAllBytes(filePath);
            Dictionary<uint, uint> offsets = new Dictionary<uint, uint>()
            {
                {0xD0, 0x0},
                {0xD4, 0x0},
                {0xB4, 0x0}
            };
            for (int i = 0; i < offsets.Count; i++)
            {
                var sizeBytes = BitConverter.GetBytes(offsets.Values.ToArray()[i]);
                Array.Copy(sizeBytes, 0, fileBytes, offsets.Keys.ToArray()[i], sizeBytes.Length);
            }
            File.WriteAllBytes(newName, fileBytes);
            Console.WriteLine($"Completed. output: {newName}");
            Console.ReadLine();
            return;
        }
    }
}