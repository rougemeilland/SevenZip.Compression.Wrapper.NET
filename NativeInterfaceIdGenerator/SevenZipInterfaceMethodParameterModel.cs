﻿using System;

namespace NativeInterfaceIdGenerator
{
    internal class SevenZipInterfaceMethodParameterModel
    {
        public SevenZipInterfaceMethodParameterModel()
        {
            ParameterType = "";
            ParameterName = "";
            ParameterComment = "";
        }

        public String ParameterType { get; set; }
        public String ParameterName { get; set; }
        public String ParameterComment { get; set; }
    }
}
