using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal enum MethodPropID
        : UInt32
    {
        ID,
        Name,
        Decoder,
        Encoder,
        PackStreams,
        UnpackStreams,
        Description,
        DecoderIsAssigned,
        EncoderIsAssigned,
        DigestSize,
        IsFilter
    }
}
