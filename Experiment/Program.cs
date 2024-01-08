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
            using var deflateEncoder = DeflateDecoder.CreateDecoder();
            // 圧縮を開始します。
            deflateEncoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、Deflate で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
            => DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);

        // これは、ファイルの内容を LZMA で圧縮して別のファイルへ保存するメソッドです。
        public static void CompressWithLzma(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            // 出力ファイルを開きます。
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            // deflate エンコーダオブジェクトを作成します。
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
            Span<byte> buffer = stackalloc byte[LzmaEncoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(buffer) != buffer.Length)
                throw new UnexpectedEndOfStreamException();

            // 出力ファイルを開きます。
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            // deflate デコーダオブジェクトを作成します。
            using var deflateEncoder = LzmaDecoder.CreateDecoder(buffer);
            // 圧縮を開始します。
            deflateEncoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

        // これは、Deflate で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
        {
            return DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);
        }

        public static void Main(string[] args)
        {
            var sourceFilePath = new FilePath(args[0]);
            var compressedFilePath1 = (FilePath?)null;
            var uncompressedFilePath1 = (FilePath?)null;
            var compressedFilePath2 = (FilePath?)null;
            var uncompressedFilePath2 = (FilePath?)null;
            var compressedFilePath3 = (FilePath?)null;
            var uncompressedFilePath3 = (FilePath?)null;
            var compressedFilePath4 = (FilePath?)null;
            var uncompressedFilePath4 = (FilePath?)null;
            try
            {
                compressedFilePath1 = new FilePath(Path.GetTempFileName());
                uncompressedFilePath1 = new FilePath(Path.GetTempFileName());
                CompressWithDeflate(sourceFilePath.FullName, compressedFilePath1.FullName);
                UncompressWithDeflate_1(compressedFilePath1.FullName, uncompressedFilePath1.FullName);
                CompareFile(sourceFilePath, uncompressedFilePath1);

                uncompressedFilePath2 = new FilePath(Path.GetTempFileName());
                using (var inStream = UncompressWithDeflate_2(compressedFilePath1.FullName))
                using (var outStream = uncompressedFilePath2.Create().AsDotNetStream())
                {
                    inStream.CopyTo(outStream);
                }


            }
            finally
            {
                compressedFilePath1?.Delete();
                uncompressedFilePath1?.Delete();
                compressedFilePath2?.Delete();
                uncompressedFilePath2?.Delete();
                compressedFilePath3?.Delete();
                uncompressedFilePath3?.Delete();
                compressedFilePath4?.Delete();
                uncompressedFilePath4?.Delete();
            }
            CompressWithDeflate(uncompressedFilePath, compressedFilePath);

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
