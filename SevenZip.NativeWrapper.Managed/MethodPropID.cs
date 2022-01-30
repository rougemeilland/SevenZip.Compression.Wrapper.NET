using System;

namespace SevenZip.NativeWrapper.Managed
{
    enum MethodPropID
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
       DigestSize
    }
}
