using System;
using System.Runtime.InteropServices;
using SevenZip.Compression.Bzip2;

namespace Experiment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var decoder = Bzip2Encoder.Create(new Bzip2EncoderProperties { });


            Console.WriteLine();
            Console.Beep();
            Console.ReadLine();
        }
    }
}