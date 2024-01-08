言語: **日本語** | [English](AboutContentProperties_en.md)

# *コンテンツプロパティ*[^1]について

## 1. *コンテンツプロパティ*とは何か？

### 1.1. *コンテンツプロパティ*の概要

*コンテンツプロパティ*とは、7-zipでサポートされているいくつかのコーデックにおいて、エンコードされたデータを正しくデコードするために必要となるバイトデータです。

アプリケーションの開発者は、*コンテンツプロパティ*はデコードに必要なある長さのバイトデータであることだけを知っていればよく、その内容が何を意味するかを理解する必要はありません。
しかし、*コンテンツプロパティ*は、7-zipが提供するコーデックによってエンコードされたデータには含まれておらず、かつデコードの際には必要なデータであるので、アプリケーションは*コンテンツプロパティ*を正しく管理する必要があります。

### 1.2. 7-zip では*コンテンツプロパティ*はどのように使用されているか

具体的には、*コンテンツプロパティ*とは、7-zipのソースコードにおける以下のインターフェースで使用されるバイト列のことです。
- `ICompressWriteCoderProperties`
- `ICompressSetDecoderProperties2`

`ICompressWriteCoderProperties`インターフェースは、指定されたストリームに*コンテンツプロパティ*を書き込むことをエンコーダーに指示するメソッドを含んでいます。

また、`ICompressSetDecoderProperties2`インターフェースは*コンテンツプロパティ*をデコーダーに設定するメソッドを含んでいます。

これらのインターフェースを実装しているコーデックをアプリケーションが利用する場合は、*コンテンツプロパティ*がアプリケーションによって適切に管理されなければなりません。
7-zip バージョン 21.07 において、その対象となるコーデックは以下の通りです。
- LZMA
- LZMA2
- PPMd7 (PPMd vesion H)
- Rar1
- Rar2
- Rar3
- Rar5

## 2. 何故アプリケーションが*コンテンツプロパティ*を管理しなければならないのか？

本来ならば、少なくともアプリケーションにとっては、*コンテンツプロパティ*のようなデータはエンコードされたデータに含まれていてアプリケーションからはその存在が隠蔽されているのが望ましいことです。
そうはならなかった理由は、おそらくは同じコーデックでも複数のファイルフォーマットが存在することに関係している、と筆者は推測しています。

例えば、LZMAの場合の例を挙げます。

LZMA SDK に含まれているドキュメント `lzma.txt` およびその実装のソースコード `LzmaAlone.cs` によると、LZMAファイルフォーマットは以下のように規定されています。

| オフセット | 長さ | 内容 |
| ---: | ---: | :--- |
| 0 | 5バイト | *コンテンツプロパティ* |
| 5 | 8バイト | エンコード前のデータの長さ (リトルエンディアン)|
| 13 |  | 圧縮されたデータ |

その一方で、[ZIPファイルフォーマットの仕様書(APPNOTE.TXT)](https://pkware.cachefly.net/webdocs/casestudies/APPNOTE.TXT)によると、ZIPファイルに含まれるデータファイルがLZMA形式で圧縮されている場合のファイルフォーマットは以下のように規定されています。(実際に7-zipでのZIPアーカイブファイルの実装でもこの仕様に従っています)

| オフセット | 長さ | 内容 |
| ---: | ---: | :--- |
| 0 | 1バイト | LZMA SDK のメジャーバージョン <br/> (7-zipによる実装では7-zipのメジャーバージョン) |
| 1 | 1バイト | LZMA SDK のマイナーバージョン <br/> (7-zipによる実装では7-zipのマイナーバージョン) |
| 2 | 2バイト | *コンテンツプロパティ* の長さ (リトルエンディアン) <br/> (常に0x0005) |
| 4 | 5バイト | *コンテンツプロパティ*|
| 9 |  | 圧縮されたデータ |

7-zipでは、これらのファイルフォーマットのうち「圧縮されたデータ」の部分は同一のコーデックを使用してエンコード/デコードしており共通のフォーマットですが、
御覧の通り、**これらのファイルフォーマットのヘッダー部分には互換性がありません**。

おそらくですが、7-zipの開発者はこのように同一のコーデックで複数のファイルフォーマットに対応するために、ヘッダー部分と圧縮されたデータ本体の部分を分離してヘッダー部分の処理はアプリケーションに任せたのではないでしょうか。[^2]

どのような経緯でそうなったかについては筆者の想像でしかありません。
しかし、結果として、LZMA (およびLZMA2、PPMd7など) では **ヘッダー部分の読み込みと書き込みはアプリケーションの責任によって行われる仕様になりました**。

## 3. アプリケーションは*コンテンツプロパティ* をどのように管理すればいいのか？

前述のように、少なくともLZMA (およびLZMA2、PPMd7など)のコーデックにおいては、エンコードされたデータのヘッダー部分の読み込みと書き込みはアプリケーションが行わなければなりません。
当然、ヘッダー部分のフォーマットはコーデックやファイルフォーマットにより異なるため、それぞれ異なる対応が必要です。

典型的なLZMAエンコーダ/デコーダのアプリケーションのサンプルコードを以下に挙げますので、参考にしてください。
その他のコーデックの場合も同じようなコードで処理できるはずですが、それぞれのファイルフォーマットのヘッダー部分の規定を理解し、それに従ってコードを修正する必要があります。[^3]

なお、`ReadBytes`関数が以下のように定義されているものとします。

```csharp
// bufferが満たされるまでinStreamからデータを読み込む。もし読み込みの途中でinStreamが終端に達したら例外が発生する。
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

### 3.1. LZMA SDK で規定されているファイルフォーマットの場合

#### 3.1.1. デコードの場合

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // 入力ストリームの設定
Stream outStream = ... ; // 出力ストリームの設定
Span<Byte> headerData = stackalloc Byte[LzmaDecoder.CONTENT_PROPERTY_SIZE + sizeof(UInt64)];
ReadBytes(inStream, headerData); // ヘッダー部分の読み込み
Span<Byte> contentProperty = headerData.Slice(0, LzmaDecoder.CONTENT_PROPERTY_SIZE); // コンテンツプロパティの部分の取得

// 圧縮前のデータの長さの取得 (リトルエンディアンの 64 ビット整数)
UInt64 uncompressedDataLength = (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 0] << (8 * 0);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 1] << (8 * 1);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 2] << (8 * 2);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 3] << (8 * 3);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 4] << (8 * 4);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 5] << (8 * 5);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 6] << (8 * 6);
uncompressedDataLength |= (UInt64)headerData[LzmaDecoder.CONTENT_PROPERTY_SIZE + 7] << (8 * 7);

using (LzmaDecoder decoder = LzmaDecoder.CreateDecoder(new LzmaDecoderProperties { FinishMode = true }, contentProperty)) // コンテンツプロパティを与えてデコーダーを生成
{
    decoder.Code(inStream, outStream, null, uncompressedDataLength, null); // データ本体のデコード
}
```

#### 3.1.2. エンコードの場合

```csharp
using SevenZip.Compression;
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // 入力ストリームの設定
Stream outStream = ... ; // 出力ストリームの設定
UInt64 uncompressedDataLength = ... ; // 入力ストリームから読み込む圧縮前のデータの長さの設定
using (LzmaEncoder encoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal })) // エンコーダーの生成
{
    encoder.WriteCoderProperties(outStream); // コンテンツプロパティの書き込み

    // 圧縮前のデータの長さの書き込み (リトルエンディアンの 64 ビット整数)
    {
        Span<Byte> uncompressedDataLengthBuffer = stackalloc Byte[sizeof(UInt64) / sizeof(Byte)];
        uncompressedDataLengthBuffer[0] = (Byte)(uncompressedDataLength >> (8 * 0));
        uncompressedDataLengthBuffer[1] = (Byte)(uncompressedDataLength >> (8 * 1));
        uncompressedDataLengthBuffer[2] = (Byte)(uncompressedDataLength >> (8 * 2));
        uncompressedDataLengthBuffer[3] = (Byte)(uncompressedDataLength >> (8 * 3));
        uncompressedDataLengthBuffer[4] = (Byte)(uncompressedDataLength >> (8 * 4));
        uncompressedDataLengthBuffer[5] = (Byte)(uncompressedDataLength >> (8 * 5));
        uncompressedDataLengthBuffer[6] = (Byte)(uncompressedDataLength >> (8 * 6));
        uncompressedDataLengthBuffer[7] = (Byte)(uncompressedDataLength >> (8 * 7));
        outStream.Write(uncompressedDataLengthBuffer);
    }

    encoder.Code(inStream, outStream, uncompressedDataLength, null, null); // データ本体のエンコード
}
```

### 3.2. ZIP で規定されているファイルフォーマットの場合

#### 3.2.1. デコードの場合

```csharp
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Stream inStream = ... ; // 入力ストリームの設定
Stream outStream = ... ; // 出力ストリームの設定
UInt64 uncompressedDataLength = ... ; // 出力ストリームへ書き込まれる伸長後のデータの長さの設定 (この値はZIPのセントラルディレクトリから取得されます)
Span<Byte> headerData = stackalloc Byte[sizeof(Byte) + sizeof(Byte) + sizeof(UInt16) + LzmaDecoder.CONTENT_PROPERTY_SIZE];
ReadBytes(inStream, headerData); // ヘッダ部分の読み込み
Byte majorVersion = headerData[0]; // 使用されない
Byte minorVersion = headerData[1]; // 使用されない
UInt16 contentPropertyLength = (UInt16)(((UInt32)headerData[2] << 0) | ((UInt32)headerData[3] << 8)); // コンテンツプロパティの長さの取得
if (contentPropertyLength != LzmaDecoder.CONTENT_PROPERTY_SIZE) // コンテンツプロパティの長さの検査
    throw new Exception("Illegal LZMA format");
Span<Byte> contentProperty = headerData.Slice(4, LzmaDecoder.CONTENT_PROPERTY_SIZE); // コンテンツプロパティの部分の取得
using (LzmaDecoder decoder = LzmaDecoder.CreateDecoder(new LzmaDecoderProperties { FinishMode = true }, contentProperty)) // コンテンツプロパティを与えてデコーダーを生成
{
    decoder.Code(inStream, outStream, null, uncompressedDataLength, null); // データ本体のデコード
}
```

#### 3.2.2. エンコードの場合

```csharp
using SevenZip.Compression;
using SevenZip.Compression.Lzma;
using System;
using System.IO;

...

Byte majorVersion = ... ; // LZMA SDK のメジャーバージョンの設定
Byte minorVersion = ... ; // LZMA SDK のマイナーバージョンの設定
Stream inStream = ... ; // 入力ストリームの設定
Stream outStream = ... ; // 出力ストリームの設定
UInt64 uncompressedDataLength = ... ; // 入力ストリームから読み込む圧縮前のデータの長さの設定
using (LzmaEncoder encoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal })) // エンコーダーの生成
{
    // コンテンツプロパティを除くヘッダ部分の書き込み
    {
        Span<Byte> headerBuffer = stackalloc Byte[sizeof(Byte) + sizeof(Byte) + sizeof(UInt16)];
        headerBuffer[0] = majorVersion; // LZMA SDK のメジャーバージョン
        headerBuffer[1] = minorVersion; // LZMA SDK のマイナーバージョン
        headerBuffer[2] = (Byte)(LzmaDecoder.CONTENT_PROPERTY_SIZE >> 0); // コンテンツプロパティの長さの下位バイト
        headerBuffer[3] = (Byte)(LzmaDecoder.CONTENT_PROPERTY_SIZE >> 8); // コンテンツプロパティの長さの上位バイト
        outStream.Write(headerBuffer);
    }
    encoder.WriteCoderProperties(outStream); // コンテンツプロパティの書き込み
    encoder.Code(inStream, outStream, uncompressedDataLength, null, null); // データ本体のエンコード
}
```

## 4. 注意事項

- このドキュメントは、7-zip 21.07 および LZMA SDK 19.00 の仕様およびソースコードに基づいて書かれています。
- このドキュメントには、筆者の考察などの、純粋な事実以外の事柄についても記載されています。そしてそれらについては、7-zipの開発者の見解とは必ずしも同一ではない可能性があることを留意してください。
- 誤った記述や誤解を招く表現はなるべく避けるように注意してはいますが、万が一このドキュメントを参照したことにより何らかの被害が生じたとしても筆者は(そしてもちろん7-zipの開発者も)責任を負いかねますので、ご注意ください。

[^1]: *コンテンツプロパティ*という用語は、本ソフトウェアの作者による造語です。7-zipのファイルフォーマット仕様書やソースコードでは単に*プロパティ*と記載されていますが、ICompressSetCoderPropertiesインターフェースなどで使用される同様の用語と紛らわしいことがあるため、本ソフトウェアでは*コンテンツプロパティ*と称することにしました。

[^2]: 筆者には正確にはわかりませんが、LZMAに限って言えば7-zipの開発者側で以下のような経緯があったのではないかという推測をしています。
    1. まず、LZMA SDK でのファイルフォーマットが規定された。
    2. 次に、ZIP ファイルでの LZMA 圧縮のサポートが検討された。
    3. ZIPファイルでは元々ZIPとしてのヘッダに「圧縮前のデータの長さ」を保持しているため、LZMA SDK のファイルフォーマットにおける「エンコード前のデータの長さ」の8バイトのフィールドは全く不要であるとの判断がされた。
    4. ZIPファイルでの LZMA 圧縮フォーマットの規定の決定にあたって、不要な8バイトのフィールドが削除された。それとともに、将来にあるかもしれないフォーマット変更に対応できるように、圧縮を行ったコーデックで使用されている LZMA SDK のバージョンのフィールドが追加された。(実際には、LZMA SDK のバージョンのフィールドは無視されているように思えます)
    5. 7-zip において、LZMA SDK 形式とZIP形式の両方を同一コーデックで処理できるように検討が行われたが、ヘッダ部分が異なるためにヘッダ部分の読み書きのコードは LZMA コーデックとは分離されてアプリケーションレベルで記述されることになった。
    6. しかし、ヘッダ部分にはデコードの際に重要となる*コンテンツプロパティ*が含まれており、*コンテンツプロパティ*の内容をアプリケーションに対して隠蔽しつつアプリケーションがヘッダーを読み書きできるようにするために`ICompressWriteCoderProperties`インターフェースと`ICompressSetDecoderProperties2`インターフェースが用意された。

[^3]: もちろん、あなたが既存のファイルフォーマットとの互換性を気にしないのであれば、あなたがヘッダー部分のフォーマットを自由に決めることができます。
