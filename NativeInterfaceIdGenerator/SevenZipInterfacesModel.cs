using System;

namespace NativeInterfaceIdGenerator
{
    internal sealed class SevenZipInterfacesModel
    {
        public SevenZipInterfacesModel()
        {
            Interfaces = Array.Empty<SevenZipInterfaceModel>();
        }

        public SevenZipInterfaceModel[] Interfaces { get; set; }
    }
}
