#include "ManagedCompressSetInStream.h"
#include "NativeSequentialInStream.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetInStream::ManagedCompressSetInStream()
            {
                _inStream = nullptr;
            }

            ManagedCompressSetInStream::ManagedCompressSetInStream(const ManagedCompressSetInStream% p)
            {
                _inStream = p._inStream;
                if (_inStream != nullptr)
                    _inStream->AddRef();
            }

            ManagedCompressSetInStream::~ManagedCompressSetInStream()
            {
                this->!ManagedCompressSetInStream();
            }

            ManagedCompressSetInStream::!ManagedCompressSetInStream()
            {
                try
                {
                    ReleaseInStream();
                }
                catch (System::Exception^)
                {
                }
            }

            ManagedCompressSetInStream^ ManagedCompressSetInStream::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetInStream^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetInStream();
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
                            managedInterfaceObject->~ManagedCompressSetInStream();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetInStream::SetInStream(NativeInterface::IO::SequentialInStreamReader^ sequentialInStreamReader)
            {
                bool success = false;
                try
                {
                    ReleaseInStream();

                    _inStream = new IO::NativeSequentialInStream(sequentialInStreamReader);
                    _inStream->AddRef();
                    HRESULT result = GetNativeInterfaceObject()->SetInStream(_inStream);
                    if (result != S_OK)
                        __ThrowExceptionForHR(result);
                    success = true;
                }
                finally
                {
                    if (!success)
                        ReleaseInStream();
                }
            }

            void ManagedCompressSetInStream::ReleaseInStream()
            {
                if (_inStream != nullptr)
                {
                    try
                    {
                        try
                        {
                            HRESULT result = GetNativeInterfaceObject()->ReleaseInStream();
                            if (result != S_OK)
                                __ThrowExceptionForHR(result);
                        }
                        finally
                        {
                            _inStream->Release();
                        }
                    }
                    finally
                    {
                        _inStream = nullptr;
                    }
                }
            }
        }
    }
}
