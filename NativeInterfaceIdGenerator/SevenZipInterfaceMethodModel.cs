namespace NativeInterfaceIdGenerator
{
    class SevenZipInterfaceMethodModel
    {
        public SevenZipInterfaceMethodModel()
        {
            MemberName = "";
            ReturnValueType = "";
            IsCustomizedParameter = false;
            Parameters7z = System.Array.Empty<SevenZipInterfaceMethodParameterModel>();
            ParametersCpp = System.Array.Empty<SevenZipInterfaceMethodParameterModel>();
            ParametersCSharp = System.Array.Empty<SevenZipInterfaceMethodParameterModel>();
            MemberComment = "";
            MemberAdditionalComment = "";
            ReturnValueComment = "";
        }

        public string MemberName { get; set; }
        public string ReturnValueType { get; set; }
        public bool IsCustomizedParameter { get; set; }
        public SevenZipInterfaceMethodParameterModel[] Parameters7z { get; set; }
        public SevenZipInterfaceMethodParameterModel[] ParametersCpp { get; set; }
        public SevenZipInterfaceMethodParameterModel[] ParametersCSharp { get; set; }
        public string MemberComment { get; set; }
        public string MemberAdditionalComment { get; set; }
        public string ReturnValueComment { get; set; }
    }
}
