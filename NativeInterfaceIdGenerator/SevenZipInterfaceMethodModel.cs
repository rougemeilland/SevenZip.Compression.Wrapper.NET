using System;

namespace NativeInterfaceIdGenerator
{
    internal class SevenZipInterfaceMethodModel
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

        public String MemberName { get; set; }
        public String ReturnValueType { get; set; }
        public Boolean IsCustomizedParameter { get; set; }
        public SevenZipInterfaceMethodParameterModel[] Parameters7z { get; set; }
        public SevenZipInterfaceMethodParameterModel[] ParametersCpp { get; set; }
        public SevenZipInterfaceMethodParameterModel[] ParametersCSharp { get; set; }
        public String MemberComment { get; set; }
        public String MemberAdditionalComment { get; set; }
        public String ReturnValueComment { get; set; }
    }
}
