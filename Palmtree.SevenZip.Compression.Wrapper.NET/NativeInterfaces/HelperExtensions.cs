using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Palmtree;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal static class HelperExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception GetExceptionFromHRESULT(this HRESULT result)
        {
            Validation.Assert(result != HRESULT.S_OK, "result != HRESULT.S_OK");
            if (result == HRESULT.S_FALSE)
                return new Exception("Error detected.");
            var exception = Marshal.GetExceptionForHR((Int32)result);
            Validation.Assert(exception is not null, "exception is not null");
            return exception;
        }

        public static unsafe VALUE_T* ToPointer<VALUE_T>(this VALUE_T? value, VALUE_T* buffer)
            where VALUE_T : unmanaged
        {
            if (value is null)
                return null;
            *buffer = value.Value;
            return buffer;
        }

        public static NativeInStreamReader FromInputStreamToNativeDelegate(this ISequentialInputByteStream inputStream)
        {
            return nativeReader;

            HRESULT nativeReader(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        var length = inputStream.Read(new Span<Byte>(buffer.ToPointer(), checked((Int32)size)));
                        processedSize = checked((UInt32)length.Maximum(0));
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
        }

        public static NativeOutStreamWriter FromOutputStreamToNativeDelegate(this ISequentialOutputByteStream outputStream)
        {
            return nativeWriter;

            HRESULT nativeWriter(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        var length = outputStream.Write(new ReadOnlySpan<Byte>(buffer.ToPointer(), checked((Int32)size)));
                        processedSize = checked((UInt32)length.Maximum(0));
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
        }

        public static NativeInStreamReader FromInputStreamToNativeDelegate(this Stream inputStream)
        {
            return nativeReader;

            HRESULT nativeReader(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        var length = inputStream.Read(new Span<Byte>(buffer.ToPointer(), checked((Int32)size)));
                        processedSize = checked((UInt32)length.Maximum(0));
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
        }

        public static NativeOutStreamWriter FromOutputStreamToNativeDelegate(this Stream outputStream)
        {
            return nativeWriter;

            HRESULT nativeWriter(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        outputStream.Write(new ReadOnlySpan<Byte>(buffer.ToPointer(), checked((Int32)size)));
                        processedSize = size;
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
        }

        public static NativeProgressReporter? FromProgressToNativeDelegate(this IProgress<(UInt64? inSize, UInt64? outSize)>? progress)
        {
            if (progress is null)
                return null;

            void nativeReporter(IntPtr inSize, IntPtr outSize)
            {
                try
                {
                    unsafe
                    {
                        progress.Report(
                            (
                                inSize == IntPtr.Zero ? null : *(UInt64*)inSize.ToPointer(),
                                outSize == IntPtr.Zero ? null : *(UInt64*)outSize.ToPointer()
                            ));
                    }
                }
                catch (Exception)
                {
                }
            }

            return nativeReporter;
        }
    }
}
