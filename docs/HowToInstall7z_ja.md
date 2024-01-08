言語: **日本語** [English](./HowToInstall7z_en.md)

# `SevenZip.Compression.Wrapper.NET` から 7-zip を使用可能にする方法

## 1. 概要

`SevenZip.Compression.Wrapper.NET` を使用するためには、7-zip を適切にインストールしなければなりません。
本稿では、7-zip のインストールの方法について説明します。

## 2. Windows の場合

### 2.1 標準的なインストール方法

以下の手順でインストールしてください。

1. [7-zip の公式サイト](https://7-zip.opensource.jp/) から、あなたのコンピュータに適した 7-zip のパッケージをダウンロードしてください。
2. 7-zip をインストールしてください。
3. 7-zip がインストールされたフォルダ (例: `C:\Program Files\7-Zip`) に `7z.dll` という名前のファイルがあることを確認してください。
4. 7-zip がインストールされたフォルダを `PATH` 環境変数に追加してください。

### 2.2 手動でインストールする方法

以下のような状況の場合、[2.1 標準的なインストール方法](#21-標準的なインストール方法) で示した方法ではうまくいかない可能性があります。

+ `PATH` 環境変数を設定したくない、あるいは設定できない場合
+ 複数のアーキテクチャ (`x86` および `x64`) で `SevenZip.Compression.Wrapper.NET` を利用したい場合[^1]

手動でインストールするためには、以下の手順に従ってください。

1. [7-zip の公式サイト](https://7-zip.opensource.jp/) から、7-zip をダウンロードしてください。
2. 7-zip をインストールしてください。
3. 7-zip がインストールされたフォルダ (例: `C:\Program Files\7-Zip`) に `7z.dll` という名前のファイルがあることを確認してください。
4. `SevenZip.Compression.Wrapper.NET` が インストールされているフォルダを確認してください。そのフォルダには `Palmtree.SevenZip.Compression.Wrapper.NET.dll` という名前のファイルがあるはずです。
5. `7z.dll` を `SevenZip.Compression.Wrapper.NET` がインストールされているフォルダにコピーしてください。

以上でインストールは完了です。
もし 7-zip を使用しない場合はアンインストールしてもかまいません。

なお、`SevenZip.Compression.Wrapper.NET` を複数のアーキテクチャで利用する場合は、`7z.dll` をコピーする際に以下のようにファイル名を変えてください。

+ `x86` 版 7-zip の `7z.dll` の場合 => `7z.win_x86.dll`
+ `x64` 版 7-zip の `7z.dll` の場合 => `7z.win_x64.dll`

## 3. Linux の場合

Linux 版 7-zip は単一の実行可能ファイルとして提供されていて、ライブラリとしては使用できません。
そのため、`SevenZip.Compression.Wrapper.NET` から 7-zip を利用するためには、7-zip のソースコードを使用してビルドしなおす必要があります。

以降では、Linux 上に gcc などの開発環境があることを前提に、ビルドの手順の説明を行います。

### 3.1 手順 (1) 最新版のソースコードを入手する。

[7-zip のダウンロードページ](https://7-zip.opensource.jp/download.html) から、7-zip のソースコードをダウンロードしてください。
`".tar.xz"` で圧縮されているものがいいでしょう。

### 3.2 手順 (2) ソースコードの解凍


例えば、カレントディレクトリ上に展開する場合は、シェルから以下のコマンドを実行します。

```
xz -dc 7z2301-src.tar.xz | tar xfv -
```

### 3.3 手順 (3) `makefile` のコピー

`makefile` を入手して、 ソースファイルのディレクトリ上にコピーします。

+ コピーする makefile => `x64` 用ならば [`makefile_x64.gcc`](../7z/makefile_x64.gcc)、`x86` 用ならば [`makefile_x86.gcc`](../7z/makefile_x86.gcc)
+ コピー先 => ソースファイル上のディレクトリ `CPP/7zip/Bundles/Format7zF/`

### 3.4 手順 (4) `make` の実行

`makefile` をコピーしたディレクトリにカレントディレクトリを移動した後、`make` を実行します。
以下の例は `x64` 版のライブラリをビルドする場合のものです。

```
make -f makefile_x64.gcc
```

ビルドが完了すると、カレントディレクトリに 7-zipのライブラリ `lib7z.linux_x64.so` (`x86` 版の場合は `lib7z.linux_x86.so`) が出来ているはずです。

出来た `lib7z.linux_x64.so` (あるいは `lib7z.linux_x86.so`) を、`SevenZip.Compression.Wrapper.NET` がインストールされているディレクトリ上にコピーしてください。


[^1]: 使用しているコンピュータが `x64` アーキテクチャであっても、`SevenZip.Compression.Wrapper.NET` を利用するアプリケーションが 32 ビットアプリケーションである場合は、`x86` 用の 7-zip が必要になります。
