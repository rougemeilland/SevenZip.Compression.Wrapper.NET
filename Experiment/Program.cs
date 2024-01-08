using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

namespace Experiment
{
    public class Program
    {
        private class ProgressReporter
            : IProgress<(ulong? inStreamProgressedCount, ulong? outStreamProcessedCount)>
        {
            public void Report((ulong? inStreamProgressedCount, ulong? outStreamProcessedCount) value)
            {
                Console.Write("\x1b[0K");
                Console.Write($"in: {(value.inStreamProgressedCount is null ? "???" : value.inStreamProgressedCount.Value.ToString("N0") + " bytes")}, ");
                Console.Write($"out: {(value.outStreamProcessedCount is null ? "???" : value.outStreamProcessedCount.Value.ToString("N0") + " bytes")}");
                Console.Write("\r");
            }
        }

        public static void Main(string[] args)
        {
            using (var inUncompressedStream = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.None))
            using (var outCompressedStream = new FileStream(args[1], FileMode.Create, FileAccess.Write, FileShare.None))
            using (var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal}))
            {
                deflateEncoder.Code(inUncompressedStream, outCompressedStream, (ulong)inUncompressedStream.Length, null, new ProgressReporter());
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Completed.");
            Console.Beep();
            _ = Console.ReadLine();
        }
    }
}
