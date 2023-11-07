using SevenZip.NativeInterface.Compression;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SevenZip
{
    static class HelperExtensions
    {
        public static void ReadBytes(this Stream inStream, Span<Byte> buffer)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (!inStream.CanRead)
                throw new NotSupportedException();

            while (buffer.Length > 0)
            {
                var length = inStream.Read(buffer);
                if (length <= 0)
                    throw new IO.UnexpectedEndOfStreamException();
                buffer = buffer[length..];
            }
        }

        public static void ReadBytes(this IO.ISequentialInStream inStream, Span<Byte> buffer)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));

            while (buffer.Length > 0)
            {
                var length = inStream.Read(buffer);
                if (length <= 0)
                    throw new IO.UnexpectedEndOfStreamException();
                buffer = buffer[length..];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytes(this Stream outStream, ReadOnlySpan<Byte> data)
        {
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));
            if (!outStream.CanWrite)
                throw new NotSupportedException();

            outStream.Write(data);
        }

        public static void WriteBytes(this IO.ISequentialOutStream outStream, ReadOnlySpan<Byte> data)
        {
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));

            while (data.Length > 0)
            {
                var length = outStream.Write(data);
                if (length <= 0)
                    throw new IO.UnexpectedEndOfStreamException();
                data = data[length..];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VALUE_T Mamimum<VALUE_T>(this VALUE_T x, VALUE_T y)
            where VALUE_T : notnull, IComparable<VALUE_T>
        {
            return x.CompareTo(y) > 0 ? x : y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VALUE_T Minimum<VALUE_T>(this VALUE_T x, VALUE_T y)
            where VALUE_T : notnull, IComparable<VALUE_T>
        {
            return x.CompareTo(y) > 0 ? y : x;
        }

        public static void ReadBytes(this NativeInterface.IO.SequentialInStreamReader inStreamReader, Span<Byte> buffer)
        {
            if (inStreamReader is null)
                throw new ArgumentNullException(nameof(inStreamReader));

            while (buffer.Length > 0)
            {
                var length = inStreamReader(buffer);
                if (length <= 0)
                    throw new IO.UnexpectedEndOfStreamException();
                buffer = buffer[length..];
            }
        }

        public static void WriteBytes(this NativeInterface.IO.SequentialOutStreamWriter outStreamWriter, ReadOnlySpan<Byte> buffer)
        {
            while (buffer.Length > 0)
            {
                var length = outStreamWriter(buffer);
                if (length <= 0)
                    throw new IOException();
                buffer = buffer[length..];
            }
        }

        public static NativeInterface.IO.SequentialInStreamReader GetStreamReader(this IO.ISequentialInStream inStream)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));

            return buffer => inStream.Read(buffer);
        }

        public static NativeInterface.IO.SequentialInStreamReader GetStreamReader(this Stream inStream)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (!inStream.CanRead)
                throw new NotSupportedException();

            return buffer => inStream.Read(buffer);
        }

        public static NativeInterface.IO.SequentialOutStreamWriter GetStreamWriter(this IO.ISequentialOutStream outStream)
        {
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));

            return buffer => outStream.Write(buffer);
        }

        public static NativeInterface.IO.SequentialOutStreamWriter GetStreamWriter(this Stream outStream)
        {
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));
            if (!outStream.CanWrite)
                throw new NotSupportedException();

            return buffer =>
            {
                outStream.Write(buffer);
                return buffer.Length;
            };
        }

        public static CompressProgressInfoReporter? GetProgressReporter(this IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (progress is null)
                return null;
            return
                (inSize, outSize) =>
                {
                    try
                    {
                        progress.Report((inSize, outSize));
                    }
                    catch (Exception)
                    {
                    }
                };
        }

        public static IO.ISequentialInStream AsISequentialInStream(this Stream sourceStream)
        {
            if (sourceStream is null)
                throw new ArgumentNullException(nameof(sourceStream));
            if (!sourceStream.CanRead)
                throw new NotSupportedException();

            return new IO.WrapperStreamAsISequentialInStream(sourceStream);
        }

        public static IO.ISequentialOutStream AsISequentialOutStream(this Stream sourceStream)
        {
            if (sourceStream is null)
                throw new ArgumentNullException(nameof(sourceStream));
            if (!sourceStream.CanWrite)
                throw new NotSupportedException();

            return new IO.WrapperStreamAsISequentialOutStream(sourceStream);
        }

        public static Stream AsStream(this IO.ISequentialInStream sourceStream)
        {
            if (sourceStream is null)
                throw new ArgumentNullException(nameof(sourceStream));

            return new IO.WrapperISequentialInStreamAsStream(sourceStream);
        }

        public static Stream AsStream(this IO.ISequentialOutStream sourceStream)
        {
            if (sourceStream is null)
                throw new ArgumentNullException(nameof(sourceStream));

            return new IO.WrapperISequentialOutStreamAsStream(sourceStream);
        }
    }
}
