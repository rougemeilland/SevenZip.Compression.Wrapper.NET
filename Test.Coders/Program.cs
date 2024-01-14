using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palmtree;
using Palmtree.Debug.IO;
using Palmtree.IO;
using SevenZip.Compression;
using SevenZip.Compression.Bzip2;
using SevenZip.Compression.Deflate;
using SevenZip.Compression.Deflate64;
using SevenZip.Compression.Lzma;
using SevenZip.Compression.Lzma2;
using SevenZip.Compression.Ppmd7;

namespace Test.Coders
{
    internal class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>")]
        public static void Main(string[] args)
        {
            // TODO: デッドコード削減の結果の検証。エンコーダ/デコーダのオプショナルなインターフェースのテストも忘れずに。
            const ulong DATA_SIZE = 64 * 1024LU;

            Console.WriteLine("自己診断中…");
            TestStore(DATA_SIZE);

            Console.WriteLine("BZIP2 のテスト中…");
            TestBzip2(DATA_SIZE);

            Console.WriteLine("BZIP2 (ストリーム) のテスト中…");
            TestBzip2Stream(DATA_SIZE);

            Console.WriteLine("Deflate のテスト中…");
            TestDeflate(DATA_SIZE);

            Console.WriteLine("Deflate (ストリーム) のテスト中…");
            TestDeflateStream(DATA_SIZE);

            Console.WriteLine("Deflate64 のテスト中…");
            TestDeflate64(DATA_SIZE);

            Console.WriteLine("Deflate64 (ストリーム) のテスト中…");
            TestDeflate64Stream(DATA_SIZE);

            Console.WriteLine("LZMA のテスト中…");
            TestLzma(DATA_SIZE);

            Console.WriteLine("LZMA (ストリーム) のテスト中…");
            TestLzmaStream(DATA_SIZE);

            Console.WriteLine("LZMA2 のテスト中…");
            TestLzma2(DATA_SIZE);

            Console.WriteLine("LZMA2 (ストリーム) のテスト中…");
            TestLzma2Stream(DATA_SIZE);

            Console.WriteLine("PPMd7 のテスト中…");
            TestPpmd7(DATA_SIZE);

            Console.WriteLine("PPMd7 (ストリーム) のテスト中…");
            TestPpmd7Stream(DATA_SIZE);

            Console.WriteLine("完了しました。");
            Console.Beep();
            _ = Console.ReadLine();
        }

        private static void TestStore(ulong DATA_SIZE)
        {
            var exception = (Exception?)null;
            using var inStream = InputTestDataStream.Create(DATA_SIZE);
            using var outStream = OutputTestDataStream.Create(ex => exception = ex);
            inStream.CopyTo(outStream);
            if (exception is not null)
                throw exception;
        }

        private static void TestBzip2(ulong DATA_SIZE)
        {
            var properties = new Bzip2EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Bzip2Encoder.CreateEncoder(properties);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    using var decoder = Bzip2Decoder.CreateDecoder();
                    decoder.Code(inStream, outStream, null, null, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestBzip2Stream(ulong DATA_SIZE)
        {
            var properties = new Bzip2EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Bzip2Encoder.CreateEncoder(properties);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = Bzip2Decoder.CreateDecoderStream(pipe.OpenInputStream(), DATA_SIZE);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestDeflate(ulong DATA_SIZE)
        {
            var properties = new DeflateEncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = DeflateEncoder.CreateEncoder(properties);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    using var decoder = DeflateDecoder.CreateDecoder();
                    decoder.Code(inStream, outStream, null, null, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestDeflateStream(ulong DATA_SIZE)
        {
            var properties = new DeflateEncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = DeflateEncoder.CreateEncoder(properties);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = DeflateDecoder.CreateDecoderStream(pipe.OpenInputStream(), DATA_SIZE);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestDeflate64(ulong DATA_SIZE)
        {
            var properties = new Deflate64EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Deflate64Encoder.CreateEncoder(properties);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    using var decoder = Deflate64Decoder.CreateDecoder();
                    decoder.Code(inStream, outStream, null, null, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestDeflate64Stream(ulong DATA_SIZE)
        {
            var properties = new Deflate64EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var encoder = Deflate64Encoder.CreateEncoder(properties);
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = Deflate64Decoder.CreateDecoderStream(pipe.OpenInputStream(), DATA_SIZE);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestLzma(ulong DATA_SIZE)
        {
            var properties = new LzmaEncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
                MatchFinder = MatchFinderType.BT4,
                EndMarker = true,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = LzmaEncoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
                    if (inStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var decoder = LzmaDecoder.CreateDecoder(contentProperties);
                    decoder.Code(inStream, outStream, null, null, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestLzmaStream(ulong DATA_SIZE)
        {
            var properties = new LzmaEncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
                MatchFinder = MatchFinderType.BT4,
                EndMarker = true,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = LzmaEncoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    var baseOfInStream = pipe.OpenInputStream();
                    Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
                    if (baseOfInStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var inStream = LzmaDecoder.CreateDecoderStream(baseOfInStream, DATA_SIZE, contentProperties);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestLzma2(ulong DATA_SIZE)
        {
            var properties = new Lzma2EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
                MatchFinder = MatchFinderType.BT4,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Lzma2Encoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    Span<byte> contentProperties = stackalloc byte[Lzma2Decoder.CONTENT_PROPERTY_SIZE];
                    if (inStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var decoder = Lzma2Decoder.CreateDecoder(contentProperties);
                    decoder.Code(inStream, outStream, null, null, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestLzma2Stream(ulong DATA_SIZE)
        {
            var properties = new Lzma2EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
                MatchFinder = MatchFinderType.BT4,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Lzma2Encoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    var baseOfInStream = pipe.OpenInputStream();
                    Span<byte> contentProperties = stackalloc byte[Lzma2Decoder.CONTENT_PROPERTY_SIZE];
                    if (baseOfInStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var inStream = Lzma2Decoder.CreateDecoderStream(baseOfInStream, DATA_SIZE, contentProperties);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestPpmd7(ulong DATA_SIZE)
        {
            var properties = new Ppmd7EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Ppmd7Encoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    using var inStream = pipe.OpenInputStream();
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    Span<byte> contentProperties = stackalloc byte[Ppmd7Decoder.CONTENT_PROPERTY_SIZE];
                    if (inStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var decoder = Ppmd7Decoder.CreateDecoder(contentProperties);
                    decoder.Code(inStream, outStream, null, DATA_SIZE, null);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }

        private static void TestPpmd7Stream(ulong DATA_SIZE)
        {
            var properties = new Ppmd7EncoderProperties
            {
                NumThreads = (uint)Environment.ProcessorCount,
                Level = CompressionLevel.Level9,
            };

            var pipe = new InProcessPipe();

            var exception = (Exception?)null;
            var encodingTask =
                Task.Run(() =>
                {
                    using var inStream = InputTestDataStream.Create(DATA_SIZE);
                    using var outStream = pipe.OpenOutputStream();
                    using var encoder = Ppmd7Encoder.CreateEncoder(properties);
                    encoder.WriteCoderProperties(outStream);
                    encoder.Code(inStream, outStream, null, null, null);
                });

            var decodingTask =
                Task.Run(() =>
                {
                    var baseOfInStream = pipe.OpenInputStream();
                    Span<byte> contentProperties = stackalloc byte[Ppmd7Decoder.CONTENT_PROPERTY_SIZE];
                    if (baseOfInStream.ReadBytes(contentProperties) != contentProperties.Length)
                        throw new Exception();
                    using var inStream = Ppmd7Decoder.CreateDecoderStream(baseOfInStream, DATA_SIZE, contentProperties);
                    using var outStream = OutputTestDataStream.Create(ex => exception = ex);
                    inStream.CopyTo(outStream);
                });

            Task.WhenAll(encodingTask, decodingTask).Wait();
            if (exception is not null)
                throw exception;
        }
    }
}
