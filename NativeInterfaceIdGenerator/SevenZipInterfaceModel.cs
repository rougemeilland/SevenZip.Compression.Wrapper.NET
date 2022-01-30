namespace NativeInterfaceIdGenerator
{
    class SevenZipInterfaceModel
    {
        public SevenZipInterfaceModel()
        {
            InterfaceName = "";
            InterfaceId = "";
            BaseInerfaceName = "";
            Implemented = false;
            ProvidedOutward = false;
            ProvidedInward = false;
            Members = System.Array.Empty<SevenZipInterfaceMethodModel>();
        }

        public string InterfaceName { get; set; }
        public string InterfaceId { get; set; }
        public string BaseInerfaceName { get; set; }
        public bool Implemented { get; set; }
        public bool ProvidedOutward { get; set; }
        public bool ProvidedInward { get; set; }
        public bool CanQuery { get; set; }
        public SevenZipInterfaceMethodModel[] Members { get; set; }
    }
}
