﻿using System;

namespace SevenZip.Compression.Ppmd7
{
    /// <summary>
    /// A container class for PPMd7 (PPMd version H) decoder properties.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Ppmd7DecoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Ppmd7DecoderProperties()
        {
            FinishMode = null;
        }

        /// <summary>
        /// <para>
        /// In decoding, it means whether to decode the entire input stream (full_decoding mode) or only part of the input stream (partial_decoding mode).
        /// </para>
        /// <para>
        /// The default value is null, which means partial_decoding mode.
        /// If you want to change this behavior, set the following values. :
        /// <list type="bullet">
        /// <item>Set true for full_decoding mode.</item>
        /// <item>Set false for partial_decoding mode.</item>
        /// </list>
        /// </para>
        /// </summary>
        public Boolean? FinishMode { get; set; }
    }
}
