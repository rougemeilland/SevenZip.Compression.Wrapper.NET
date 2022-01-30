using System;

namespace SevenZip.NativeInterface
{
    /// <summary>
    /// An enumeration that indicates the types of properties that can be set to or referenced by the coder.
    /// </summary>
    public enum CoderPropertyId
        : UInt32
    {
        /// <summary>
        /// Equivalent to the kDefaultProp property of 7-zip.
        /// </summary>
        DefaultProp = 0,

        /// <summary>
        /// Equivalent to the kDictionarySize property of 7-zip.
        /// </summary>
        DictionarySize,

        /// <summary>
        /// Equivalent to the kUsedMemorySize property of 7-zip.
        /// </summary>
        UsedMemorySize,

        /// <summary>
        /// Equivalent to the kOrder property of 7-zip.
        /// </summary>
        Order,

        /// <summary>
        /// Equivalent to the kBlockSize property of 7-zip.
        /// </summary>
        BlockSize,

        /// <summary>
        /// Equivalent to the kPosStateBits property of 7-zip.
        /// </summary>
        PosStateBits,

        /// <summary>
        /// Equivalent to the kLitContextBits property of 7-zip.
        /// </summary>
        LitContextBits,

        /// <summary>
        /// Equivalent to the kLitPosBits property of 7-zip.
        /// </summary>
        LitPosBits,

        /// <summary>
        /// Equivalent to the kNumFastBytes property of 7-zip.
        /// </summary>
        NumFastBytes,

        /// <summary>
        /// Equivalent to the kMatchFinder property of 7-zip.
        /// </summary>
        MatchFinder,

        /// <summary>
        /// Equivalent to the kMatchFinderCycles property of 7-zip.
        /// </summary>
        MatchFinderCycles,

        /// <summary>
        /// Equivalent to the kNumPasses property of 7-zip.
        /// </summary>
        NumPasses,

        /// <summary>
        /// Equivalent to the kAlgorithm property of 7-zip.
        /// </summary>
        Algorithm,

        /// <summary>
        /// Equivalent to the kNumThreads property of 7-zip.
        /// </summary>
        NumThreads,

        /// <summary>
        /// Equivalent to the kEndMarker property of 7-zip.
        /// </summary>
        EndMarker,

        /// <summary>
        /// Equivalent to the kLevel property of 7-zip.
        /// </summary>
        Level,

        /// <summary>
        /// Equivalent to the kReduceSize property of 7-zip.
        /// </summary>
        ReduceSize,

        /// <summary>
        /// Equivalent to the kExpectedDataSize property of 7-zip.
        /// </summary>
        ExpectedDataSize,

        /// <summary>
        /// Equivalent to the kBlockSize2 property of 7-zip.
        /// </summary>
        BlockSize2,

        /// <summary>
        /// Equivalent to the kCheckSize property of 7-zip.
        /// </summary>
        CheckSize,

        /// <summary>
        /// Equivalent to the kFilter property of 7-zip.
        /// </summary>
        Filter,

        /// <summary>
        /// Equivalent to the kMemUse property of 7-zip.
        /// </summary>
        MemUse,

        /// <summary>
        /// Equivalent to the kAffinity property of 7-zip.
        /// </summary>
        Affinity,
    }
}
