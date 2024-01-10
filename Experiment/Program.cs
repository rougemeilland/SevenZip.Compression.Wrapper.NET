using System;
using SevenZip.Compression;

namespace Experiment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"version={SevenZipModule.Versio >> 16}.{(ushort)SevenZipModule.Versio}");
            Console.WriteLine($"InterfaceType={SevenZipModule.InterfaceType}");

            Console.WriteLine("Completed.");
            Console.Beep();
            _ = Console.ReadLine();
        }
    }
}
