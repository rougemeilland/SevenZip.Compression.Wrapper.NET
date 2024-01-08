Language: [日本語](HowToInstall7z_ja.md) **English**

<!--
# `SevenZip.Compression.Wrapper.NET` から 7-zip を使用可能にする方法
-->
# How to enable 7-zip from `SevenZip.Compression.Wrapper.NET`

<!--
## 1. 概要
-->
## 1. Overview

<!--
`SevenZip.Compression.Wrapper.NET` を使用するためには、7-zip を適切にインストールしなければなりません。
本稿では、7-zip のインストールの方法について説明します。
-->
In order to use `SevenZip.Compression.Wrapper.NET`, 7-zip must be properly installed.
This article explains how to install 7-zip.

<!--
## 2. Windows の場合
-->
## 2. For Windows

<!--
### 2.1 標準的なインストール方法
-->
### 2.1 Standard installation method

<!--
以下の手順でインストールしてください。
-->
Please follow the steps below to install.

<!--
1. [7-zip の公式サイト](https://7-zip.opensource.jp/) から、あなたのコンピュータに適した 7-zip のパッケージをダウンロードしてください。
2. 7-zip をインストールしてください。
3. 7-zip がインストールされたフォルダ (例: `C:\Program Files\7-Zip`) に `7z.dll` という名前のファイルがあることを確認してください。
4. 7-zip がインストールされたフォルダを `PATH` 環境変数に追加してください。
-->
1.Download the appropriate 7-zip package for your computer from [7-zip official website](https://7-zip.opensource.jp/).
2. Please install 7-zip.
3. Make sure there is a file named `7z.dll` in the folder where 7-zip is installed (e.g. `C:\Program Files\7-Zip`).
4. Add the folder where 7-zip is installed to your `PATH` environment variable.

<!--
### 2.2 手動でインストールする方法
-->
### 2.2 Manual installation method

<!--
以下のような状況の場合、[2.1 標準的なインストール方法](#21-標準的なインストール方法) で示した方法ではうまくいかない可能性があります。
-->
In the following situations, the method shown in [2.1 Standard installation method](#21-Standard-installation-method) may not work.


<!--
+ `PATH` 環境変数を設定したくない、あるいは設定できない場合
+ 複数のアーキテクチャ (`x86` および `x64`) で `SevenZip.Compression.Wrapper.NET` を利用したい場合[^1]
-->
+ If you don't want or can't set the `PATH` environment variable
+ If you want to use `SevenZip.Compression.Wrapper.NET` on multiple architectures (`x86` and `x64`) [^1]

<!--
手動でインストールするためには、以下の手順に従ってください。
-->
To install manually, follow the steps below.

<!--
1. [7-zip の公式サイト](https://7-zip.opensource.jp/) から、7-zip をダウンロードしてください。
2. 7-zip をインストールしてください。
3. 7-zip がインストールされたフォルダ (例: `C:\Program Files\7-Zip`) に `7z.dll` という名前のファイルがあることを確認してください。
4. `SevenZip.Compression.Wrapper.NET` が インストールされているフォルダを確認してください。そのフォルダには `Palmtree.SevenZip.Compression.Wrapper.NET.dll` という名前のファイルがあるはずです。
5. `7z.dll` を `SevenZip.Compression.Wrapper.NET` がインストールされているフォルダにコピーしてください。
-->
1. Download 7-zip from [7-zip official website](https://7-zip.opensource.jp/).
1. Please install 7-zip.
1. Make sure there is a file named `7z.dll` in the folder where 7-zip is installed (e.g. `C:\Program Files\7-Zip`).
1. Please check the folder where `SevenZip.Compression.Wrapper.NET` is installed. There should be a file named `Palmtree.SevenZip.Compression.Wrapper.NET.dll` in that folder.
1. Copy `7z.dll` to the folder where `SevenZip.Compression.Wrapper.NET` is installed.<!--

<!--
以上でインストールは完了です。
もし 7-zip を使用しない場合はアンインストールしてもかまいません。
-->
The installation is now complete.
If you do not use 7-zip, you can uninstall it.

<!--
なお、`SevenZip.Compression.Wrapper.NET` を複数のアーキテクチャで利用する場合は、`7z.dll` をコピーする際に以下のようにファイル名を変えてください。
-->
If you want to use `SevenZip.Compression.Wrapper.NET` on multiple architectures, please change the file name as shown below when copying `7z.dll`.

<!--
+ `x86` 版 7-zip の `7z.dll` の場合 => `7z.win_x86.dll`
+ `x64` 版 7-zip の `7z.dll` の場合 => `7z.win_x64.dll`
-->
+ For `7z.dll` of `x86` version 7-zip => `7z.win_x86.dll`
+ For `7z.dll` of `x64` version 7-zip => `7z.win_x64.dll`

<!--
## 3. Linux の場合
-->
## 3. For Linux

<!--
Linux 版 7-zip は単一の実行可能ファイルとして提供されていて、ライブラリとしては使用できません。
そのため、`SevenZip.Compression.Wrapper.NET` から 7-zip を利用するためには、7-zip のソースコードを使用してビルドしなおす必要があります。
-->
7-zip for Linux is provided as a single executable file and cannot be used as a library.
Therefore, in order to use 7-zip from `SevenZip.Compression.Wrapper.NET`, you need to rebuild it using the 7-zip source code.

<!--
以降では、Linux 上に gcc などの開発環境があることを前提に、ビルドの手順の説明を行います。
-->
From now on, we will explain the build procedure assuming that you have a development environment such as gcc on Linux.

<!--
### 3.1 手順 (1) 最新版のソースコードを入手する。
-->
### 3.1 Steps (1) Obtain the latest version of the source code.

<!--
[7-zip のダウンロードページ](https://7-zip.opensource.jp/download.html) から、7-zip のソースコードをダウンロードしてください。
`".tar.xz"` で圧縮されているものがいいでしょう。
-->
Please download the 7-zip source code from the [7-zip download page](https://7-zip.opensource.jp/download.html).
It is best to use `".tar.xz"`.


<!--
### 3.2 手順 (2) ソースコードの解凍
-->
### 3.2 Step (2) Unzip the source code

<!--
例えば、カレントディレクトリ上に展開する場合は、シェルから以下のコマンドを実行します。
-->
For example, to extract to the current directory, run the following command from the shell.

```
xz -dc 7z2301-src.tar.xz | tar xfv -
```

<!--
### 3.3 手順 (3) makefile のコピー
-->
### 3.3 Step (3) Copy makefile

<!--
makefile を入手して、 ソースファイルのディレクトリ上にコピーします。
-->
Obtain the `makefile` and copy it to the source file directory.

<!--
+ コピーする makefile => `x64` 用ならば [`makefile_x64.gcc`](../7z/makefile_x64.gcc)、`x86` 用ならば [`makefile_x86.gcc`](../7z/makefile_x86.gcc)
+ コピー先 => ソースファイル上のディレクトリ `CPP/7zip/Bundles/Format7zF/`
-->
+ Makefile to copy => [`makefile_x64.gcc`](../7z/makefile_x64.gcc) for `x64`, [`makefile_x86.gcc`](../7z/makefile_x86.gcc) for `x86`
+ Copy destination => directory on source file `CPP/7zip/Bundles/Format7zF/`

<!--
### 3.4 手順 (4) make の実行
-->
### 3.4 Step (4) Run make

<!--
`makefile` をコピーしたディレクトリにカレントディレクトリを移動した後、`make` を実行します。
以下の例は `x64` 版のライブラリをビルドする場合のものです。
-->
Change the current directory to the directory where you copied the `makefile`, then run `make`.
The following example is for building the `x64` version of the library.

```
make -f makefile_x64.gcc
```

<!--
ビルドが完了すると、カレントディレクトリに 7-zipのライブラリ `lib7z.linux_x64.so` (x86版の場合は `lib7z.linux_x86.so`) が出来ているはずです。
-->
Once the build is complete, the 7-zip library `lib7z.linux_x64.so` (`lib7z.linux_x86.so` for the `x86` version) should be created in the current directory.

<!--
出来た `lib7z.linux_x64.so` (あるいは `lib7z.linux_x86.so`) を、`SevenZip.Compression.Wrapper.NET` がインストールされているディレクトリ上にコピーしてください。
-->
Copy the created `lib7z.linux_x64.so` (or `lib7z.linux_x86.so`) to the directory where `SevenZip.Compression.Wrapper.NET` is installed.

<!--
[^1]: 使用しているコンピュータが `x64` アーキテクチャであっても、`SevenZip.Compression.Wrapper.NET` を利用するアプリケーションが 32 ビットアプリケーションである場合は、`x86` 用の 7-zip が必要になります。
-->
[^1]: Even if your computer has `x64` architecture, if your application that utilizes `SevenZip.Compression.Wrapper.NET` is a 32-bit application, you will need 7zip for `x86`.
