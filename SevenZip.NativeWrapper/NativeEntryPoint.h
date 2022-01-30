#pragma once

#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        class NativeEntryPoint
        {
        private:
            typedef HRESULT(STDMETHODCALLTYPE* FuncCreateDecoder)(UInt32 index, const GUID* iid, void** outObject);
            typedef HRESULT(STDMETHODCALLTYPE* FuncCreateEncoder)(UInt32 index, const GUID* iid, void** outObject);
            typedef HRESULT(STDMETHODCALLTYPE* FuncCreateObject)(const GUID* clsid, const GUID* iid, void** outObject);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetHandlerProperty)(PROPID propID, PROPVARIANT* value);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetHandlerProperty2)(UInt32 formatIndex, PROPID propID, PROPVARIANT* value);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetHashers)(SevenZip::NativeWrapper::SevenZipInterface::IHashers** hashers);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetIsArc)(UInt32 formatIndex, SevenZip::NativeWrapper::SevenZipInterface::Func_IsArc* isArc);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetMethodProperty)(UInt32 codecIndex, PROPID propID, PROPVARIANT* value);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetNumberOfFormats)(UINT32* numFormats);
            typedef HRESULT(STDMETHODCALLTYPE* FuncGetNumberOfMethods)(UINT32* numCodecs);
            HINSTANCE _dllHandle;
            UInt32 _referenceCount;
            FuncCreateDecoder _fpCreateDecoder;
            FuncCreateEncoder _fpCreateEncoder;
            FuncCreateObject _fpCreateObject;
            FuncGetHandlerProperty _fpGetHandlerProperty;
            FuncGetHandlerProperty2 _fpGetHandlerProperty2;
            FuncGetHashers _fpGetHashers;
            FuncGetIsArc _fpGetIsArc;
            FuncGetMethodProperty _fpGetMethodProperty;
            FuncGetNumberOfFormats _fpGetNumberOfFormats;
            FuncGetNumberOfMethods _fpGetNumberOfMethods;
            NativeEntryPoint(HINSTANCE dllHandle);
            NativeEntryPoint(const NativeEntryPoint& p); // unused
        public:
            virtual ~NativeEntryPoint();
            static NativeEntryPoint* Create();
            HRESULT STDMETHODCALLTYPE CreateDecoder(UInt32 index, const GUID* iid, void** outObject);
            HRESULT STDMETHODCALLTYPE CreateEncoder(UInt32 index, const GUID* iid, void** outObject);
            HRESULT STDMETHODCALLTYPE CreateObject(const GUID& clsid, const GUID& iid, void** outObject);
            HRESULT STDMETHODCALLTYPE GetHandlerProperty(PROPID propID, PROPVARIANT* value);
            HRESULT STDMETHODCALLTYPE GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT* value);
            HRESULT STDMETHODCALLTYPE GetHashers(SevenZip::NativeWrapper::SevenZipInterface::IHashers** hashers);
            HRESULT STDMETHODCALLTYPE GetIsArc(UInt32 formatIndex, SevenZip::NativeWrapper::SevenZipInterface::Func_IsArc* isArc);
            HRESULT STDMETHODCALLTYPE GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT* value);
            HRESULT STDMETHODCALLTYPE GetNumberOfFormats(UINT32* numFormats);
            HRESULT STDMETHODCALLTYPE GetNumberOfMethods(UINT32* numCodecs);
            void AddRef();
            void Release();
        };
    }
}
