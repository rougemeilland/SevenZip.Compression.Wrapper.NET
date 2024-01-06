using System;
using Palmtree.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// It is an interface that can read the remaining data after processing <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> from the input stream.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>Commenrs (English)</term>
    /// <description>
    /// <para>
    /// <see cref="ICompressReadUnusedFromInBuf"/> is supported by <see cref="ICompressCoder"/> object call <see cref="ReadUnusedFromInBuf(Span{Byte})"/> after <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/>.
    /// </para>
    /// <para>
    /// <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> decodes data, and the <see cref="ICompressCoder"/> object is allowed  to read from inStream to internal buffers more data than minimal data required for decoding.
    /// </para>
    /// <para>
    /// So we can call <see cref="ReadUnusedFromInBuf(Span{Byte})"/> from same <see cref="ICompressCoder"/> object to read unused input  data from the internal buffer.
    /// </para>
    /// <para>
    /// in <see cref="ReadUnusedFromInBuf(Span{Byte})"/>: the Coder is not allowed to use (<see cref="ISequentialInputByteStream"/> inStream) object, that was sent to <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/>.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>コメント (日本語)</term>
    /// <description>
    /// <para>
    /// <see cref="ICompressReadUnusedFromInBuf"/> は <see cref="ICompressCoder"/> オブジェクトによってサポートされています。
    /// <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> の後に <see cref="ReadUnusedFromInBuf(Span{Byte})"/> を呼び出します。
    /// </para>
    /// <para>
    /// <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> はデータをデコードします。
    /// </para>
    /// <para>
    /// <see cref="ICompressCoder"/> オブジェクトは、デコードに必要な最小限のデータよりも多くのデータを inStream から内部バッファーに読み取ることができます。
    /// </para>
    /// <para>
    /// したがって、同じ <see cref="ICompressCoder"/> オブジェクトから <see cref="ReadUnusedFromInBuf(Span{Byte})"/> を呼び出して、内部バッファから未使用の入力データを読み取ることができます。
    /// </para>
    /// <para>
    /// <see cref="ReadUnusedFromInBuf(Span{Byte})"/> 内で、コーダーは <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> に送信された (<see cref="ISequentialInputByteStream"/> inStream) オブジェクトを使用することを許可されていません。
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public interface ICompressReadUnusedFromInBuf
    {
        /// <summary>
        /// Reads the remaining data after processing <see cref="ICompressCoder.Code(ISequentialInputByteStream, ISequentialOutputByteStream, UInt64?, UInt64?, IProgress{ValueTuple{UInt64?, UInt64?}}?)"/> from the input stream.
        /// </summary>
        /// <param name="data">
        /// Set a buffer to store the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// </returns>
        UInt32 ReadUnusedFromInBuf(Span<Byte> data);
    }
}
