# Palmtree.SevenZip.Compression.Wrapper.NET

## 1. 概要

`Palmtree.SevenZip.Compression.Wrapper.NET` は、7-zip の機能を利用してバイナリーデータストリームを圧縮あるいは解凍するためのラッパーライブラリです。

## 2. 必要な環境

| 項目 | 条件 |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET ランタイム | 7.0 / 8.0 |
| 7-zip | 7-zip 23.01 で動作確認済 |

## 3. 本ライブラリの機能

本ライブラリのクラスを利用することにより、7-zip によるデータストリームの圧縮および伸長をする機能を利用することが出来ます。
本ライブラリでサポートされている圧縮/伸長は以下の通りです。

| 圧縮方式 | 圧縮のためのクラス | 伸長のためのクラス |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |

なお 本ライブラリはあくまで単体のデータストリームの圧縮/伸長をするものであり、`.zip` や `.7z` などの書庫へのアクセスはサポートされていないことに注意してください。

## 4. 7-zip に関する設定

本ライブラリを利用するためには、7-zip をインストールするだけではなく、適切な設定を行う必要があります。
詳細については、"[`Palmtree.SevenZip.Compression.Wrapper.NET` から 7-zip を使用可能にする方法]( (HowToInstall7z_ja.md))" を参照してください。

## 5. サンプルソースコード

### 5.1 deflate アルゴリズムでファイルを圧縮するプログラムのサンプル

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

namespace Sample
{
    public class SampleClass
    {
        // このクラスは圧縮の進捗状況をコンソールに表示するためのものです。
        // これは必ずしも必要ではありません。
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

        public static void CompressWithDeflate(string uncompressedFilePath, string compressedFilePath)
        {
            // 入力ファイルを開きます。
            using (var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            // 出力ファイルを開きます。
            using (var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            // deflate エンコーダオブジェクトを作成します。圧縮レベルは通常とします。
            using (var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal }))
            {
                // 圧縮を開始します。
                deflateEncoder.Code(
                    inUncompressedStream,
                    outCompressedStream,
                    (ulong)inUncompressedStream.Length,
                    null,
                    new ProgressReporter()); // 進捗状況の表示が不要な場合には、"new ProgressReporter()" の代わりに "null" を指定します。
            }
        }
    }
}

```






## 注意事項

### PPMd アルゴリズムについて

PPMd アルゴリズムには互換性のないいくつかのバージョンが存在します。
本ライブラリでサポートされているのは、"PPMd version H" と呼ばれているアルゴリズムであり、(おそらく) これは 7-zip の `.7z` 形式の書庫で採用されているアルゴリズムです。

それに対して、`.zip` で採用されているのは "PPMd version I, Rev 1" と呼ばれているアルゴリズムです。
このアルゴリズムも 7-zip の `.zip` 形式の書庫で利用されているのですが、本ライブラリではそれはサポートされていません。
その理由は、7-zip のライブラリが "PPMd version I, Rev 1" のアルゴリズムを外部に公開していないからです。
7-zip のライブラリにおいて "PPMd version I, Rev 1" が公開されていない理由については、[SourceForge.net の 7-zip のフォーラムの記事](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) を参照してください。
