using System;

namespace NativeInterfaceIdGenerator
{
    internal class SevenZipInterfaceModel
    {
        public SevenZipInterfaceModel()
        {
            InterfaceName = "";
            InterfaceId = "";
            BaseInerfaceName = "";
            Implemented = false;
            ProvidedOutward = false;
            ProvidedInward = false;
            Members = Array.Empty<SevenZipInterfaceMethodModel>();
        }

        public String InterfaceName { get; set; }
        public String InterfaceId { get; set; }
        public String BaseInerfaceName { get; set; }
        public Boolean Implemented { get; set; }
        public Boolean ProvidedOutward { get; set; }
        public Boolean ProvidedInward { get; set; }
        public Boolean CanQuery { get; set; }
        public SevenZipInterfaceMethodModel[] Members { get; set; }
    }
}
