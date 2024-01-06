using System;

namespace SevenZip.Compression
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
        /// <para>
        /// The type of the property value varies depending on the coder.
        /// </para>
        DefaultProp = 0,

        /// <summary>
        /// <para>
        /// Equivalent to the kDictionarySize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/> or <see cref="UInt64"/>. Which type the property value is depends on the coder.
        /// </para>
        /// </summary>
        DictionarySize,

        /// <summary>
        /// <para>
        /// Equivalent to the kUsedMemorySize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        UsedMemorySize,

        /// <summary>
        /// <para>
        /// Equivalent to the kOrder property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        Order,

        /// <summary>
        /// <para>
        /// Equivalent to the kBlockSize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/> or <see cref="UInt64"/>. Which type the property value is depends on the coder.
        /// </para>
        /// </summary>
        BlockSize,

        /// <summary>
        /// <para>
        /// Equivalent to the kPosStateBits property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        PosStateBits,

        /// <summary>
        /// <para>
        /// Equivalent to the kLitContextBits property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        LitContextBits,

        /// <summary>
        /// <para>
        /// Equivalent to the kLitPosBits property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        LitPosBits,

        /// <summary>
        /// <para>
        /// Equivalent to the kNumFastBytes property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        NumFastBytes,

        /// <summary>
        /// <para>
        /// Equivalent to the kMatchFinder property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="MatchFinderType"/>.
        /// </para>
        /// </summary>
        MatchFinder,

        /// <summary>
        /// <para>
        /// Equivalent to the kMatchFinderCycles property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        MatchFinderCycles,

        /// <summary>
        /// <para>
        /// Equivalent to the kNumPasses property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        NumPasses,

        /// <summary>
        /// <para>
        /// Equivalent to the kAlgorithm property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        Algorithm,

        /// <summary>
        /// <para>
        /// Equivalent to the kNumThreads property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        NumThreads,

        /// <summary>
        /// <para>
        /// Equivalent to the kEndMarker property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="Boolean"/>.
        /// </para>
        /// </summary>
        EndMarker,

        /// <summary>
        /// <para>
        /// Equivalent to the kLevel property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        Level,

        /// <summary>
        /// <para>
        /// Equivalent to the kReduceSize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt64"/>.
        /// </para>
        /// </summary>
        ReduceSize,

        /// <summary>
        /// <para>
        /// Equivalent to the kExpectedDataSize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt64"/>.
        /// </para>
        /// </summary>
        ExpectedDataSize,

        /// <summary>
        /// <para>
        /// Equivalent to the kBlockSize2 property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/> or <see cref="UInt64"/>. Which type the property value is depends on the coder.
        /// </para>
        /// </summary>
        BlockSize2,

        /// <summary>
        /// <para>
        /// Equivalent to the kCheckSize property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        CheckSize,

        /// <summary>
        /// <para>
        /// Equivalent to the kFilter property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="String"/>.
        /// </para>
        /// </summary>
        Filter,

        /// <summary>
        /// <para>
        /// Equivalent to the kMemUse property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt64"/>.
        /// </para>
        /// </summary>
        MemUse,

        /// <summary>
        /// <para>
        /// Equivalent to the kAffinity property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt64"/>.
        /// </para>
        /// </summary>
        Affinity,

        /// <summary>
        /// <para>
        /// Equivalent to the kBranchOffset property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        BranchOffset,

        /// <summary>
        /// <para>
        /// Equivalent to the kHashBits property of 7-zip.
        /// </para>
        /// <para>
        /// The property value type is <see cref="UInt32"/>.
        /// </para>
        /// </summary>
        HashBits,
    }
}
