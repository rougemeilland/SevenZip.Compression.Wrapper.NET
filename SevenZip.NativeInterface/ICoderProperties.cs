using SevenZip.NativeInterface.Compression;
using System;
using System.Collections.Generic;

namespace SevenZip.NativeInterface
{
    /// <summary>
    /// An interface that provides various properties to the coder.
    /// </summary>
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByExternalCode)]
    public interface ICoderProperties
    {
        /// <summary>
        /// Enumerates ID/value pairs for coder properties.
        /// However, properties that have a default value (null) are excluded from the enumeration.
        /// </summary>
        /// <returns>
        /// An enumerator of ID/value pairs for coder properties.
        /// </returns>
        /// <remarks>
        /// The value is represented by the object type.
        /// The value must be of one of the following types:
        /// <list type="bullet">
        /// <item><see cref="Boolean"/></item>
        /// <item><see cref="String"/></item>
        /// <item><see cref="UInt32"/></item>
        /// <item><see cref="UInt64"/></item>
        /// <item><see cref="DateTime"/> (Must be <see cref="DateTime.Kind"/> != <see cref="DateTimeKind.Unspecified"/> ) </item>
        /// <item><see cref="DateTimeOffset"/></item>
        /// <item><see cref="MatchFinderType"/></item>
        /// </list>
        /// </remarks>
        IEnumerable<(CoderPropertyId propertyId, object propertryValue)> EnumerateProperties();
    }
}
