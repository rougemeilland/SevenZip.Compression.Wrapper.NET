# 7-zip Library

<!--
## 1. 概要
-->
## 1. Overview
<!--
このパッケージは、[7-zip の公式サイト](https://7-zip.opensource.jp/) から提供されているパッケージのライブラリ部分を抜き出したものです。
-->
This package is the extracted library part of the package provided from [7-zip official website](https://7-zip.opensource.jp/).

<!--
## 2. 必要な環境
-->
## 2. Required environment

+ Windows 10/8/7/Vista/XP/2016/2012/2008/2003/2000/NT
+ Linux (x86 or x64)

<!--
## 3. ライセンスについて
-->
## 3. About license

<!--
ほとんどのソースコードは **GNU LGPL** のライセンスです。
詳細については、本パッケージに同梱されている `License.txt` を参照してください。
-->
Most source code is licensed under the **GNU LGPL**.
For details, please refer to `License.txt` included in this package.

<!--
## 3. パッケージに含まれているファイル
-->
## 3. Files included in the package

<!--
| ファイル名 | オリジナルのファイル名 | ファイルの説明 |
| ---------- | ---------------| ---------------|
| `License.txt` | `License.txt` | 7-zip のライセンスについて書かれているドキュメントです。 |
| `7z.win_x64.dll` | `7z.dll` | 64ビット Windows (x64) 版のパッケージに含まれている `7z.dll` と同一のファイルです。 |
| `7z.win_x86.dll` | `7z.dll` | 32ビット Windows (x86) 版のパッケージに含まれている `7z.dll` と同一のファイルです。 |
| `lib7z.linux_x64.so` | ----- | 7-zip のソースコードをビルドしたものです。 |
| `lib7z.linux_x86.so` | ----- | 7-zip のソースコードをビルドしたものです。 |
| `makefile_x64.gcc` | ----- | Linux (x64) 用の 7-zip ライブラリをビルドするための makefile です。 |
| `makefile_x86.gcc` | ----- | Linux (x86) 用の 7-zip ライブラリをビルドするための makefile です。 |
-->

| File name | Original file name | Description |
| ---------- | ---------------| ---------------|
| `License.txt` | `License.txt` | This document describes the 7-zip license. |
| `7z.win_x64.dll` | `7z.dll` | This is the same file as `7z.dll` included in the 64-bit Windows (x64) version package. |
| `7z.win_x86.dll` | `7z.dll` | This is the same file as `7z.dll` included in the 32-bit Windows (x86) version package. |
| `lib7z.linux_x64.so` | ----- | This is a file built from the 7-zip source code. |
| `lib7z.linux_x86.so` | ----- | This is a file built from the 7-zip source code. |
| `makefile_x64.gcc` | ----- | This is a makefile for building the 7-zip library for Linux (x64). |
| `makefile_x86.gcc` | ----- | This is a makefile for building the 7-zip library for Linux (x86). |

<!--
## 4. Linux 用 7-zip ライブラリのビルドの方法について
-->
## 4. How to build the 7-zip library for Linux

<!--
Linux 版の場合、[7-zip の公式サイト](https://7-zip.opensource.jp/) から提供されているパッケージには単一の実行可能ファイルだけが含まれており、7-zip のライブラリは単体では提供されていません。-->
For the Linux version, the package provided by [7-zip official website](https://7-zip.opensource.jp/) contains only a single executable file, and 7-zip's Libraries are not provided separately.

<!--
しかし、ソースコードをビルドすることにより、ライブラリのバイナリを入手することが出来ます。
-->
However, we can obtain the library binaries by building the source code.

<!--
なお、ビルドには Linux 上での開発環境 (WSLでもOK) が必要です。
本稿では、既に Linux 上での開発環境が構築済みであるものとして、ビルドの手順の説明をします。
-->
Please note that a development environment on Linux (WSL is also OK) is required for building.
In this article, we will explain the build procedure assuming that you have already created a development environment on Linux.

<!--
### 4.1 手順 (1) 最新版のソースコードを入手する。
-->
### 4.1 Steps (1) Obtain the latest version of the source code.

<!--
[7-zip のダウンロードページ](https://7-zip.opensource.jp/download.html) から、7-zip のソースコードをダウンロードしてください。
`".tar.xz"` で圧縮されているものがいいでしょう。
-->
Please download the 7-zip source code from the [7-zip download page](https://7-zip.opensource.jp/download.html).
A file compressed with `".tar.xz"` is best.

<!--
### 4.2 手順 (2) ソースコードの解凍
-->
### 4.2 Step (2) Unzip source code

<!--
例えば、カレントディレクトリ上に展開する場合は、シェルから以下のコマンドを実行します。
-->
For example, to extract to the current directory, run the following command from the shell.

```
xz -dc 7z2301-src.tar.xz | tar xfv -
```

<!--
### 4.3 手順 (3) makefile のコピー
-->
### 4.3 Step (3) Copy makefile

<!--
本パッケージに含まれている makefile を ソースファイルのディレクトリ上にコピーします。
-->
Copy the makefile included in this package to the source file directory.

<!--
+ コピーする makefile => x64 用ならば `makefile_x64.gcc`、x86 用ならば `makefile_x86.gcc`
+ コピー先 => ソースファイル上のディレクトリ `CPP/7zip/Bundles/Format7zF/`
-->
+ makefile to copy => `makefile_x64.gcc` for x64, `makefile_x86.gcc` for x86
+ Copy destination => directory `CPP/7zip/Bundles/Format7zF/` on source files

<!--
### 4.4 手順 (4) make の実行
-->
### 4.4 Step (4) Run make

<!--
makefile をコピーしたディレクトリにカレントディレクトリを移動した後、make を実行します。
以下の例は x64 版のライブラリをビルドする場合のものです。
-->
Change the current directory to the directory where you copied the makefile, then run make.
The following example is for building the x64 version of the library.

```
make -f makefile_x64.gcc
```

<!--
ビルドが完了すると、カレントディレクトリに 7-zipのライブラリ `lib7z.linux_x64.so` (x86版の場合は `lib7z.linux_x86.so`) が出来ているはずです。
-->
Once the build is complete, the 7-zip library `lib7z.linux_x64.so` (`lib7z.linux_x86.so` for the x86 version) should be created in the current directory.
