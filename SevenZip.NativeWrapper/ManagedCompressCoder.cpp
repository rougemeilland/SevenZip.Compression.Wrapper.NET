#include "ManagedCompressCoder.h"
#include "ClrInterOperation.h"
#include "NativeSequentialInStream.h"
#include "NativeSequentialOutStream.h"
#include "NativeCompressProgressInfo.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressCoder^ ManagedCompressCoder::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressCoder^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressCoder();
                    if (!managedInterfaceObject->AttatchNativeInterfaceObject(nativeUnknownObject))
                        return nullptr;
                    success = true;
                    return managedInterfaceObject;
                }
                catch (System::Exception^ ex)
                {
                    if (!success)
                    {
                        if (managedInterfaceObject != nullptr)
                            managedInterfaceObject->~ManagedCompressCoder();
                    }
                    throw ex;
                }
            }

            void ManagedCompressCoder::Code(SevenZip::NativeInterface::IO::SequentialInStreamReader^ sequentialInStreamReader, SevenZip::NativeInterface::IO::SequentialOutStreamWriter^ sequentialOutStreamWriter, System::Nullable<UInt64> inSize, System::Nullable<UInt64> outSize, SevenZip::NativeInterface::Compression::CompressProgressInfoReporter^ progressReporter)
            {
                IO::NativeSequentialInStream inStream(sequentialInStreamReader);
                inStream.AddRef(); // Call AddRef() to avoid deleting the auto variable
                IO::NativeSequentialOutStream outStream(sequentialOutStreamWriter);
                outStream.AddRef(); // Call AddRef() to avoid deleting the auto variable
                UInt64 inSizeBuffer;
                UInt64 outSizeBuffer;
                if (progressReporter == nullptr)
                {
                    HRESULT result = GetNativeInterfaceObject()->Code(&inStream, &outStream, FromNullableUInt64ToUInt64Pointer(inSize, &inSizeBuffer), FromNullableUInt64ToUInt64Pointer(outSize, &outSizeBuffer), nullptr);
                    if (result != S_OK)
                        __ThrowExceptionForHR(result);
                }
                else
                {
                    NativeCompressProgressInfo progress(progressReporter);
                    progress.AddRef(); // Call AddRef() to avoid deleting the auto variable
                    HRESULT result = GetNativeInterfaceObject()->Code(&inStream, &outStream, FromNullableUInt64ToUInt64Pointer(inSize, &inSizeBuffer), FromNullableUInt64ToUInt64Pointer(outSize, &outSizeBuffer), &progress);
                    if (result != S_OK)
                        __ThrowExceptionForHR(result);
                }
            }
        }
    }
}
