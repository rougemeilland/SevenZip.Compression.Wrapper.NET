using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SevenZip.NativeWrapper.Managed
{
    static class HelperExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception GetExceptionFromHRESULT(this HRESULT result)
        {
            if (result == HRESULT.S_OK)
                throw new ArgumentException("Must not be 'result == S_OK'.", nameof(result));
            var exception = Marshal.GetExceptionForHR((Int32)result);
            if (exception is null)
                throw new Exception("Code that should be unreachable has been executed.");
            return exception;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UInt16 ToUInt16LE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt16))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            var value = (UInt32)buffer[1];
            value = (value << 8) | buffer[0];
            return (UInt16)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16BE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt16))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            var value = (UInt32)buffer[0];
            value = (value << 8) | buffer[1];
            return (UInt16)value;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32LE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt32))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            unsafe
            {
                fixed (Byte* p = buffer)
                {
                    return *(UInt32*)p;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32BE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt32))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            var value = (UInt32)buffer[0];
            value = (value << 8) | buffer[1];
            value = (value << 8) | buffer[2];
            value = (value << 8) | buffer[3];
            return (UInt16)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64LE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt64))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            unsafe
            {
                fixed (Byte* p = buffer)
                {
                    return *(UInt64*)p;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt64BE(this ReadOnlySpan<Byte> buffer)
        {
            if (buffer.Length < sizeof(UInt64))
                throw new ArgumentException($"The size of the specified buffer is {sizeof(UInt16)} bytes, but smaller than the required size ({buffer.Length} bytes).");
            var value = (UInt32)buffer[0];
            value = (value << 8) | buffer[1];
            value = (value << 8) | buffer[2];
            value = (value << 8) | buffer[3];
            value = (value << 8) | buffer[4];
            value = (value << 8) | buffer[5];
            value = (value << 8) | buffer[6];
            value = (value << 8) | buffer[7];
            return (UInt16)value;
        }

        public static unsafe VALUE_T* ToPointer<VALUE_T>(this VALUE_T? value, VALUE_T* buffer)
            where VALUE_T : unmanaged
        {
            if (!value.HasValue)
                return null;
            *buffer = value.Value;
            return buffer;
        }

        public static NativeInStreamReader ToNativeDelegate(this NativeInterface.IO.SequentialInStreamReader reader)
        {
            HRESULT nativeReader(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        var length = reader(new Span<Byte>(buffer.ToPointer(), (Int32)size));
                        processedSize = length >= 0 ? (UInt32)length : 0;
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
            return nativeReader;
        }

        public static NativeOutStraamWriter ToNativeDelegate(this NativeInterface.IO.SequentialOutStreamWriter writer)
        {
            HRESULT nativeWriter(IntPtr buffer, UInt32 size, out UInt32 processedSize)
            {
                try
                {
                    if (size > Int32.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(size));
                    unsafe
                    {
                        var length = writer(new Span<Byte>(buffer.ToPointer(), (Int32)size));
                        processedSize = length >= 0 ? (UInt32)length : 0;
                        return HRESULT.S_OK;
                    }
                }
                catch (Exception ex)
                {
                    processedSize = 0;
                    return (HRESULT)ex.HResult;
                }
            }
            return nativeWriter;
        }

        public static NativeProgressReporter ToNativeDelegate(this NativeInterface.Compression.CompressProgressInfoReporter reporter)
        {
            void nativeReporter(IntPtr inSize, IntPtr outSize)
            {
                try
                {
                    unsafe
                    {
                        reporter(
                            inSize == IntPtr.Zero ? null : *(UInt64*)inSize.ToPointer(),
                            outSize == IntPtr.Zero ? null : *(UInt64*)outSize.ToPointer());
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