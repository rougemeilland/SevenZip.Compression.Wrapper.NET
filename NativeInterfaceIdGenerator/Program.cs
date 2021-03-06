using SevenZip.NativeInterface;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace NativeInterfaceIdGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var solutionPath = GetSolutionPath();
            if (solutionPath is not null)
            {
                var sevenZipInterfacesModel = JsonSerializer.Deserialize<SevenZipInterfacesModel>(File.ReadAllText("SevenZipInterfaces.json"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (sevenZipInterfacesModel is null)
                    throw new Exception();

                //var s = JsonSerializer.Serialize(sevenZipInterfacesModel, new JsonSerializerOptions { PropertyNameCaseInsensitive = false, WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces)
                {
                    foreach (var member in sevenZipInterface.Members)
                    {
                        if (member.Parameters7z.Length > 0)
                        {
                            if (member.ParametersCpp.Length <= 0)
                                member.ParametersCpp = member.Parameters7z;
                        }
                        if (member.ParametersCpp.Length > 0)
                        {
                            if (member.ParametersCSharp.Length <= 0)
                                member.ParametersCSharp = member.ParametersCpp;
                        }
                    }
                }

                CheckInterface(sevenZipInterfacesModel);

                RenderInterfaceDefinitionsH(solutionPath, sevenZipInterfacesModel);
                RenderInterfaceDefinitionsCpp(solutionPath, sevenZipInterfacesModel);
                RenderSevenZipNativeWrapperUnmanagedH(solutionPath, sevenZipInterfacesModel);
                RenderSevenZipNativeWrapperUnmanagedCpp(solutionPath, sevenZipInterfacesModel);
                RenderSevenZipNativeWrapperUnmanagedCs(solutionPath, sevenZipInterfacesModel);
                RenderSevenZipNativeWrapperManagedUnknownCs(solutionPath, sevenZipInterfacesModel);
            }
            Console.WriteLine();
            Console.WriteLine("Complete");
            Console.Beep();
            Console.ReadLine();
        }

        private static string? GetSolutionPath()
        {
            var solutionPath = typeof(Program).Assembly.Location;
            while (solutionPath is not null && !string.Equals(Path.GetFileName(solutionPath), "bin", StringComparison.OrdinalIgnoreCase))
                solutionPath = Path.GetDirectoryName(solutionPath);
            solutionPath = Path.GetDirectoryName(solutionPath);
            if (solutionPath is null)
                return null;
            if (!string.Equals(Path.GetFileName(solutionPath), "NativeInterfaceIdGenerator", StringComparison.OrdinalIgnoreCase))
                return null;
            return Path.GetDirectoryName(solutionPath);
        }

        private static void CheckInterface(SevenZipInterfacesModel sevenZipInterfaces)
        {
            var iunknown = typeof(IUnknown);
            var iunknownName = iunknown.ToString();
            var definedInterfaces =
                iunknown.Assembly.GetTypes()
                .Where(type => type.IsInterface && type.IsPublic && (type.GetInterface(iunknownName) != null))
                .Where(type => Attribute.GetCustomAttributes(type).Any(attr => attr is GuidAttribute))
                .Select(type => new { type, typeInfo = Attribute.GetCustomAttributes(type).Select(attr => attr as InterfaceImplementationAttribute).SingleOrDefault(attr => attr is not null) })
                .Select(item => new { item.type, typeInfo = item.typeInfo is null ? InterfaceImplementarionType.None : item.typeInfo.UsageType })
                .Where(item => !item.typeInfo.HasFlag(InterfaceImplementarionType.NotImplemented))
                .Select(item => new
                {
                    name = item.type.Name,
                    id = item.type.GUID,
                    implementedByInternal = item.typeInfo.HasFlag(InterfaceImplementarionType.ImplementedByInternalCode),
                    implementedByExternal = item.typeInfo.HasFlag(InterfaceImplementarionType.ImplementedByExternalCode),
                })
                .ToDictionary(
                    item => item.id.ToString(),
                    item => (item.name, item.implementedByInternal, item.implementedByExternal),
                    StringComparer.OrdinalIgnoreCase);
            var expectedInterfaces =
                sevenZipInterfaces.Interfaces
                .Where(sevenZipInterface =>
                    !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal)
                    && sevenZipInterface.Implemented)
                .ToDictionary(
                    sevenZipInterface => sevenZipInterface.InterfaceId,
                    sevenZipInterface => 
                    (
                        name: sevenZipInterface.InterfaceName,
                        implementedByInternal: sevenZipInterface.ProvidedOutward,
                        implementedByExternal: sevenZipInterface.ProvidedInward
                    ),
                    StringComparer.OrdinalIgnoreCase);
            foreach (var item in definedInterfaces)
            {
                if (!expectedInterfaces.TryGetValue(item.Key, out (string name, bool implementedByInternal, bool implementedByExternal) value))
                    throw new Exception($"The interface \"{item.Value.name}\" ({item.Key}) is defined but does not exist in the database.");
                if (!string.Equals(value.name, item.Value.name, StringComparison.Ordinal))
                    throw new Exception($"The name of the interface with ID \"{item.Key}\" is different. : <defined> = \"{item.Value.name}\", <database> = \"{value.name}\"");
                if (value.implementedByExternal!= item.Value.implementedByExternal)
                    throw new Exception($"The implementedByExternal value of the interface named \"{item.Value.name}\" is different. : <defined> = \"{item.Value.implementedByExternal}\", <database> = \"{value.implementedByExternal}\"");
                if (value.implementedByInternal != item.Value.implementedByInternal)
                    throw new Exception($"The implementedByInternal value of the interface named \"{item.Value.name}\" is different. : <defined> = \"{item.Value.implementedByInternal}\", <database> = \"{value.implementedByInternal}\"");
            }

            foreach (var item in expectedInterfaces)
            {
                if (!definedInterfaces.TryGetValue(item.Key, out (string name, bool implementedByInternal, bool implementedByExternal) value))
                    throw new Exception($"Interface \"{item.Value.name}\" ({item.Key}) exists in the database but is not defined.");
                if (!string.Equals(value.name, item.Value.name, StringComparison.Ordinal))
                    throw new Exception($"The name of the interface with ID \"{item.Key}\" is different. : <defined> = \"{value.name}\", <database> = \"{item.Value.name}\"");
                if (value.implementedByExternal != item.Value.implementedByExternal)
                    throw new Exception($"The implementedByExternal value of the interface named \"{item.Value.name}\" is different. : <defined> = \"{value.implementedByExternal}\", <database> = \"{item.Value.implementedByExternal}\"");
                if (value.implementedByInternal != item.Value.implementedByInternal)
                    throw new Exception($"The implementedByInternal value of the interface named \"{item.Value.name}\" is different. : <defined> = \"{value.implementedByInternal}\", <database> = \"{item.Value.implementedByInternal}\"");
            }

        }

        private static void RenderInterfaceDefinitionsH(string solutionPath, SevenZipInterfacesModel sevenZipInterfaces)
        {
            using var writer = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper", "InterfaceDefinitions.h"));
            writer.WriteLine("// This file is automatically generated. Do not rewrite manually.");
            writer.WriteLine();
            writer.WriteLine(@"#pragma once");
            writer.WriteLine();
            writer.WriteLine(@"#include ""Platform.h""");
            writer.WriteLine();
            foreach (var sevenZipInterface in sevenZipInterfaces.Interfaces.Where(sevenZipInterface => !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal)))
                writer.WriteLine($"#define IIDS_{sevenZipInterface.InterfaceName} \"{{{sevenZipInterface.InterfaceId}}}\"");
            writer.WriteLine();
            writer.WriteLine("namespace SevenZip");
            writer.WriteLine("{");
            writer.WriteLine("    namespace NativeWrapper");
            writer.WriteLine("    {");
            writer.WriteLine("        SevenZip::NativeInterface::IUnknown^ CreateManagedInterfaceObject(const GUID& interfaceId, IUnknown* interfaceObject);");
            writer.WriteLine("#ifdef _DEBUG");
            writer.WriteLine("        void ValidateInterfaceIds();");
            writer.WriteLine("#endif");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        private static void RenderInterfaceDefinitionsCpp(string solutionPath, SevenZipInterfacesModel sevenZipInterfaces)
        {
            using (var writer = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper", "InterfaceDefinitions.cpp")))
            {
                var interfaceCreatorFinderSource =
                    sevenZipInterfaces.Interfaces
                    .Where(sevenZipInterface => !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal) && sevenZipInterface.Implemented && sevenZipInterface.ProvidedOutward && sevenZipInterface.CanQuery)
                    .Select(sevenZipInterface =>
                    (
                        name: sevenZipInterface.InterfaceName,
                        guidByteArray: Guid.Parse(sevenZipInterface.InterfaceId).ToByteArray(),
                        guid: Guid.Parse(sevenZipInterface.InterfaceId
                    )));
                writer.WriteLine("// This file is automatically generated. Do not rewrite manually.");
                writer.WriteLine();
                writer.WriteLine(@"#include ""InterfaceDefinitions.h""");
                writer.WriteLine(@"#include ""Guid.h""");
                writer.WriteLine(@"#include ""SevenZipInterface.h""");
                writer.WriteLine();
                foreach (var sevenZipInterface in sevenZipInterfaces.Interfaces)
                    writer.WriteLine($"{((!sevenZipInterface.Implemented || !sevenZipInterface.ProvidedOutward) ? "// " : "")}#include \"Managed{sevenZipInterface.InterfaceName[1..]}.h\"");
                writer.WriteLine();
                writer.WriteLine("#ifndef ENABLE_MSVC_SPECIFIC_SPECIFICATIONS");
                foreach (var sevenZipInterface in sevenZipInterfaces.Interfaces)
                {
                    var interfaceId = Guid.Parse(sevenZipInterface.InterfaceId).ToByteArray();
                    writer.WriteLine($"GUID IID_{sevenZipInterface.InterfaceName} = {{ 0x{BitConverter.ToUInt32(interfaceId, 0):x8}, 0x{BitConverter.ToUInt16(interfaceId, 4):x4}, 0x{BitConverter.ToUInt16(interfaceId, 6):x4}, {{ 0x{interfaceId[8]:x2}, 0x{interfaceId[9]:x2}, 0x{interfaceId[10]:x2}, 0x{interfaceId[11]:x2}, 0x{interfaceId[12]:x2}, 0x{interfaceId[13]:x2}, 0x{interfaceId[14]:x2}, 0x{interfaceId[15]:x2} }}}}");
                }
                writer.WriteLine("#endif  // ENABLE_MSVC_SPECIFIC_SPECIFICATIONS");
                writer.WriteLine();
                writer.WriteLine("using namespace SevenZip::NativeWrapper::IO;");
                writer.WriteLine("using namespace SevenZip::NativeWrapper::Compression;");
                writer.WriteLine();
                writer.WriteLine("namespace SevenZip");
                writer.WriteLine("{");
                writer.WriteLine("    namespace NativeWrapper");
                writer.WriteLine("    {");
                writer.WriteLine("        SevenZip::NativeInterface::IUnknown^ CreateManagedInterfaceObject(const GUID& interfaceId, IUnknown* nativeUnknownObject)");
                writer.WriteLine("        {");
                    writer.WriteLine("#if defined(_ARCHITECTURE_NATIVE_INTEGER_32_BIT)");
                    writer.WriteLine("            UInt32 guid0 = ((const UInt32*)&interfaceId)[0];");
                    writer.WriteLine("            UInt32 guid1 = ((const UInt32*)&interfaceId)[1];");
                    writer.WriteLine("            UInt32 guid2 = ((const UInt32*)&interfaceId)[2];");
                    writer.WriteLine("            UInt32 guid3 = ((const UInt32*)&interfaceId)[3];");
                    writer.WriteLine("#if defined(_ARCHITECTURE_LITTLE_ENDIAN)");
                    RenderInterfaceCreatorFinder32(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        true,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    return nullptr;");
                            else
                                writer.WriteLine($"{indent}return Managed{interfaceName[1..]}::Create(nativeUnknownObject);");
                        });
                    writer.WriteLine("#elif defined(_ARCHITECTURE_BIG_ENDIAN)");
                    RenderInterfaceCreatorFinder32(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        false,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    return nullptr;");
                            else
                                writer.WriteLine($"{indent}return Managed{interfaceName[1..]}::Create(nativeUnknownObject);");
                        });
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_xxx_ENDIAN'\"");
                    writer.WriteLine("#endif");
                    writer.WriteLine("#elif defined(_ARCHITECTURE_NATIVE_INTEGER_64_BIT)");
                    writer.WriteLine("            UInt64 guid0 = ((const UInt64*)&interfaceId)[0];");
                    writer.WriteLine("            UInt64 guid1 = ((const UInt64*)&interfaceId)[1];");
                    writer.WriteLine("#if defined(_ARCHITECTURE_LITTLE_ENDIAN)");
                    RenderInterfaceCreatorFinder64(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        true,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    return nullptr;");
                            else
                                writer.WriteLine($"{indent}return Managed{interfaceName[1..]}::Create(nativeUnknownObject);");
                        });
                    writer.WriteLine("#elif defined(_ARCHITECTURE_BIG_ENDIAN)");
                    RenderInterfaceCreatorFinder64(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        false,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    return nullptr;");
                            else
                                writer.WriteLine($"{indent}return Managed{interfaceName[1..]}::Create(nativeUnknownObject);");
                        });
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_xxx_ENDIAN'\"");
                    writer.WriteLine("#endif");
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_NATIVE_INTEGER_xx_BIT'\"");
                    writer.WriteLine("#endif");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("#ifdef _DEBUG");
                writer.WriteLine("        static void ValidateInterfaceId(const GUID& interfaceId)");
                writer.WriteLine("        {");
                    writer.WriteLine("#if defined(_ARCHITECTURE_NATIVE_INTEGER_32_BIT)");
                    writer.WriteLine("            UInt32 guid0 = ((const UInt32*)&interfaceId)[0];");
                    writer.WriteLine("            UInt32 guid1 = ((const UInt32*)&interfaceId)[1];");
                    writer.WriteLine("            UInt32 guid2 = ((const UInt32*)&interfaceId)[2];");
                    writer.WriteLine("            UInt32 guid3 = ((const UInt32*)&interfaceId)[3];");
                    writer.WriteLine("#if defined(_ARCHITECTURE_LITTLE_ENDIAN)");
                    RenderInterfaceCreatorFinder32(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        true,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                            else
                            {
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}{{");
                                writer.WriteLine($"{indent}if (!IsEqualInterfaceId(interfaceId, __UUIDOF({interfaceName})))");
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}}}");
                            }
                        });
                    writer.WriteLine("#elif defined(_ARCHITECTURE_BIG_ENDIAN)");
                    RenderInterfaceCreatorFinder32(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        false,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                            else
                            {
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}{{");
                                writer.WriteLine($"{indent}if (!IsEqualInterfaceId(interfaceId, __UUIDOF({interfaceName})))");
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}}}");
                            }
                        });
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_xxx_ENDIAN'\"");
                    writer.WriteLine("#endif");
                    writer.WriteLine("#elif defined(_ARCHITECTURE_NATIVE_INTEGER_64_BIT)");
                    writer.WriteLine("            UInt64 guid0 = ((const UInt64*)&interfaceId)[0];");
                    writer.WriteLine("            UInt64 guid1 = ((const UInt64*)&interfaceId)[1];");
                    writer.WriteLine("#if defined(_ARCHITECTURE_LITTLE_ENDIAN)");
                    RenderInterfaceCreatorFinder64(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        true,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                            else
                            {
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}{{");
                                writer.WriteLine($"{indent}if (!IsEqualInterfaceId(interfaceId, __UUIDOF({interfaceName})))");
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}}}");
                            }
                        });
                    writer.WriteLine("#elif defined(_ARCHITECTURE_BIG_ENDIAN)");
                    RenderInterfaceCreatorFinder64(
                        writer,
                        "                ",
                        interfaceCreatorFinderSource,
                        false,
                        (writer, isRoot, indent, interfaceName) =>
                        {
                            if (interfaceName is null)
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                            else
                            {
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}{{");
                                writer.WriteLine($"{indent}if (!IsEqualInterfaceId(interfaceId, __UUIDOF({interfaceName})))");
                                writer.WriteLine($"{indent}    throw gcnew System::Exception(\"There is an error in the algorithm of 'SevenZip::Compression :: Implementation :: CreateManagedInterfaceObject()'.\");");
                                if (!isRoot)
                                    writer.WriteLine($"{indent[4..]}}}");
                            }
                        });
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_xxx_ENDIAN'\"");
                    writer.WriteLine("#endif");
                    writer.WriteLine("#else");
                    writer.WriteLine("#error \"Undefined macro '_ARCHITECTURE_NATIVE_INTEGER_xx_BIT'\"");
                    writer.WriteLine("#endif");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        void ValidateInterfaceIds()");
                writer.WriteLine("        {");
                foreach (var (name, guidByteArray, guid) in interfaceCreatorFinderSource)
                    writer.WriteLine($"                ValidateInterfaceId(__UUIDOF({name}));");
                writer.WriteLine("        }");
                writer.WriteLine("#endif");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }

        private static void RenderSevenZipNativeWrapperUnmanagedH(string solutionPath, SevenZipInterfacesModel sevenZipInterfacesModel)
        {
            using (var writer = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper.Unmanaged", "SevenZipInterface_AutoGenerated.h")))
            {
                writer.WriteLine("#pragma once");
                writer.WriteLine();
                writer.WriteLine("// This file is automatically generated. Do not rewrite manually.");
                writer.WriteLine();
                writer.WriteLine("#include \"Platform.h\"");
                writer.WriteLine();
                writer.WriteLine("struct ICompressProgressInfo;");
                foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces.Where(sevenZipInterface => !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal)))
                {
                    writer.WriteLine();
                    if (!sevenZipInterface.Implemented)
                        writer.WriteLine("#if false // This interface is not supported by the wrapper.");
                    var interfaceId = Guid.Parse(sevenZipInterface.InterfaceId);
                    var interfaceIdBytes = interfaceId.ToByteArray();
                    writer.WriteLine($"extern \"C\" const GUID IID_{sevenZipInterface.InterfaceName};");
                    writer.WriteLine($"struct {sevenZipInterface.InterfaceName}");
                    writer.WriteLine($"    : public {sevenZipInterface.BaseInerfaceName}");
                    writer.WriteLine("{");
                    foreach (var sevenZipInterfaceMember in sevenZipInterface.Members)
                    {
                        var parameters = string.Join(", ", sevenZipInterfaceMember.Parameters7z.Select(parameter => $"{parameter.ParameterType} {parameter.ParameterName}"));
                        writer.WriteLine($"    virtual {sevenZipInterfaceMember.ReturnValueType} STDMETHODCALLTYPE {sevenZipInterfaceMember.MemberName}({parameters}) throw() abstract;");
                    }
                    writer.WriteLine("};");
                    if (!sevenZipInterface.Implemented)
                        writer.WriteLine("#endif // This interface is not supported by the wrapper.");
                }
                writer.WriteLine();
                foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces)
                {
                    if (sevenZipInterface.Implemented && sevenZipInterface.ProvidedOutward)
                        foreach (var sevenZipInterfaceMember in sevenZipInterface.Members)
                        {
                            if (sevenZipInterfaceMember.IsCustomizedParameter)
                            {
                                var parameters = string.Join(", ", sevenZipInterfaceMember.ParametersCpp.Select(parameter => $"{parameter.ParameterType} {parameter.ParameterName}").Prepend($"{sevenZipInterface.InterfaceName}*  ifp"));
                                writer.WriteLine($"extern {sevenZipInterfaceMember.ReturnValueType} Customized_{sevenZipInterface.InterfaceName}__{sevenZipInterfaceMember.MemberName}({parameters});");
                            }
                        }
                }
            }
        }

        private static void RenderSevenZipNativeWrapperUnmanagedCpp(string solutionPath, SevenZipInterfacesModel sevenZipInterfacesModel)
        {
            using (var nativeInterfaceCppWriter = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper.Unmanaged", "SevenZipInterface_AutoGenerated.cpp")))
            {
                nativeInterfaceCppWriter.WriteLine("// This file is automatically generated. Do not rewrite manually.");
                nativeInterfaceCppWriter.WriteLine();
                nativeInterfaceCppWriter.WriteLine("#include \"SevenZipInterface.h\"");
                nativeInterfaceCppWriter.WriteLine();
                foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces.Where(sevenZipInterface => !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal)))
                {
                    if (!sevenZipInterface.Implemented)
                        nativeInterfaceCppWriter.WriteLine("#if false // This interface is not supported by the wrapper.");
                    var interfaceId = Guid.Parse(sevenZipInterface.InterfaceId);
                    var interfaceIdBytes = interfaceId.ToByteArray();
                    nativeInterfaceCppWriter.WriteLine($"extern \"C\" const GUID IID_{sevenZipInterface.InterfaceName} = {{ 0x{interfaceIdBytes[3]:x2}{interfaceIdBytes[2]:x2}{interfaceIdBytes[1]:x2}{interfaceIdBytes[0]:x2}, 0x{interfaceIdBytes[5]:x2}{interfaceIdBytes[4]:x2}, 0x{interfaceIdBytes[7]:x2}{interfaceIdBytes[6]:x2}, {{ 0x{interfaceIdBytes[9]:x2}, 0x{interfaceIdBytes[8]:x2}, 0x{interfaceIdBytes[10]:x2}, 0x{interfaceIdBytes[11]:x2}, 0x{interfaceIdBytes[12]:x2}, 0x{interfaceIdBytes[13]:x2}, 0x{interfaceIdBytes[14]:x2}, 0x{interfaceIdBytes[15]:x2} }} }};");
                    if (!sevenZipInterface.Implemented)
                        nativeInterfaceCppWriter.WriteLine("#endif // This interface is not supported by the wrapper.");
                }
                nativeInterfaceCppWriter.WriteLine();
                nativeInterfaceCppWriter.WriteLine("extern \"C\"");
                nativeInterfaceCppWriter.WriteLine("{");
                foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces)
                {
                    if (!sevenZipInterface.Implemented)
                        nativeInterfaceCppWriter.WriteLine("#if false // This interface is not supported by the wrapper.");
                    foreach (var sevenZipInterfaceMember in sevenZipInterface.Members)
                    {
                        var parameters = string.Join(", ", sevenZipInterfaceMember.ParametersCpp.Select(parameter => $"{parameter.ParameterType} {parameter.ParameterName}").Prepend($"{sevenZipInterface.InterfaceName}* ifp"));
                        nativeInterfaceCppWriter.WriteLine($"    __DEFINE_PUBLIC_FUNC({sevenZipInterfaceMember.ReturnValueType}, {sevenZipInterface.InterfaceName}, {sevenZipInterfaceMember.MemberName})({parameters})");
                        nativeInterfaceCppWriter.WriteLine("    {");
                        if (sevenZipInterfaceMember.IsCustomizedParameter)
                        {
                            var values = string.Join(", ", sevenZipInterfaceMember.ParametersCpp.Select(parameter => parameter.ParameterName).Prepend("ifp"));
                            if (string.Equals(sevenZipInterfaceMember.ReturnValueType, "void", StringComparison.Ordinal))
                                nativeInterfaceCppWriter.WriteLine($"        Customized_{sevenZipInterface.InterfaceName}__{sevenZipInterfaceMember.MemberName}({values});");
                            else
                                nativeInterfaceCppWriter.WriteLine($"        return Customized_{sevenZipInterface.InterfaceName}__{sevenZipInterfaceMember.MemberName}({values});");
                            nativeInterfaceCppWriter.WriteLine("    }");
                        }
                        else
                        {
                            var values = string.Join(", ", sevenZipInterfaceMember.Parameters7z.Select(parameter => parameter.ParameterName));
                            if (string.Equals(sevenZipInterfaceMember.ReturnValueType, "void", StringComparison.Ordinal))
                                nativeInterfaceCppWriter.WriteLine($"        ifp->{sevenZipInterfaceMember.MemberName}({values});");
                            else
                                nativeInterfaceCppWriter.WriteLine($"        return ifp->{sevenZipInterfaceMember.MemberName}({values});");
                            nativeInterfaceCppWriter.WriteLine("    }");
                        }
                        nativeInterfaceCppWriter.WriteLine();
                    }
                    if (!sevenZipInterface.Implemented)
                        nativeInterfaceCppWriter.WriteLine("#endif // This interface is not supported by the wrapper.");
                }
                nativeInterfaceCppWriter.WriteLine("}");
            }
        }

        private static void RenderSevenZipNativeWrapperUnmanagedCs(string solutionPath, SevenZipInterfacesModel sevenZipInterfacesModel)
        {
            foreach (var architecture in new[] {"x86", "x64"})
            {
                using (var nativeInterfaceCsWriter = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper.Managed", "Platform", $"UnmanagedEntryPoint.AutoGenerated-{architecture}.cs")))
                {
                    nativeInterfaceCsWriter.WriteLine("// This file is automatically generated. Do not rewrite manually.");
                    nativeInterfaceCsWriter.WriteLine();
                    nativeInterfaceCsWriter.WriteLine("using System;");
                    nativeInterfaceCsWriter.WriteLine("using System.Runtime.InteropServices;");
                    nativeInterfaceCsWriter.WriteLine();
                    nativeInterfaceCsWriter.WriteLine("namespace SevenZip.NativeWrapper.Managed.Platform");
                    nativeInterfaceCsWriter.WriteLine("{");
                    nativeInterfaceCsWriter.WriteLine("    partial class UnmanagedEntryPoint");
                    nativeInterfaceCsWriter.WriteLine("    {");
                    foreach (var sevenZipInterface in sevenZipInterfacesModel.Interfaces)
                    {
                        if (!sevenZipInterface.Implemented)
                            nativeInterfaceCsWriter.WriteLine($"#if false // {sevenZipInterface.InterfaceName} interface is not supported by the wrapper.");
                        foreach (var sevenZipInterfaceMember in sevenZipInterface.Members)
                        {
                            nativeInterfaceCsWriter.WriteLine("        /// <summary>");
                            nativeInterfaceCsWriter.WriteLine($"        /// {sevenZipInterface.InterfaceName}.{sevenZipInterfaceMember.MemberName}");
                            nativeInterfaceCsWriter.WriteLine("        /// </summary>");
                            nativeInterfaceCsWriter.WriteLine($"        /// <param name=\"ifp\">{sevenZipInterface.InterfaceName}*</param>");
                            foreach (var parameter in sevenZipInterfaceMember.ParametersCSharp)
                                nativeInterfaceCsWriter.WriteLine($"        /// <param name=\"{parameter.ParameterName}\">{parameter.ParameterType}</param>");
                            var returnValueType = MapTypeFromUnmanageToManage(sevenZipInterfaceMember.ReturnValueType);
                            if (!string.Equals(returnValueType, "void", StringComparison.Ordinal))
                                nativeInterfaceCsWriter.WriteLine($"        /// <returns>{sevenZipInterfaceMember.ReturnValueType}</returns>");
                            nativeInterfaceCsWriter.WriteLine($"        [DllImport(\"SevenZip.NativeWrapper.Unmanaged\", EntryPoint = \"EXPORTED_{sevenZipInterface.InterfaceName}__{sevenZipInterfaceMember.MemberName}\")]");
                            var parametersSource = sevenZipInterfaceMember.ParametersCSharp;
                            var parameters =
                                string.Join(
                                    ", ",
                                    parametersSource
                                        .Select(parameter => $"{MapTypeFromUnmanageToManage(parameter.ParameterType)} {parameter.ParameterName}").Prepend($"IntPtr ifp"));
                            var isSafe = parametersSource.All(parameter => IsCSharpSafeType(parameter.ParameterType));
                            nativeInterfaceCsWriter.WriteLine($"        {(isSafe ? "public" : "private")} static {(isSafe ? "" : "unsafe ")}extern {MapTypeFromUnmanageToManage(sevenZipInterfaceMember.ReturnValueType)} {sevenZipInterface.InterfaceName}__{sevenZipInterfaceMember.MemberName}({parameters});");
                            nativeInterfaceCsWriter.WriteLine();
                        }
                        if (!sevenZipInterface.Implemented)
                            nativeInterfaceCsWriter.WriteLine($"#endif // {sevenZipInterface.InterfaceName} interface is not supported by the wrapper.");
                    }
                    nativeInterfaceCsWriter.WriteLine("    }");
                    nativeInterfaceCsWriter.WriteLine("}");
                }
            }
        }

        private static void RenderSevenZipNativeWrapperManagedUnknownCs(string solutionPath, SevenZipInterfacesModel sevenZipInterfaces)
        {
            using (var writer = new StreamWriter(Path.Combine(solutionPath, "SevenZip.NativeWrapper.Managed", "Unknown.AutoGenerated.cs")))
            {

                var interfaceCreatorFinderSource =
                    sevenZipInterfaces.Interfaces
                    .Where(sevenZipInterface =>
                        !string.Equals(sevenZipInterface.InterfaceName, "IUnknown", StringComparison.Ordinal)
                        && sevenZipInterface.Implemented
                        && sevenZipInterface.ProvidedOutward && sevenZipInterface.CanQuery)
                    .Select(sevenZipInterface =>
                    (
                        name: sevenZipInterface.InterfaceName,
                        guidByteArray: Guid.Parse(sevenZipInterface.InterfaceId).ToByteArray(),
                        guid: Guid.Parse(sevenZipInterface.InterfaceId
                    )));
                writer.WriteLine("// This file is automatically generated. Do not rewrite manually.");
                writer.WriteLine();
                writer.WriteLine("#if _PLATFORM_WINDOWS_X64");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_64");
                writer.WriteLine("#elif _PLATFORM_WINDOWS_X86");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_32");
                writer.WriteLine("#elif _PLATFORM_LINUX_X64");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_64");
                writer.WriteLine("#elif _PLATFORM_LINUX_X86");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_32");
                writer.WriteLine("#elif _PLATFORM_MACOS_X64");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_64");
                writer.WriteLine("#elif _PLATFORM_MACOS_X86");
                writer.WriteLine("#define _ARCHITECTURE_LITTLE_ENDIAN");
                writer.WriteLine("#define _ARCHITECTURE_NATIVE_INT_32");
                writer.WriteLine("#else");
                writer.WriteLine("#error Not supported platform");
                writer.WriteLine("#endif");
                writer.WriteLine();
                writer.WriteLine("using SevenZip.NativeInterface;");
                writer.WriteLine("using SevenZip.NativeInterface.Compression;");
                writer.WriteLine("using SevenZip.NativeInterface.IO;");
                writer.WriteLine("using SevenZip.NativeWrapper.Managed.Compression;");
                writer.WriteLine("using SevenZip.NativeWrapper.Managed.IO;");
                writer.WriteLine("using System;");
                writer.WriteLine();
                writer.WriteLine("namespace SevenZip.NativeWrapper.Managed");
                writer.WriteLine("{");
                writer.WriteLine("    partial class Unknown");
                writer.WriteLine("    {");
                writer.WriteLine("        private static IUnknown? GetInterfaceObjectCreator(Guid interfaceId, IntPtr nativeInterfaceObject)");
                writer.WriteLine("        {");
                writer.WriteLine("            Span<Byte> interfaceIdBuffer = stackalloc Byte[16];");
                writer.WriteLine("            interfaceId.TryWriteBytes(interfaceIdBuffer);");
                writer.WriteLine("#if _ARCHITECTURE_NATIVE_INT_32");
                writer.WriteLine("            UInt32 guid0;");
                writer.WriteLine("            UInt32 guid1;");
                writer.WriteLine("            UInt32 guid2;");
                writer.WriteLine("            UInt32 guid3;");
                writer.WriteLine("            unsafe");
                writer.WriteLine("            {");
                writer.WriteLine("                fixed (Byte* interfaceIdPointer = interfaceIdBuffer)");
                writer.WriteLine("                {");
                writer.WriteLine("                    var p = (UInt32*)interfaceIdPointer;");
                writer.WriteLine("                    guid0 = *p++;");
                writer.WriteLine("                    guid1 = *p++;");
                writer.WriteLine("                    guid2 = *p++;");
                writer.WriteLine("                    guid3 = *p++;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("#if _ARCHITECTURE_LITTLE_ENDIAN");
                RenderInterfaceCreatorFinder32(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    true,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    return null;");
                        else
                            writer.WriteLine($"{indent}return {interfaceName[1..]}.Create(nativeInterfaceObject);");
                    });
                writer.WriteLine("#elif _ARCHITECTURE_BIG_ENDIAN");
                RenderInterfaceCreatorFinder32(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    false,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    return null;");
                        else
                            writer.WriteLine($"{indent}return {interfaceName[1..]}.Create(nativeInterfaceObject);");
                    });
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_xx_ENDIAN\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("#elif _ARCHITECTURE_NATIVE_INT_64");
                writer.WriteLine("            UInt64 guid0;");
                writer.WriteLine("            UInt64 guid1;");
                writer.WriteLine("            unsafe");
                writer.WriteLine("            {");
                writer.WriteLine("                fixed (Byte* interfaceIdPointer = interfaceIdBuffer)");
                writer.WriteLine("                {");
                writer.WriteLine("                    var p = (UInt64*)interfaceIdPointer;");
                writer.WriteLine("                    guid0 = *p++;");
                writer.WriteLine("                    guid1 = *p++;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("#if _ARCHITECTURE_LITTLE_ENDIAN");
                RenderInterfaceCreatorFinder64(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    true,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    return null;");
                        else
                            writer.WriteLine($"{indent}return {interfaceName[1..]}.Create(nativeInterfaceObject);");
                    });
                writer.WriteLine("#elif _ARCHITECTURE_BIG_ENDIAN");
                RenderInterfaceCreatorFinder64(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    false,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    return null;");
                        else
                            writer.WriteLine($"{indent}return {interfaceName[1..]}.Create(nativeInterfaceObject);");
                    });
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_xx_ENDIAN\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_NATIVE_INT_nn\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("#if DEBUG");
                writer.WriteLine("        private static void ValidateInterfaces()");
                writer.WriteLine("        {");
                foreach (var (name, guidByteArray, guid) in interfaceCreatorFinderSource)
                {
                    writer.WriteLine($"            ValidateInterface(typeof({name}).GUID);");
                }
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        private static void ValidateInterface(Guid interfaceId)");
                writer.WriteLine("        {");
                writer.WriteLine("            Span<Byte> interfaceIdBuffer = stackalloc Byte[16];");
                writer.WriteLine("            interfaceId.TryWriteBytes(interfaceIdBuffer);");
                writer.WriteLine("#if _ARCHITECTURE_NATIVE_INT_32");
                writer.WriteLine("            UInt32 guid0;");
                writer.WriteLine("            UInt32 guid1;");
                writer.WriteLine("            UInt32 guid2;");
                writer.WriteLine("            UInt32 guid3;");
                writer.WriteLine("            unsafe");
                writer.WriteLine("            {");
                writer.WriteLine("                fixed (Byte* interfaceIdPointer = interfaceIdBuffer)");
                writer.WriteLine("                {");
                writer.WriteLine("                    var p = (UInt32*)interfaceIdPointer;");
                writer.WriteLine("                    guid0 = *p++;");
                writer.WriteLine("                    guid1 = *p++;");
                writer.WriteLine("                    guid2 = *p++;");
                writer.WriteLine("                    guid3 = *p++;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("#if _ARCHITECTURE_LITTLE_ENDIAN");
                RenderInterfaceCreatorFinder32(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    true,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                        else
                        {
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}{{");
                            writer.WriteLine($"{indent}if (interfaceId != typeof({interfaceName}).GUID)");
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}}}");
                        }
                    });
                writer.WriteLine("#elif _ARCHITECTURE_BIG_ENDIAN");
                RenderInterfaceCreatorFinder32(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    false,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                        else
                        {
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}{{");
                            writer.WriteLine($"{indent}if (interfaceId != typeof({interfaceName}).GUID)");
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}}}");
                        }
                    });
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_xx_ENDIAN\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("#elif _ARCHITECTURE_NATIVE_INT_64");
                writer.WriteLine("            UInt64 guid0;");
                writer.WriteLine("            UInt64 guid1;");
                writer.WriteLine("            unsafe");
                writer.WriteLine("            {");
                writer.WriteLine("                fixed (Byte* interfaceIdPointer = interfaceIdBuffer)");
                writer.WriteLine("                {");
                writer.WriteLine("                    var p = (UInt64*)interfaceIdPointer;");
                writer.WriteLine("                    guid0 = *p++;");
                writer.WriteLine("                    guid1 = *p++;");
                writer.WriteLine("                }");
                writer.WriteLine("            }");
                writer.WriteLine("#if _ARCHITECTURE_LITTLE_ENDIAN");
                RenderInterfaceCreatorFinder64(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    true,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                        else
                        {
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}{{");
                            writer.WriteLine($"{indent}if (interfaceId != typeof({interfaceName}).GUID)");
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}}}");
                        }
                    });
                writer.WriteLine("#elif _ARCHITECTURE_BIG_ENDIAN");
                RenderInterfaceCreatorFinder64(
                    writer,
                    "                ",
                    interfaceCreatorFinderSource,
                    false,
                    (writer, isRoot, indent, interfaceName) =>
                    {
                        if (interfaceName is null)
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                        else
                        {
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}{{");
                            writer.WriteLine($"{indent}if (interfaceId != typeof({interfaceName}).GUID)");
                            writer.WriteLine($"{indent}    throw new Exception(\"There is an error in the algorithm of 'SevenZip.NativeWrapper.Managed.Unknown.ValidateInterface()'.\");");
                            if (!isRoot)
                                writer.WriteLine($"{indent[4..]}}}");
                        }
                    });
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_xx_ENDIAN\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("#else");
                writer.WriteLine("#error The \"_ARCHITECTURE_NATIVE_INT_nn\" symbol is not defined.");
                writer.WriteLine("#endif");
                writer.WriteLine("        }");
                writer.WriteLine("#endif");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }

        private static void RenderInterfaceCreatorFinder32(TextWriter writer, string indent, IEnumerable<(string name, Byte[] guidByteArray, Guid guid)> interfaceNames, bool isLittleEndian, Action<TextWriter, bool, string, string?> creatorWriter)
        {
            var source =
                interfaceNames
                .Select(item => new
                {
                    item.name,
                    id = new[]
                    {
                        ToUInt32(item.guidByteArray, sizeof(UInt32) * 0, isLittleEndian),
                        ToUInt32(item.guidByteArray, sizeof(UInt32) * 1, isLittleEndian),
                        ToUInt32(item.guidByteArray, sizeof(UInt32) * 2, isLittleEndian),
                        ToUInt32(item.guidByteArray, sizeof(UInt32) * 3, isLittleEndian),
                    },
                    item.guid,
                })
                .ToArray();
            Array.Sort(
                source,
                (x, y) =>
                {
                    Int32 c;
                    if ((c = x.id[0].CompareTo(y.id[0])) != 0)
                        return c;
                    if ((c = x.id[1].CompareTo(y.id[1])) != 0)
                        return c;
                    if ((c = x.id[2].CompareTo(y.id[2])) != 0)
                        return c;
                    if ((c = x.id[3].CompareTo(y.id[3])) != 0)
                        return c;
                    return 0;
                });
            RenderInterfaceCreatorFinder32(writer, indent, source.Select(item => (item.name, item.id, item.guid)).ToArray(), 0, 0, source.Length, true, creatorWriter);
        }

        private static void RenderInterfaceCreatorFinder32(TextWriter writer, string indent, (string name, UInt32[] keys, Guid guid)[] source, Int32 keyIndex, Int32 lowerBound, Int32 upperBound, bool isRoot, Action<TextWriter, bool, string, string?> creatorWriter)
        {
            if (keyIndex >= 4)
            {
                if (upperBound - lowerBound != 1)
                    throw new Exception();
                creatorWriter(writer, isRoot, indent, source[lowerBound].name);
            }
            else if (upperBound - lowerBound <= 0)
                throw new Exception();
            else
            {
                var keys = source.Skip(lowerBound).Take(upperBound - lowerBound).Select(item => item.keys[keyIndex]).Distinct().OrderBy(key => key).ToArray();
                if (keys.Length <= 1)
                {
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} != 0x{source[lowerBound].keys[keyIndex]:x8})");
                    creatorWriter(writer, isRoot, indent, null);
                    RenderInterfaceCreatorFinder32(writer, indent, source, keyIndex + 1, lowerBound, upperBound, isRoot, creatorWriter);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
                else if (keys.Length == 2)
                {
                    var part1Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] == keys[0]).Count();
                    var part2Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] == keys[1]).Count();
                    if (part1Length + part2Length != upperBound - lowerBound)
                        throw new Exception();
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} == 0x{keys[0]:x8})");
                    RenderInterfaceCreatorFinder32(writer, indent + "    ", source, keyIndex + 1, lowerBound, lowerBound + part1Length, false, creatorWriter);
                    writer.WriteLine($"{indent}else if (guid{keyIndex} == 0x{keys[1]:x8})");
                    RenderInterfaceCreatorFinder32(writer, indent + "    ", source, keyIndex + 1, lowerBound + part1Length, upperBound, false, creatorWriter);
                    writer.WriteLine($"{indent}else");
                    creatorWriter(writer, isRoot, indent, null);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
                else
                {
                    var mediumKey = keys[keys.Length / 2];
                    var part1Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] < mediumKey).Count();
                    var part2Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] >= mediumKey).Count();
                    if (part1Length + part2Length != upperBound - lowerBound)
                        throw new Exception();
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} < 0x{mediumKey:x8})");
                    RenderInterfaceCreatorFinder32(writer, indent + "    ", source, keyIndex, lowerBound, lowerBound + part1Length, false, creatorWriter);
                    writer.WriteLine($"{indent}else");
                    RenderInterfaceCreatorFinder32(writer, indent + "    ", source, keyIndex, lowerBound + part1Length, upperBound, false, creatorWriter);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
            }
        }

        private static void RenderInterfaceCreatorFinder64(TextWriter writer, string indent, IEnumerable<(string name, Byte[] guidByteArray, Guid guid)> interfaceNames, bool isLittleEndian, Action<TextWriter, bool, string, string?> creatorWriter)
        {
            var source =
                interfaceNames
                .Select(item => new
                {
                    item.name,
                    id = new[]
                    {
                        ToUInt64(item.guidByteArray, sizeof(UInt64) * 0, isLittleEndian),
                        ToUInt64(item.guidByteArray, sizeof(UInt64) * 1, isLittleEndian),
                    },
                    item.guid,
                })
                .ToArray();
            Array.Sort(
                source,
                (x, y) =>
                {
                    Int32 c;
                    if ((c = x.id[0].CompareTo(y.id[0])) != 0)
                        return c;
                    if ((c = x.id[1].CompareTo(y.id[1])) != 0)
                        return c;
                    return 0;
                });
            RenderInterfaceCreatorFinder64(writer, indent, source.Select(item => (item.name, item.id, item.guid)).ToArray(), 0, 0, source.Length, true, creatorWriter);
        }

        private static void RenderInterfaceCreatorFinder64(TextWriter writer, string indent, (string name, UInt64[] keys, Guid guid)[] source, Int32 keyIndex, Int32 lowerBound, Int32 upperBound, bool isRoot, Action<TextWriter, bool, string, string?> creatorWriter)
        {
            if (keyIndex >= 2)
            {
                if (upperBound - lowerBound != 1)
                    throw new Exception();
                creatorWriter(writer, isRoot, indent, source[lowerBound].name);
            }
            else if (upperBound - lowerBound <= 0)
                throw new Exception();
            else
            {
                var keys = source.Skip(lowerBound).Take(upperBound - lowerBound).Select(item => item.keys[keyIndex]).Distinct().OrderBy(key => key).ToArray();
                if (keys.Length <= 1)
                {
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} != 0x{source[lowerBound].keys[keyIndex]:x8})");
                    creatorWriter(writer, isRoot, indent, null);
                    RenderInterfaceCreatorFinder64(writer, indent, source, keyIndex + 1, lowerBound, upperBound, isRoot, creatorWriter);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
                else if (keys.Length == 2)
                {
                    var part1Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] == keys[0]).Count();
                    var part2Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] == keys[1]).Count();
                    if (part1Length + part2Length != upperBound - lowerBound)
                        throw new Exception();
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} == 0x{keys[0]:x8})");
                    RenderInterfaceCreatorFinder64(writer, indent + "    ", source, keyIndex + 1, lowerBound, lowerBound + part1Length, false, creatorWriter);
                    writer.WriteLine($"{indent}else if (guid{keyIndex} == 0x{keys[1]:x8})");
                    RenderInterfaceCreatorFinder64(writer, indent + "    ", source, keyIndex + 1, lowerBound + part1Length, upperBound, false, creatorWriter);
                    writer.WriteLine($"{indent}else");
                    creatorWriter(writer, isRoot, indent, null);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
                else
                {
                    var mediumKey = keys[keys.Length / 2];
                    var part1Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] < mediumKey).Count();
                    var part2Length = source.Skip(lowerBound).Take(upperBound - lowerBound).Where(item => item.keys[keyIndex] >= mediumKey).Count();
                    if (part1Length + part2Length != upperBound - lowerBound)
                        throw new Exception();
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}{{");
                    writer.WriteLine($"{indent}if (guid{keyIndex} < 0x{mediumKey:x8})");
                    RenderInterfaceCreatorFinder64(writer, indent + "    ", source, keyIndex, lowerBound, lowerBound + part1Length, false, creatorWriter);
                    writer.WriteLine($"{indent}else");
                    RenderInterfaceCreatorFinder64(writer, indent + "    ", source, keyIndex, lowerBound + part1Length, upperBound, false, creatorWriter);
                    if (!isRoot)
                        writer.WriteLine($"{indent[4..]}}}");
                }
            }
        }

        private static UInt32 ToUInt32(Byte[] array, Int32 index, bool isLitleEndian)
        {
            var value = 0U;
            if (isLitleEndian)
            {
                value |= (UInt32)array[index + 0] << (8 * 0);
                value |= (UInt32)array[index + 1] << (8 * 1);
                value |= (UInt32)array[index + 2] << (8 * 2);
                value |= (UInt32)array[index + 3] << (8 * 3);
            }
            else
            {
                value |= (UInt32)array[index + 0] << (8 * 3);
                value |= (UInt32)array[index + 1] << (8 * 2);
                value |= (UInt32)array[index + 2] << (8 * 1);
                value |= (UInt32)array[index + 3] << (8 * 0);
            }
            return value;
        }

        private static UInt64 ToUInt64(Byte[] array, Int32 index, bool isLitleEndian)
        {
            UInt64 value = 0UL;
            if (isLitleEndian)
            {
                value |= (UInt64)array[index + 0] << (8 * 0);
                value |= (UInt64)array[index + 1] << (8 * 1);
                value |= (UInt64)array[index + 2] << (8 * 2);
                value |= (UInt64)array[index + 3] << (8 * 3);
                value |= (UInt64)array[index + 4] << (8 * 4);
                value |= (UInt64)array[index + 5] << (8 * 5);
                value |= (UInt64)array[index + 6] << (8 * 6);
                value |= (UInt64)array[index + 7] << (8 * 7);
            }
            else
            {
                value |= (UInt64)array[index + 0] << (8 * 7);
                value |= (UInt64)array[index + 1] << (8 * 6);
                value |= (UInt64)array[index + 2] << (8 * 5);
                value |= (UInt64)array[index + 3] << (8 * 4);
                value |= (UInt64)array[index + 4] << (8 * 3);
                value |= (UInt64)array[index + 5] << (8 * 2);
                value |= (UInt64)array[index + 6] << (8 * 1);
                value |= (UInt64)array[index + 7] << (8 * 0);
            }
            return value;
        }

        private static bool IsCSharpSafeType(string unmanagedType)
        {
            switch (unmanagedType)
            {
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                case "MethodPropID":
                case "NativeInStreamReader":
                case "NativeOutStraamWriter":
                case "NativeProgressReporter":
                case "NativeProgressReporter?":
                case "ref NativeGUID":
                case "ref PROPVARIANT":
                case "out UInt32":
                case "out UInt64":
                case "out IntPtr":
                    return true;
                case "void*":
                case "Byte*":
                case "UInt32*":
                case "UInt64*":
                case "CoderPropID*":
                case "PROPVARIANT*":
                case "NativeInStreamReader*":
                case "NativeOutStraamWriter*":
                case "void**":
                case "UInt64**":
                    return false;
                default:
                    throw new Exception($"Unhandled type: {unmanagedType}");
            }
        }

        private static string MapTypeFromUnmanageToManage(string unmanagedType)
        {
            switch (unmanagedType)
            {
                case "void":
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                case "CoderPropID":
                case "MethodPropID":
                case "HRESULT":
                case "NativeInStreamReader":
                case "NativeOutStraamWriter":
                case "NativeProgressReporter":
                case "NativeProgressReporter?":
                case "void*":
                case "Byte*":
                case "UInt32*":
                case "UInt64*":
                case "CoderPropID*":
                case "MethodPropID*":
                case "PROPVARIANT*":
                case "NativeInStreamReader*":
                case "NativeOutStraamWriter*":
                case "void**":
                case "UInt64**":
                case "ref NativeGUID":
                case "ref PROPVARIANT":
                case "out UInt32":
                case "out UInt64":
                case "out IntPtr":
                    return unmanagedType;

                default:
                    {
                        var match = Regex.Match(unmanagedType, @"^I[A-Za-z0-9]+\*$");
                        if (match.Success)
                            return "void*";
                    }
                    {
                        var match = Regex.Match(unmanagedType, @"^I[A-Za-z0-9]+\*\*$");
                        if (match.Success)
                            return "void**";
                    }
                    throw new Exception($"Unhandled type: {unmanagedType}");
            }
        }
    }
}
