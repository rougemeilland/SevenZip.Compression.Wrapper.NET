言語: [日本語](AboutContentProperty_ja.md) | **English**

<!--コンテンツプロパティについて-->
# About *Content property*[^1]

<!--コンテンツプロパティとは何か？-->
## 1. What is *content property* ?

<!--コンテンツプロパティの概要-->
### 1.1. Content property overview

<!--*コンテンツプロパティ*とは、7-zipでサポートされているいくつかのコーデックにおいて、エンコードされたデータを正しくデコードするために必要となるバイトデータです。-->
*Content property* is the byte data required to correctly decode the encoded data in some codecs supported by 7-zip.

<!--アプリケーションの開発者は、*コンテンツプロパティ*はデコードに必要なある長さのバイトデータであることだけを知っていればよく、その内容が何を意味するかを理解する必要はありません。-->
<!--しかし、*コンテンツプロパティ*は、7-zipが提供するコーデックによってエンコードされたデータには含まれておらず、かつデコードの際には必要なデータであるので、アプリケーションは*コンテンツプロパティ*を正しく管理する必要があります。-->
Application developers only need to know that the *content property* is byte data of the length required for decoding, and do not need to understand what that content means.
However, the application needs to manage the *content property* correctly. Because the *content property* is not included in the data encoded by the codec provided by 7-zip, and it is necessary data for decoding.

<!--7-zip では*コンテンツプロパティ*はどのように使用されているか-->
### 1.2. How is content property used in 7-zip ?

<!--具体的には、*コンテンツプロパティ*とは、7-zipのソースコードにおける以下のインターフェースで使用されるバイト列のことです。-->
Specifically, the content property is the byte string used in the following interface in the 7-zip source code.
- `ICompressWriteCoderProperties`
- `ICompressSetDecoderProperties2`

<!--`ICompressWriteCoderProperties`インターフェースは、指定されたストリームに*コンテンツプロパティ*を書き込むことをエンコーダーに指示するメソッドを含んでいます。-->
The `ICompressWriteCoderProperties` interface contains a method that tells the encoder to write a *content property* to the specified stream.

<!--また、`ICompressSetDecoderProperties2`インターフェースは*コンテンツプロパティ*をデコーダーに設定するメソッドを含んでいます。-->
Also, the `ICompressSetDecoderProperties2` interface contains a method to set the *content property* to the decoder.

<!--これらのインターフェースを実装しているコーデックをアプリケーションが利用する場合は、*コンテンツプロパティ*がアプリケーションによって適切に管理されなければなりません。-->
<!--7-zip バージョン 21.07 において、その対象となるコーデックは以下の通りです。-->
If your application uses codecs that implement above interfaces, the *content property* must be properly managed by your application.
In 7-zip version 21.07, the target codecs are as follows.
- LZMA
- LZMA2
- PPMd7 (PPMd vesion H)
- Rar1
- Rar2
- Rar3
- Rar5

<!--何故アプリケーションが*コンテンツプロパティ*を管理しなければならないのか？-->
## 2. Why does an application have to manage *content property* ?

<!--本来ならば、少なくともアプリケーションにとっては、*コンテンツプロパティ*のようなデータはエンコードされたデータに含まれていてアプリケーションからはその存在が隠蔽されているのが望ましいことです。-->
<!--そうはならなかった理由は、おそらくは同じコーデックでも複数のファイルフォーマットが存在することに関係している、と筆者は推測しています。-->
Originally, at least for applications, it is desirable that data such as *content property* be contained in the encoded data and hidden from the application.
I'm guessing that the reason this isn't really the case is probably related to the fact that the same codec supports multiple file formats.

<!--例えば、LZMAの場合の例を挙げます。-->
For example, here is an example for LZMA.

<!--LZMA SDK に含まれているドキュメント `lzma.txt` およびその実装のソースコード `LzmaAlone.cs` によると、LZMAファイルフォーマットは以下のように規定されています。-->
According to the document `lzma.txt` and its implementation source code` LzmaAlone.cs` included in the LZMA SDK, the LZMA file format is specified as follows:

<!--
| オフセット | 長さ | 内容 |
| ---: | ---: | :--- |
| 0 | 5バイト | *コンテンツプロパティ* |
| 5 | 8バイト | エンコード前のデータの長さ (リトルエンディアン)|
| 13 | (圧縮されたデータの長さ) | 圧縮されたデータ |
-->
| Offset | Length | Description |
| ---: | ---: | :--- |
| 0 | 5 bytes | *content property* |
| 5 | 8 bytes | Data length before encoding (little endian) |
| 13 | (Length of compressed data) | Compressed data |

<!--その一方で、[ZIPファイルフォーマットの仕様書(APPNOTE.TXT)](https://pkware.cachefly.net/webdocs/casestudies/APPNOTE.TXT)によると、ZIPファイルに含まれるデータファイルがLZMA形式で圧縮されている場合のファイルフォーマットは以下のように規定されています。(実際に7-zipでのZIPアーカイブファイルの実装でもこの仕様に従っています)-->
On the other hand, according to the [ZIP File Format Specification (APPNOTE.TXT)](https://pkware.cachefly.net/webdocs/casestudies/APPNOTE.TXT), the file format when the data file included in the ZIP file is compressed in the LZMA format is specified as follows. (Actually, the implementation of ZIP archive file with 7-zip also follows this specification)

<!--
| オフセット | 長さ | 内容 |
| ---: | ---: | :--- |
| 0 | 1バイト | LZMA SDK のメジャーバージョン<br/>7-zipによる実装では7-zipのメジャーバージョン |
| 1 | 1バイト | LZMA SDK のマイナーバージョン<br/>7-zipによる実装では7-zipのマイナーバージョン |
| 2 | 2バイト | *コンテンツプロパティ* の長さ (リトルエンディアン)<br/>常に0x0005|
| 4 | 5バイト | *コンテンツプロパティ*|
| 9 | (Length of compressed data) | 圧縮されたデータ |
-->
| Offset | Length | Description |
| ---: | ---: | :--- |
| 0 | 1 byte | Major version of LZMA SDK <br/> Major version of 7-zip in implementation with 7-zip |
| 1 | 1 byte | Minor version of LZMA SDK <br/> Minor version of 7-zip in implementation with 7-zip |
| 2 | 2 bytes | Length of *content property* (little endian) <br/> Always 0x0005 |
| 4 | 5 bytes | *content property* |
| 9 | (Length of compressed data) | Compressed data |

<!--
7-zipでは、これらのファイルフォーマットのうち「圧縮されたデータ」の部分は同一のコーデックを使用してエンコード/デコードしており共通のフォーマットですが、
御覧の通り、**これらのファイルフォーマットのヘッダー部分には互換性がありません**。
-->
In 7-zip, the "compressed data" part of these file formats is encoded / decoded using the same codec, which is a common format.
However, as you can see, **the header parts of these file formats are incompatible**.

<!--
おそらくですが、7-zipの開発者はこのように同一のコーデックで複数のファイルフォーマットに対応するために、ヘッダー部分と圧縮されたデータ本体の部分を分離してヘッダー部分の処理はアプリケーションに任せたのではないでしょうか。[^2]
-->
I guessed that the 7-zip developer separated the header part and the compressed data body part and left the processing of the header part to the application in order to support the multiple file formats mentioned above with one codec.[^2]

<!--
しかし、どのような経緯があったかはともかく、結果として、LZMA (およびLZMA2、PPMd7など) では **ヘッダー部分の読み込みと書き込みはアプリケーションの責任によって行われる**ことになりました。
-->
However, regardless of what happened, as a result, LZMA (and LZMA2, PPMd7, etc.) stipulates that **the reading and writing of the header part must be the responsibility of the application**.

<!--
## 3. アプリケーションは*コンテンツプロパティ* をどのように管理すればいいのか？
-->
## 3. How should an application manage *content property* ?

<!--
前述のように、少なくともLZMA (およびLZMA2、PPMd7など)のコーデックにおいては、エンコードされたデータのヘッダー部分の読み込みと書き込みはアプリケーションが行わなければなりません。
当然、ヘッダー部分のフォーマットはコーデックやファイルフォーマットにより異なるため、それぞれ異なる対応が必要です。
-->
As mentioned earlier, at least for the LZMA (and LZMA2, PPMd7, etc.) codecs, the application must read and write the header portion of the encoded data.
Of course, the format of the header part differs depending on the codec and file format, so different measures are required for each.

<!--
典型的なLZMAエンコーダ/デコーダのアプリケーションのサンプルコードを以下に挙げますので、参考にしてください。
その他のコーデックの場合も同じようなコードで処理できるはずですが、それぞれのファイルフォーマットのヘッダー部分の規定を理解し、それに従ってコードを修正する必要があります。
-->

The following is sample code for a typical LZMA encoder / decoder application for your reference.
You should be able to handle similar code for other codecs.
However, you need to understand the header part of each file format and modify the code accordingly.

<!--
なお、`ReadBytes`関数が以下のように定義されているものとします。
-->
It is assumed that the `ReadBytes` function is defined as follows.

```csharp
// Read data from inStream until buffer is filled. If inStream reaches the end in the middle of reading, an exception will occur.
private static void ReadBytes(Stream inStream, Span<Byte> buffer)
{
    while (buffer.Length > 0)
    {
        Int32 length = inStream.Read(buffer);
        if (length <= 0)
            throw new Exception("Unexpected end of stream");
        buffer = buffer.Slice(length);
    }
}
```

<!--
### 3.1. LZMA SDK で規定されているファイルフォーマットの場合
-->
### 3.1. For the file format specified by the LZMA SDK

<!--
#### 3.1.1. デコードの場合
-->
#### 3.1.1. For decoding

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // Set the input stream
Stream outStream = ... ; // Set the output stream
Byte[] headerData = new Byte[LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE + sizeof(UInt64)];
ReadBytes(inStream, headerData); // Read the header part
Span<Byte> contentProperty = new Span<Byte>(headerData, 0, LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE); // Get the content property part
UInt64 uncompressedDataLength = BitConverter.ToUInt64(headerData, LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE); // Get the size of the data before compression
using (LzmaDecoder decoder = LzmaDecoder.Create(new LzmaDecoderProperties { FinishMode = true }, contentProperty)) // Create a decoder with content property
{
    decoder.Code(inStream, outStream, null, null, null); // Decode the body of the data
}
```

<!--
#### 3.1.2. エンコードの場合
-->
#### 3.1.2. For encoding

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // Set the input stream
Stream outStream = ... ; // Set the output stream
using (LzmaEncoder encoder = LzmaEncoder.Create(new LzmaEncoderProperties { Level = CompressionLevel.Normal })) // Create an encoder
{
    encoder.WriteCoderProperties(outStream); // Write content property
    outStream.Write(BitConverter.GetBytes((UInt64)uncompressedDataLength)); // Write the length of the data before compression
    encoder.Code(inStream, outStream, null, null, null); // Encode the body of the data
}
```

<!--
### 3.2. ZIP で規定されているファイルフォーマットの場合
-->
### 3.2. For the file format specified by ZIP

<!--
#### 3.2.1. デコードの場合
-->
#### 3.2.1. For decoding

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // Set the input stream
Stream outStream = ... ; // Set the output stream
Byte[] headerData = new Byte[sizeof(Byte) + sizeof(Byte) + sizeof(UInt16) + LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE];
ReadBytes(inStream, headerData); // Read the header part
Byte majorVersion = headerData[0]; // The major version is not used.
Byte minorVersion = headerData[1]; // The minor version is not used.
UInt16 contentPropertyLength = BitConverter.ToUInt16(headerData, 2); // Get the length of content property
if (contentPropertyLength != LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE) // Check the length of content property
    throw new Exception("Illegal LZMA format");
Span<Byte> contentProperty = new Span<Byte>(headerData, 4, LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE); // Get the content property part.
using (LzmaDecoder decoder = LzmaDecoder.Create(new LzmaDecoderProperties { FinishMode = true }, contentProperty)) // Create a decoder with content property
{
    decoder.Code(inStream, outStream, null, null, null); // Decode the body of the data
}
```

<!--
#### 3.2.2. エンコードの場合
-->
#### 3.2.2. For encoding

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Byte majorVersion = ... ; // Set the major version of the LZMA SDK.
Byte minorVersion = ... ; // Set the minor version of the LZMA SDK.
Stream inStream = ... ; // Set the input stream
Stream outStream = ... ; // Set the output stream
using (LzmaEncoder encoder = LzmaEncoder.Create(new LzmaEncoderProperties { Level = CompressionLevel.Normal })) // Create an encoder
{
    outSteram.WriteByte(majorVersion); // Write a major version of the LZMA SDK
    outSteram.WriteByte(minorVersion); // Write a minor version of the LZMA SDK
    outSteram.WriteByte((Byte)(LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE >> 0)); // Write the low-order byte of the content property length
    outSteram.WriteByte((Byte)(LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE >> 8)); // Write the high-order byte of the content property length
    encoder.WriteCoderProperties(outStream); // Write content property
    encoder.Code(inStream, outStream, null, null, null); // Encode the body of the data
}
```

<!--
## 4. 注意事項
-->
## 4. Cautions

<!--
- このドキュメントは、7-zip 21.07 および LZMA SDK 19.00 の仕様およびソースコードに基づいて書かれています。
-->
- This document is based on the 7-zip 21.07 and LZMA SDK 19.00 specifications and source code.
<!--
- このドキュメントには、筆者の考察などの、純粋な事実以外の事柄についても記載されています。そしてそれらについては、7-zipの開発者の見解とは必ずしも同一ではない可能性があることを留意してください。
-->
- This document also describes things other than pure facts, such as my thoughts. And keep in mind that they may not always be the same as the 7-zip developer's view.
<!--
- 誤った記述や誤解を招く表現はなるべく避けるように注意してはいますが、万が一このドキュメントを参照したことにより何らかの被害が生じたとしても筆者は(そしてもちろん7-zipの開発者も)責任を負いかねますので、ご注意ください。
-->
― I am careful to avoid erroneous descriptions and misleading expressions as much as possible.
However, please note that I (and the 7-zip developers) are not responsible for any damage caused by referring to this document.

<!--
[^1]: *コンテンツプロパティ*という用語は、本ソフトウェアの作者による造語です。7-zipのファイルフォーマット仕様書やソースコードでは単に*プロパティ*と記載されていますが、ICompressSetCoderPropertiesインターフェースなどで使用される同様の用語と紛らわしいことがあるため、本ソフトウェアでは*コンテンツプロパティ*と称することにしました。
-->
[^1]: The term *content property* was coined by the author of this software. In the 7-zip file format specification and source code, it is simply described as "property". However, the term "property" can be confusing with similar terms used in the `ICompressSetCoderProperties` interface, etc., so I chose to call it *content property* in this software.

<!--
[^2]: 筆者には正確にはわかりませんが、LZMAに限って言えば7-zipの開発者側で以下のような経緯があったのではないかという推測をしています。
    1. まず、LZMA SDKでのファイルフォーマットが規定された。
    2. 次に、ZIPファイルでのLZMA圧縮のサポートが検討された。
    3. ZIPファイルでは元々ZIPとしてのヘッダに「圧縮前のデータの長さ」を保持しているため、LZMA SDKのファイルフォーマットにおける「エンコード前のデータの長さ」の8バイトのフィールドは全く不要であるとの判断がされた。
    4. ZIPファイルでのLZMA圧縮フォーマットの規定の決定にあたって、不要な8バイトのフィールドが削除された。それとともに、将来にあるかもしれないフォーマット変更に対応できるように、圧縮を行ったコーデックで使用されているLZMA SDKのバージョンのフィールドが追加された。(実際のところ、LZMA SDK のバージョンのフィールドが正しく運用されているとは言い難いように思えますが・・・)
    5. 7-zipにおいて、LZMA SDK形式とZIP形式の両方を同一コーデックで処理できるように検討が行われたが、ヘッダ部分が異なるためにヘッダ部分の読み書きのコードはLZMAコーデックとは分離されてアプリケーションレベルで記述されることになった。
    6. しかし、ヘッダ部分にはデコードの際に重要となる*コンテンツプロパティ*が含まれており、*コンテンツプロパティ*の内容をアプリケーションに対して隠蔽しつつアプリケーションがヘッダーを読み書きできるようにするために`ICompressWriteCoderProperties`インターフェースと`ICompressSetDecoderProperties2`インターフェースが用意された。
-->
[^2]: I don't know exactly, but as far as LZMA is concerned, I presume that the 7-zip developer had the following background.
    1. First, the file format in the LZMA SDK was specified.
    2. Next, support for LZMA compression on ZIP files was considered.
    3. The ZIP file originally holds the "length of data before compression" in the header. Therefore, it was decided that the 8-byte field of "data length before encoding" in the file format of the LZMA SDK is completely unnecessary.
    4. Unnecessary 8-byte fields have been removed in determining the LZMA compression format definition for ZIP files. Along with that, a field for the version of the LZMA SDK used by the compressed codec has been added to accommodate possible format changes in the future. (Actually, the LZMA SDK version field seems to be ignored)
    5. The 7-zip developer has considered making it possible to process both the LZMA SDK format and the ZIP format with the same codec. However, since those formats have different header parts, the read / write code in the header part is written at the application level separately from the LZMA codec.
    6. However, the header part of the file format contained *content property* which is important for decoding. Therefore, 7-zip developers have provided the `ICompressWriteCoderProperties` interface and the` ICompressSetDecoderProperties2` interface. The purpose is to allow the application to read and write headers while hiding the contents of the *content property* from the application.
