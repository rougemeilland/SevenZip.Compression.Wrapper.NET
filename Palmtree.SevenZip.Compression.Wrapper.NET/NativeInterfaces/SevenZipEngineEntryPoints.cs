using System.Runtime.InteropServices;

namespace SevenZip.Compression.NativeInterfaces
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SevenZipEngineEntryPoints
    {
        /* 00 */
        public void* FpCreateDecoder;

        /* 01 */
        public void* FpCreateEncoder;

        /* 02 */
        public void* FpCreateObject;

        /* 03 */
        public void* FpGetHandlerProperty;

        /* 04 */
        public void* FpGetHandlerProperty2;

        /* 05 */
        public void* FpGetHashers;

        /* 06 */
        public void* FpGetMethodProperty;

        /* 07 */
        public void* FpGetNumberOfFormats;

        /* 08 */
        public void* FpGetNumberOfMethods;

        /* 09 */
        public void* FpGetModuleProp;
    }
}
