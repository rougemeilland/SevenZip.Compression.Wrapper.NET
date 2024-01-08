言語: **日本語** [English](README.md)

# SevenZip.Compression.Wrapper.NET

## 0. 目次

+ [1. 概要](#1-概要)
+ [2. 本ソフトウェアの機能](#2-本ソフトウェアの機能)
+ [3. 必要な環境](#3-必要な環境)
+ [4. 設定について](#4-設定について)
  + [4.1 7-zip に関する設定](#41-7-zip-に関する設定)
+ [5. サンプルソースコード](#5-サンプルソースコード)
  + [5.1 Deflate アルゴリズムでファイルを圧縮する](#51-Deflate-アルゴリズムでファイルを圧縮する)
  + [5.2 Deflate アルゴリズムでファイルを伸長する (1)](#52-Deflate-アルゴリズムでファイルを伸長する-1)
  + [5.3 Deflate アルゴリズムでファイルを伸長する (2)](#53-Deflate-アルゴリズムでファイルを伸長する-2)
  + [5.4 LZMA アルゴリズムでファイルを圧縮する](#54-LZMA-アルゴリズムでファイルを圧縮する)
  + [5.5 LZMA アルゴリズムでファイルを伸長する (1)](#55-LZMA-アルゴリズムでファイルを伸長する-1)
  + [5.6 LZMA アルゴリズムでファイルを伸長する (2)](#56-LZMA-アルゴリズムでファイルを伸長する-2)
  + [5.7 進捗状況の通知を受けるクラス (`ProgressReporter`) の実装サンプル](#57-進捗状況の通知を受けるクラス-ProgressReporter-の実装サンプル)
+ [6. ライセンス](#6-ライセンス)
+ [7. 注意事項](#7-注意事項)
  + [7.1 PPMd アルゴリズムの互換性について](#71-PPMd-アルゴリズムの互換性について)
+ [8. 免責事項](#8-免責事項)

## 1. 概要

本ソフトウェア (`SevenZip.Compression.Wrapper.NET`) は、7-zip が提供している一部の機能 (ストリームの圧縮/伸長)を .NET アプリケーションから利用するためのライブラリです。

## 2. 本ソフトウェアの機能

本ソフトウェアのクラスを利用することにより、7-zip によるデータストリームの圧縮および伸長をする機能を利用することが出来ます。
本ソフトウェアでサポートされている圧縮/伸長の方式は以下の通りです。

| 圧縮方式 | 圧縮のためのクラス | 伸長のためのクラス |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |

なお 本ソフトウェアはあくまで単体のデータストリームの圧縮/伸長をするものであり、`.zip` や `.7z` などの書庫へのアクセスはサポートされていないことに注意してください。

## 3. 必要な環境

| 項目 | 条件 |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET ランタイム | 7.0 / 8.0 |
| 7-zip | 7-zip 23.01 で動作確認済 |

## 4. 設定について

### 4.1 7-zip に関する設定

本ソフトウェアを利用するためには、7-zip をインストールする必要があります。
また、それに加えて、適切な設定を行う必要があります。
詳細については、"[`SevenZip.Compression.Wrapper.NET` から 7-zip を使用可能にする方法](HowToInstall7z_ja.md)" を参照してください。

## 5. サンプルソースコード

### 5.1 Deflate アルゴリズムでファイルを圧縮する

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

...


        // これは、ファイルの内容を Deflate で圧縮して別のファイルへ保存するメソッドです。
        public static void CompressWithDeflate(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Deflate エンコーダオブジェクトを作成します。
            using var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal });

            // 圧縮を開始します。
            deflateEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

```

### 5.2 Deflate アルゴリズムでファイルを伸長する (1)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // これは、Deflate で圧縮されたファイルを伸長して別のファイルに保存するメソッドです。
        public static void UncompressWithDeflate_1(string compressedFilePath, string uncompressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Deflate デコーダオブジェクトを作成します。
            using var deflateDecoder = DeflateDecoder.CreateDecoder();

            // 圧縮を開始します。
            deflateDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }

```

### 5.3 Deflate アルゴリズムでファイルを伸長する (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // これは、Deflate で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
        {
            // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。 
            return DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);
        }

```

### 5.4 LZMA アルゴリズムでファイルを圧縮する

LZMA および LZMA2 アルゴリズムでの圧縮の際には、圧縮を行う前に、エンコーダの `WriteCoderProperties()` メソッドを呼び出して圧縮先のファイルに小さなヘッダを書き込まなくてはなりません。
本ソフトウェアではこの小さなヘッダを **コンテンツプロパティ** と呼称しています。
コンテンツプロパティの詳細については、[コンテンツプロパティについて](AboutContentProperties_ja.md) を参照してください。

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Lzma;

...


        // これは、ファイルの内容を LZMA で圧縮して別のファイルへ保存するメソッドです。
        public static void CompressWithLzma(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // 出力ファイルを開きます。
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA エンコーダオブジェクトを作成します。
            using var lzmaEncoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal, EndMarker = true });
            
            // LZMA などいくつかのエンコーダではこの手順が必要です。
            lzmaEncoder.WriteCoderProperties(outCompressedStream);

            // 圧縮を開始します。
            lzmaEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }
```

### 5.5 LZMA アルゴリズムでファイルを伸長する (1)

LZMA および LZMA2 アルゴリズムでの伸長の際には、まず圧縮されているデータの先頭から小さなヘッダを読み込み、読み込んだヘッダをデコーダに与えなければなりません。
本ソフトウェアではこの小さなヘッダを **コンテンツプロパティ** と呼称しています。
コンテンツプロパティの長さ (バイト数) は圧縮アルゴリズムの種類によって決まっており、LZMAデコーダの場合は定数 `SevenZip.Compression.Lzma.LzmaDecoder.CONTENT_PROPERTY_SIZE` に定義されています。
コンテンツプロパティの詳細については、[コンテンツプロパティについて](AboutContentProperties_ja.md) を参照してください。

```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // これは、LZMA で圧縮されたファイルを伸長して別のファイルに保存するメソッドです。
        public static void UncompressWithLzma_1(string compressedFilePath, string uncompressedFilePath)
        {
            // 入力ファイルを開きます。
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA などいくつかのデコーダではこの手順が必要です。
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // 出力ファイルを開きます。
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA デコーダオブジェクトを作成します。
            using var lzmaDecoder = LzmaDecoder.CreateDecoder(contentProperties);

            // 圧縮を開始します。
            lzmaDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
        }
```

### 5.6 LZMA アルゴリズムでファイルを伸長する (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // これは、LZMA で圧縮されたファイルを伸長するメソッドの別のバージョンです。
        // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。
        public static Stream UncompressWithLzma_2(string compressedFilePath)
        {
            // 入力ファイルを開きます。
            var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA などいくつかのデコーダではこの手順が必要です。
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // このメソッドが返した Stream オブジェクトから読み込むデータは伸長されたデータです。 
            return LzmaDecoder.CreateDecoderStream(inCompressedStream, null, contentProperties);
        }
```

### 5.7 進捗状況の通知を受けるクラス (`ProgressReporter`) の実装サンプル

```csharp
using System;

...

        // このクラスは圧縮の進捗状況をコンソールに表示するためのものです。
        // 必ずしも必要ではありません。
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


## 6. ライセンス

本ソフトウェアのソースコードには MIT ライセンスが適用されます。

7-zip のライセンスについては、[7-zip の公式サイト](https://www.7-zip.org/) を参照してください。


## 7. 注意事項

### 7.1 PPMd アルゴリズムの互換性について

PPMd アルゴリズムには互換性のないいくつかのバージョンが存在します。
本ソフトウェアでサポートされているのは、**"PPMd version H"** と呼ばれているアルゴリズムであり、(おそらく) これは 7-zip の `.7z` 形式の書庫で採用されているアルゴリズムです。

それに対して、`.zip` で採用されているのは **"PPMd version I, Rev 1"** と呼ばれているアルゴリズムです。
7-zip の `.zip` 形式の書庫では **"PPMd version I, Rev 1"** も実装されているのですが、それは本ソフトウェアではサポートされていません。
その理由は、7-zip のライブラリが **"PPMd version I, Rev 1"** の実装を外部に公開していないからです。

7-zip のライブラリにおいて **"PPMd version I, Rev 1"** の実装が公開されていない理由については、[SourceForge.net の 7-zip のフォーラムの記事](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) を参照してください。

## 8. 免責事項

本ソフトウェアの利用にあたり何らかの不具合やトラブルが生じたとしても、本ソフトウェアの開発者は一切の責任を取りかねます。ご了承ください。
