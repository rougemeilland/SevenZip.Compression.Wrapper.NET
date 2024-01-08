using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;
using SevenZip.Compression.Lzma;

namespace Experiment
{
    public class Program
    {
        // このクラスは圧縮の進捗状況をコンソールに表示するためのものです。
        // 必ずしも必要ではありません。
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

        // これは、ファイルの内容を Deflate で圧縮して別のファイルへ保存するメソッドです。
        public static void CompressWithDeflate(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // deflate エンコーダオブジェクトを作成します。
            using var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal });

            // 圧縮を開始します。
            deflateEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、Deflate で圧縮されたファイルを伸長して別のファイルに保存するメソッドです。
        public static void UncompressWithDeflate_1(string compressedFilePath, string uncompressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // deflate デコーダオブジェクトを作成します。
            using var deflateDecoder = DeflateDecoder.CreateDecoder();

            // 圧縮を開始します。
            deflateDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、Deflate で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        // 
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
        {
            // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。 
            return DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);
        }

        // これは、ファイルの内容を LZMA で圧縮して別のファイルへ保存するメソッドです。
        public static void CompressWithLzma(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA エンコーダオブジェクトを作成します。
            using var lzmaEncoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal, EndMarker = true });
            
            // LZMA などいくつかのエンコーダではこの手順が必要です。
            lzmaEncoder.WriteCoderProperties(outCompressedStream);

            // 圧縮を開始します。
            lzmaEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、LZMA で圧縮されたファイルを伸長して別のファイルに保存するメソッドです。
        public static void UncompressWithLzma_1(string compressedFilePath, string uncompressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA などいくつかのデコーダではこの手順が必要です。
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // 出力ファイルを開きます。
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA デコーダオブジェクトを作成します。
            using var lzmaDecoder = LzmaDecoder.CreateDecoder(contentProperties);

            // 圧縮を開始します。
            lzmaDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、LZMA で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。
        public static Stream UncompressWithLzma_2(string compressedFilePath)
        {
            // 入力ファイルを開きます。
            var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA などいくつかのデコーダではこの手順が必要です。
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。 
            return LzmaDecoder.CreateDecoderStream(inCompressedStream, null, contentProperties);
        }

        public static void Main(string[] args)
        {
            var sourceFilePath = new FilePath(args[0]);

            var compressedFilePath1 = (FilePath?)null;
            var uncompressedFilePath1_1 = (FilePath?)null;
            var uncompressedFilePath1_2 = (FilePath?)null;

            var compressedFilePath2 = (FilePath?)null;
            var uncompressedFilePath2_1 = (FilePath?)null;
            var uncompressedFilePath2_2 = (FilePath?)null;
            try
            {
                compressedFilePath1 = new FilePath(Path.GetTempFileName());
                uncompressedFilePath1_1 = new FilePath(Path.GetTempFileName());
                CompressWithDeflate(sourceFilePath.FullName, compressedFilePath1.FullName);

                Console.WriteLine("\x1b[0KDeflate compresssion completed.");
                Console.WriteLine();

                UncompressWithDeflate_1(compressedFilePath1.FullName, uncompressedFilePath1_1.FullName);
                if (!CompareFile(sourceFilePath, uncompressedFilePath1_1))
                    throw new Exception();

                Console.WriteLine("\x1b[0KDeflate uncompresssion(1) completed.");
                Console.WriteLine();

                uncompressedFilePath1_2 = new FilePath(Path.GetTempFileName());
                using (var inStream = UncompressWithDeflate_2(compressedFilePath1.FullName))
                using (var outStream = uncompressedFilePath1_2.Create().AsDotNetStream())
                {
                    inStream.CopyTo(outStream);
                }

                if (!CompareFile(sourceFilePath, uncompressedFilePath1_2))
                    throw new Exception();

                Console.WriteLine("\x1b[0KDeflate uncompresssion(2) completed.");
                Console.WriteLine();

                compressedFilePath2 = new FilePath(Path.GetTempFileName());
                uncompressedFilePath2_1 = new FilePath(Path.GetTempFileName());
                CompressWithLzma(sourceFilePath.FullName, compressedFilePath2.FullName);

                Console.WriteLine("\x1b[0KLZMA compresssion completed.");
                Console.WriteLine();

                UncompressWithLzma_1(compressedFilePath2.FullName, uncompressedFilePath2_1.FullName);
                if (!CompareFile(sourceFilePath, uncompressedFilePath2_1))
                    throw new Exception();

                Console.WriteLine("\x1b[0KLZMA uncompresssion(1) completed.");
                Console.WriteLine();

                uncompressedFilePath2_2 = new FilePath(Path.GetTempFileName());
                using (var inStream = UncompressWithLzma_2(compressedFilePath2.FullName))
                using (var outStream = uncompressedFilePath2_2.Create().AsDotNetStream())
                {
                    inStream.CopyTo(outStream);
                }

                if (!CompareFile(sourceFilePath, uncompressedFilePath2_2))
                    throw new Exception();

                Console.WriteLine("\x1b[0KLZMA uncompresssion(2) completed.");
                Console.WriteLine();
            }
            finally
            {
                compressedFilePath1?.Delete();
                uncompressedFilePath1_1?.Delete();
                uncompressedFilePath1_2?.Delete();

                compressedFilePath2?.Delete();
                uncompressedFilePath2_1?.Delete();
                uncompressedFilePath2_2?.Delete();
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Completed.");
            Console.Beep();
            _ = Console.ReadLine();
        }

        private static bool CompareFile(FilePath sourceFilePath, FilePath uncompressedFilePath1)
        {
            using var inStream1 = sourceFilePath.OpenRead();
            using var inStream2 = uncompressedFilePath1.OpenRead();
            return inStream1.StreamBytesEqual(inStream2);
        }
    }
}
