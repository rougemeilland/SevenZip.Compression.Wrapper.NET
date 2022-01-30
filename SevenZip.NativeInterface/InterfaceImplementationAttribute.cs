using System;

namespace SevenZip.NativeInterface
{
    /// <summary>
    /// A class of attributes that can specify the implementation status and usage of the interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class InterfaceImplementationAttribute
        : Attribute
    {
        /// <summary>
        /// Initialize the attribute by specifying the <see cref="UsageType"/> value.
        /// </summary>
        /// <param name="usageType">
        /// <see cref="UsageType"/> value used for initialization.
        /// The default value is <see cref="InterfaceImplementarionType.None"/>.
        /// </param>
        public InterfaceImplementationAttribute(InterfaceImplementarionType usageType = InterfaceImplementarionType.None)
        {
            UsageType = usageType;
        }

        /// <summary>
        /// The <see cref="UsageType"/> value set in the attribute object.
        /// </summary>
        public InterfaceImplementarionType UsageType { get; }
    }
}
