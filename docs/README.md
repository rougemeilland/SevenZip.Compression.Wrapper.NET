Language: [日本語](README_ja.md) **English**

# SevenZip.Compression.Wrapper.NET

<!--
## 0. 目次
-->

## 0. Table of contents

+ [1. Overview](#1-Overview)
+ [2. Features of this software](#2-Features-of-this-software)
+ [3. Required environment](#3-Required-environment)
+ [4. About settings](#4-About-settings)
  + [4.1 7-zip settings](#41-7-zip-settings)
+ [5. Sample source code](#5-Sample-source-code)
  + [5.1 Compress a file with Deflate algorithm](#51-Compress-a-file-with-Deflate-algorithm)
  + [5.2 Decompress a file with Deflate algorithm (1)](#52-Decompress-a-file-with-Deflate-algorithm-1)
  + [5.3 Decompress a file with Deflate algorithm (2)](#53-Decompress-a-file-with-Deflate-algorithm-2)
  + [5.4 Compress a file with LZMA algorithm](#54-Compress-a-file-with-LZMA-algorithm)
  + [5.5 Decompress a file with LZMA algorithm (1)](#55-Decompress-a-file-with-LZMA-algorithm-1)
  + [5.6 Decompress a file with LZMA algorithm (2)](#56-Decompress-a-file-with-LZMA-algorithm-2)
  + [5.7 Implementation sample of class (`ProgressReporter`) that receives progress notifications](#57-Implementation-sample-of-class-ProgressReporter-that-receives-progress-notifications)
+ [6. License](#6-License)
+ [7. Notes](#7-Notes)
  + [7.1 About PPMd algorithm compatibility](#71-About-PPMd-algorithm-compatibility)
+ [8. Disclaimer](#8-Disclaimer)
+ [9. Appendix](#9-Appendix)
  + [9.1 Notes on porting to other systems](#91-Notes-on-porting-to-other-systems)

<!--
## 1. 概要
-->
## 1. Overview

<!--
本ソフトウェア (`SevenZip.Compression.Wrapper.NET`) は、7-zip が提供している一部の機能 (ストリームの圧縮/伸長)を .NET アプリケーションから利用するためのライブラリです。
-->
This software (`SevenZip.Compression.Wrapper.NET`) is a library that allows you to use some of the functions (stream compression/decompression) provided by 7-zip from .NET applications.

<!--
## 2. 本ソフトウェアの機能
-->
## 2. Features of this software

<!--
本ソフトウェアのクラスを利用することにより、7-zip によるデータストリームの圧縮および伸長をする機能を利用することが出来ます。
本ソフトウェアでサポートされている圧縮/伸長の方式は以下の通りです。
-->
By using the classes of this software, you can use the 7-zip function to compress and decompress data streams.
The compression/decompression methods supported by this software are as follows.

<!--
| 圧縮方式 | 圧縮のためのクラス | 伸長のためのクラス |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |
-->
| Compression method | Class for compression | Class for decompression |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |

<!--
なお 本ソフトウェアはあくまで単体のデータストリームの圧縮/伸長をするものであり、`.zip` や `.7z` などの書庫へのアクセスはサポートされていないことに注意してください。
-->
Please note that this software only compresses/decompresses a single data stream, and does not support accessing archives such as `.zip` or `.7z`.

<!--
## 3. 必要な環境
-->
## 3. Required environment

<!--
| 項目 | 条件 |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET ランタイム | 7.0 / 8.0 |
| 7-zip | 7-zip 23.01 で動作確認済 |
-->
| Item | Condition |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET rumtime | 7.0 / 8.0 |
| 7-zip | Confirmed to work with 7-zip 23.01 |

<!--
## 4. 設定について
-->
## 4. About settings

<!--
### 4.1 7-zip に関する設定
-->
### 4.1 7-zip settings

<!--
本ソフトウェアを利用するためには、7-zip をインストールする必要があります。
また、それに加えて、適切な設定を行う必要があります。
詳細については、"[`SevenZip.Compression.Wrapper.NET` から 7-zip を使用可能にする方法](HowToInstall7z_ja.md)" を参照してください。
-->
To use this software, you need to install 7-zip.
Additionally, appropriate settings must be made.
For more information, see "[How to enable 7-zip from `SevenZip.Compression.Wrapper.NET`](HowToInstall7z_en.md)."

<!--
## 5. サンプルソースコード
-->
## 5. Sample source code

<!--
### 5.1 Deflate アルゴリズムでファイルを圧縮する
-->
### 5.1 Compress a file with Deflate algorithm


```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

...


        // This is a method that compresses the contents of a file using Deflate and saves it to another file.
        public static void CompressWithDeflate(string uncompressedFilePath, string compressedFilePath)
        {
            // Open the input file.
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // Open the output file.
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Create a Deflate encoder object.
            using var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal });

            // Start compression.
            deflateEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // If you do not need to display the progress status, specify "null" instead of "new ProgressReporter()".
        }

```

<!--
### 5.2 Deflate アルゴリズムでファイルを伸長する (1)
-->
### 5.2 Decompress a file with Deflate algorithm (1)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // This is a method that decompresses a file compressed with Deflate and saves it to another file.
        public static void UncompressWithDeflate_1(string compressedFilePath, string uncompressedFilePath)
        {
            // Open the input file.
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // Open the output file.
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Create a Deflate decoder object.
            using var deflateDecoder = DeflateDecoder.CreateDecoder();

            // Start decompression.
            deflateDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // If you do not need to display the progress status, specify "null" instead of "new ProgressReporter()".
        }

```

<!--
### 5.3 Deflate アルゴリズムでファイルを伸長する (2)
-->
### 5.3 Decompress a file with Deflate algorithm (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // This is another version of the method for decompressing files compressed with Deflate.
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
        {
            // The data read from the Stream object returned by this method is decompressed data.
            return DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);
        }

```

<!--
### 5.4 LZMA アルゴリズムでファイルを圧縮する
-->
### 5.4 Compress a file with LZMA algorithm

<!--
LZMA および LZMA2 アルゴリズムでの圧縮の際には、圧縮を行う前に、エンコーダの `WriteCoderProperties()` メソッドを呼び出して圧縮先のファイルに小さなヘッダを書き込まなくてはなりません。
本ソフトウェアではこの小さなヘッダを **コンテンツプロパティ** と呼称しています。
コンテンツプロパティの詳細については、[コンテンツプロパティについて](AboutContentProperties_ja.md) を参照してください。
-->
When compressing with the LZMA and LZMA2 algorithms, you must call the encoder's `WriteCoderProperties()` method to write a small header to the destination file before compression.
In this software, this small header is called **content properties**.
For more information about content properties, see [About Content properties](AboutContentProperties_en.md).


```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Lzma;

...


        // This is a method that compresses the contents of a file using LZMA and saves it to another file.
        public static void CompressWithLzma(string uncompressedFilePath, string compressedFilePath)
        {
            // Open the input file.
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // Open the output file.
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Create an LZMA encoder object.
            using var lzmaEncoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal, EndMarker = true });
            
            // This step is required for some encoders such as LZMA.
            lzmaEncoder.WriteCoderProperties(outCompressedStream);

            // Start compression.
            lzmaEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // If you do not need to display the progress status, specify "null" instead of "new ProgressReporter()".
        }
```

<!--
### 5.5 LZMA アルゴリズムでファイルを伸長する (1)
-->
### 5.5 Decompress a file with LZMA algorithm (1)

<!--
LZMA および LZMA2 アルゴリズムでの伸長の際には、まず圧縮されているデータの先頭から小さなヘッダを読み込み、読み込んだヘッダをデコーダに与えなければなりません。
本ソフトウェアではこの小さなヘッダを **コンテンツプロパティ** と呼称しています。
コンテンツプロパティの長さ (バイト数) は圧縮アルゴリズムの種類によって異なります。
LZMAデコーダの場合、その値は定数 `SevenZip.Compression.Lzma.LzmaDecoder.CONTENT_PROPERTY_SIZE` に定義されています。
コンテンツプロパティの詳細については、[コンテンツプロパティについて](AboutContentProperties_ja.md) を参照してください。
-->
When decompressing with the LZMA and LZMA2 algorithms, you must first read a small header from the beginning of the compressed data and feed the read header to the decoder.
In this software, this small header is called **content properties**.
The length (in bytes) of the content property varies depending on the type of compression algorithm.
For LZMA decoders, its value is defined in the constant `SevenZip.Compression.Lzma.LzmaDecoder.CONTENT_PROPERTY_SIZE`.
For more information about content properties, see [About Content Properties](AboutContentProperties_en.md).


```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // This is a method that decompresses a file compressed with LZMA and saves it to another file.
        public static void UncompressWithLzma_1(string compressedFilePath, string uncompressedFilePath)
        {
            // Open the input file.
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // This step is required for some decoders such as LZMA.
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // Open the output file.
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Create an LZMA decoder object.
            using var lzmaDecoder = LzmaDecoder.CreateDecoder(contentProperties);

            // Start decomprssion.
            lzmaDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // If you do not need to display the progress status, specify "null" instead of "new ProgressReporter()".
        }
```

<!--
### 5.6 LZMA アルゴリズムでファイルを伸長する (2)
-->
### 5.6 Decompress a file with LZMA algorithm (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // This is another version of the method for decompressing files compressed with LZMA.
        // The data read from the Stream object returned by this method is decompressed data.
        public static Stream UncompressWithLzma_2(string compressedFilePath)
        {
            // Open the input file.
            var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // This step is required for some decoders such as LZMA.
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // The data read from the Stream object returned by this method is decompressed data.
            return LzmaDecoder.CreateDecoderStream(inCompressedStream, null, contentProperties);
        }
```

<!--
### 5.7 進捗状況の通知を受けるクラス (`ProgressReporter`) の実装サンプル
-->
### 5.7 Implementation sample of class (`ProgressReporter`) that receives progress notifications

```csharp
using System;

...

        // This class is for displaying the compression progress on the console.
        // This is not necessary.
        private class ProgressReporter
            : IProgress<(ulong? inStreamProgressedCount, ulong? outStreamProcessedCount)>
        {
            public void Report((ulong? inStreamProgressedCount, ulong? outStreamProcessedCount) value)
            {
                Console.Write("\x1b[0K");
                Console.Write($"in: {(value.inStreamProgressedCount is null ? "???" : value.inStreamProgressedCount.Value.ToString("N0") + " bytes")}, ");
                Console.Write($"out: {(value.outStreamProcessedCount is null ? "???" : value.outStreamProcessedCount.Value.ToString("N0") + " bytes")}");
                Console.Write("\r");
            }
        }

```

<!--
## 6. ライセンス
-->
## 6. License

<!--
本ソフトウェアのソースコードには MIT ライセンスが適用されます。
7-zip のライセンスについては、[7-zip の公式サイト](https://www.7-zip.org/) を参照してください。
-->
The source code of this software is covered by the MIT License.
For information on the 7-zip license, please refer to [7-zip official website](https://www.7-zip.org/).

<!--
## 7. 注意事項
-->
## 7. Notes

<!--
### 7.1 PPMd アルゴリズムの互換性について
-->
### 7.1 About PPMd algorithm compatibility

<!--
PPMd アルゴリズムには互換性のないいくつかのバージョンが存在します。
それらの中で、本ソフトウェアでサポートされているのは **"PPMd version H"** と呼ばれているアルゴリズムです。それは (おそらく) これは 7-zip の `.7z` 形式の書庫で採用されているアルゴリズムです。
-->
Several incompatible versions of the PPMd algorithm exist.
Among them, the one supported by this software is the algorithm called **"PPMd version H"**. This is (presumably) the algorithm used in 7-zip's `.7z` archives.

<!--
それに対して、`.zip` で採用されているのは **"PPMd version I, Rev 1"** と呼ばれているアルゴリズムです。
7-zip の `.zip` 形式の書庫では **"PPMd version I, Rev 1"** も実装されているのですが、それは本ソフトウェアではサポートされていません。
その理由は、7-zip のライブラリが **"PPMd version I, Rev 1"** の実装を外部に公開していないからです。
-->
On the other hand, `.zip` uses an algorithm called **"PPMd version I, Rev 1"**.
7-zip's `.zip` format archive also implements **"PPMd version I, Rev 1"**, but it is not supported by this software.
The reason is that the 7-zip library does not expose an implementation of **"PPMd version I, Rev 1"**.

<!--
7-zip のライブラリにおいて **"PPMd version I, Rev 1"** の実装が公開されていない理由については、[SourceForge.net の 7-zip のフォーラムの記事](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) を参照してください。
-->
See the [7-zip forum article on SourceForge.net](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) for information on why 7-zip's library does not publish an implementation of **"PPMd version I, Rev 1"**.

<!--
## 8. 免責事項
-->
## 8. Disclaimer

<!--
本ソフトウェアの利用にあたり何らかの不具合やトラブルが生じたとしても、本ソフトウェアの開発者は一切の責任を取りかねます。ご了承ください。
-->
The developer of this software is not responsible for any defects or troubles that may occur when using this software. Please understand that.

<!--
## 9. 付録
-->
## 9. Appendix

<!--
### 9.1 他の処理系への移植に関するメモ
-->
### 9.1 Notes on porting to other systems

<!--
現在のところ、本ソフトウェアがサポートしている OS や CPU は [3. 必要な環境](#3-必要な環境) に示されている通りです。
しかし、これは本ソフトウェアの開発者が利用可能なハードウェアが制限されていることが理由であって、本ソフトウェアの仕組みによるものではありません。
おそらくは、他のプラットフォームへの移植は以下の手順で可能であると思われます。
-->
Currently, the OS and CPUs supported by this soHowever, this is due to the limitations on the hardware available to the developer of this software, and is not due to the mechanism of this software.
ftware are as shown in [3. Required environment] (#3-Required environment).
Perhaps porting to other platforms is possible by following the steps below.

<!--
1. インクルードファイル `Palmtree.SevenZip.Compression.Wrapper.NET.Native/Platform.h` において、OSおよびアーキテクチャに依存するコードを修正する。
2. ネイティブコードの C++ プロジェクトを追加する。そのプロジェクトの名前は`Palmtree.SevenZip.Compression.Wrapper.NET.Native.<OS名>_<アーキテクチャ名>` とする。
  + OS名 => Windows の場合は **"win"** 、Linux の場合は **"linux"** 、MacOS の場合は **"osx"**
  + アーキテクチャ名 => x86 の場合は **"x86"** 、x64 の場合は **"x64"** 、ARM の場合は **"arm32"** 、ARM64 の場合は **"arm64"**
3. ネイティブコードの OS が Windows である場合は、`NativeInterfaceIdGenerator.exe` を Visual Studio 上から実行する。(バージョンリソースをテンプレートからコピーするため)
4. 作成したプロジェクトでは、ディレクトリ `Palmtree.SevenZip.Compression.Wrapper.NET.Native/` 上のソースファイルおよびインクルードファイルを参照して使用する。ただし、`.rc` ファイルと `resource.h` だけは各プロジェクトにあるものを参照する。
5. `Palmtree.SevenZip.Compression.Wrapper.NET.Native/Palmtree.SevenZip.Compression.Wrapper.NET.Native.nuspec` を編集して、ネイティブコードに対応する部分のコメントを解除する。
6. PowerShell で `Palmtree.SevenZip.Compression.Wrapper.NET.Native/MakePackage.ps1` を実行してパッケージを作成する。
7. Visual Studio において、プロジェクト `Palmtree.SevenZip.Compression.Wrapper.NET` の `nuget パッケージの管理` 画面を開き、既にインストールされているパッケージ `Palmtree.SevenZip.Compression.Wrapper.NET.Native` を更新する。
8. Visual Studio において、プロジェクト `Palmtree.SevenZip.Compression.Wrapper.NET` をビルドする。
-->
1. Modify the OS and architecture dependent code in the include file `Palmtree.SevenZip.Compression.Wrapper.NET.Native/Platform.h`.
2. Add a native code C++ project. The name of the project is `Palmtree.SevenZip.Compression.Wrapper.NET.Native.<OS>_<Architecture>`.
  + OS => **"win"** for Windows, **"linux"** for Linux, **"osx"** for MacOS
  + Architecture => **"x86"** for x86, **"x64"** for x64, **"arm32"** for ARM, **"arm64" for ARM64 **
3. If the native code OS is Windows, run `NativeInterfaceIdGenerator.exe` from Visual Studio. (to copy the version resource from the template)
4. In the created project, refer to and use the source files and include files in the directory `Palmtree.SevenZip.Compression.Wrapper.NET.Native/`. However, only the `.rc` file and `resource.h` refer to those in each project.
5. Edit `Palmtree.SevenZip.Compression.Wrapper.NET.Native/Palmtree.SevenZip.Compression.Wrapper.NET.Native.nuspec` and uncomment the part that corresponds to the native code.
6. Run `Palmtree.SevenZip.Compression.Wrapper.NET.Native/MakePackage.ps1` in PowerShell to create a package.
7. In Visual Studio, open the `manage nuget packages` screen for the project `Palmtree.SevenZip.Compression.Wrapper.NET`. Then, update the already installed package `Palmtree.SevenZip.Compression.Wrapper.NET.Native`.
8. Build the project `Palmtree.SevenZip.Compression.Wrapper.NET` in Visual Studio.
