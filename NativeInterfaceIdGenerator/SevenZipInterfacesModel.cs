using System;

namespace NativeInterfaceIdGenerator
{
    class SevenZipInterfacesModel
    {
        public SevenZipInterfacesModel()
        {
            Interfaces = Array.Empty<SevenZipInterfaceModel>();
        }

        public SevenZipInterfaceModel[] Interfaces { get; set; }
    }
}
