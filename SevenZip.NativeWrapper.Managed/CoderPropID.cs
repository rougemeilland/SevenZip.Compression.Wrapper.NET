using System;

namespace SevenZip.NativeWrapper.Managed.win.x64
{
    enum CoderPropID
        : UInt32
    {
       DefaultProp = 0,
       DictionarySize,
       UsedMemorySize,
       Order,
       BlockSize,
       PosStateBits,
       LitContextBits,
       LitPosBits,
       NumFastBytes,
       MatchFinder,
       MatchFinderCycles,
       NumPasses,
       Algorithm,
       NumThreads,
       EndMarker,
       Level,
       ReduceSize,
       ExpectedDataSize,
       BlockSize2,
       CheckSize,
       Filter,
       MemUse,
       Affinity
    }
}
