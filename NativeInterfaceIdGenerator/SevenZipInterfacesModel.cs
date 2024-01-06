using System;

namespace NativeInterfaceIdGenerator
{
    internal class SevenZipInterfacesModel
    {
        public SevenZipInterfacesModel()
        {
            Interfaces = Array.Empty<SevenZipInterfaceModel>();
        }

        public SevenZipInterfaceModel[] Interfaces { get; set; }
    }
}
