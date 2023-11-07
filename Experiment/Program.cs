using SevenZip.Compression.Bzip2;
using SevenZip.Compression;
using SevenZip.Compression.Lzma;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Experiment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string testDataPath = @"D:\テストデータ\source\TEST_程々に圧縮可能なファイル.txt";

            NewMethod(
                testDataPath,
                (inStream, outStream, inStreamSize) =>
                {
                    using (var encoder = Bzip2Encoder.Create(new Bzip2EncoderProperties
                    {
                        NumThreads = (UInt32)Environment.ProcessorCount,
                        NumPasses = 10,
                        DictionarySize = 900000,
                        Level = CompressionLevel.Ultra,
                    }))
                    {
                        encoder.Code(inStream, outStream, inStreamSize, null, null);
                    }
                },
                (inStream, outStream) =>
                {
                    using (var decoder = Bzip2Decoder.Create(new Bzip2DecoderProperties { FinishMode = true, NumThreads = UInt32.MaxValue }))
                    {
                        decoder.Code(inStream, outStream, null, null, null);
                    }
                });

            Console.WriteLine("");
            Console.WriteLine();
            Console.Beep();
            Console.ReadLine();
        }

        private static void NewMethod(string testDataPath, Action<Stream, Stream, UInt64> Encoder, Action<Stream, Stream> Decoder)
        {
            string? temporaryFile1Path = null;
            string? temporaryFile2Path = null;
            try
            {
                {
                    temporaryFile1Path = Path.GetTempFileName();
                    using var inStream = new FileStream(testDataPath, FileMode.Open, FileAccess.Read);
                    using var outStream = new FileStream(temporaryFile1Path, FileMode.Create, FileAccess.Write);
                    var inStreamLength = (UInt64)inStream.Length;
                    Encoder(inStream, outStream, inStreamLength);
                }
                {
                    temporaryFile2Path = Path.GetTempFileName();
                    using var inStream = new FileStream(temporaryFile1Path, FileMode.Open, FileAccess.Read);
                    using var outStream = new FileStream(temporaryFile2Path, FileMode.Create, FileAccess.Write);
                    Decoder(inStream, outStream);
                }
                {
                    var originalData = File.ReadAllBytes(testDataPath);
                    var uncompressedData = File.ReadAllBytes(temporaryFile2Path);
                    if (!originalData.SequenceEqual(uncompressedData))
                        throw new Exception("データが一致しません。");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            finally
            {
                if (temporaryFile1Path is not null)
                    File.Delete(temporaryFile1Path);
                if (temporaryFile2Path is not null)
                    File.Delete(temporaryFile2Path);
            }
        }
    }
}
