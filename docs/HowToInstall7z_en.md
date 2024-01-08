Language: [���{��](HowToInstall7z_ja.md) **English**

<!--
# `SevenZip.Compression.Wrapper.NET` ���� 7-zip ���g�p�\�ɂ�����@
-->
# How to enable 7-zip from `SevenZip.Compression.Wrapper.NET`

<!--
## 1. �T�v
-->
## 1. Overview

<!--
`SevenZip.Compression.Wrapper.NET` ���g�p���邽�߂ɂ́A7-zip ��K�؂ɃC���X�g�[�����Ȃ���΂Ȃ�܂���B
�{�e�ł́A7-zip �̃C���X�g�[���̕��@�ɂ��Đ������܂��B
-->
In order to use `SevenZip.Compression.Wrapper.NET`, 7-zip must be properly installed.
This article explains how to install 7-zip.

<!--
## 2. Windows �̏ꍇ
-->
## 2. For Windows

<!--
### 2.1 �W���I�ȃC���X�g�[�����@
-->
### 2.1 Standard installation method

<!--
�ȉ��̎菇�ŃC���X�g�[�����Ă��������B
-->
Please follow the steps below to install.

<!--
1. [7-zip �̌����T�C�g](https://7-zip.opensource.jp/) ����A���Ȃ��̃R���s���[�^�ɓK���� 7-zip �̃p�b�P�[�W���_�E�����[�h���Ă��������B
2. 7-zip ���C���X�g�[�����Ă��������B
3. 7-zip ���C���X�g�[�����ꂽ�t�H���_ (��: `C:\Program Files\7-Zip`) �� `7z.dll` �Ƃ������O�̃t�@�C�������邱�Ƃ��m�F���Ă��������B
4. 7-zip ���C���X�g�[�����ꂽ�t�H���_�� `PATH` ���ϐ��ɒǉ����Ă��������B
-->
1.Download the appropriate 7-zip package for your computer from [7-zip official website](https://7-zip.opensource.jp/).
2. Please install 7-zip.
3. Make sure there is a file named `7z.dll` in the folder where 7-zip is installed (e.g. `C:\Program Files\7-Zip`).
4. Add the folder where 7-zip is installed to your `PATH` environment variable.

<!--
### 2.2 �蓮�ŃC���X�g�[��������@
-->
### 2.2 Manual installation method

<!--
�ȉ��̂悤�ȏ󋵂̏ꍇ�A[2.1 �W���I�ȃC���X�g�[�����@](#21-�W���I�ȃC���X�g�[�����@) �Ŏ��������@�ł͂��܂������Ȃ��\��������܂��B
-->
In the following situations, the method shown in [2.1 Standard installation method](#21-Standard-installation-method) may not work.


<!--
+ `PATH` ���ϐ���ݒ肵�����Ȃ��A���邢�͐ݒ�ł��Ȃ��ꍇ
+ �����̃A�[�L�e�N�`�� (`x86` ����� `x64`) �� `SevenZip.Compression.Wrapper.NET` �𗘗p�������ꍇ[^1]
-->
+ If you don't want or can't set the `PATH` environment variable
+ If you want to use `SevenZip.Compression.Wrapper.NET` on multiple architectures (`x86` and `x64`) [^1]

<!--
�蓮�ŃC���X�g�[�����邽�߂ɂ́A�ȉ��̎菇�ɏ]���Ă��������B
-->
To install manually, follow the steps below.

<!--
1. [7-zip �̌����T�C�g](https://7-zip.opensource.jp/) ����A7-zip ���_�E�����[�h���Ă��������B
2. 7-zip ���C���X�g�[�����Ă��������B
3. 7-zip ���C���X�g�[�����ꂽ�t�H���_ (��: `C:\Program Files\7-Zip`) �� `7z.dll` �Ƃ������O�̃t�@�C�������邱�Ƃ��m�F���Ă��������B
4. `SevenZip.Compression.Wrapper.NET` �� �C���X�g�[������Ă���t�H���_���m�F���Ă��������B���̃t�H���_�ɂ� `Palmtree.SevenZip.Compression.Wrapper.NET.dll` �Ƃ������O�̃t�@�C��������͂��ł��B
5. `7z.dll` �� `SevenZip.Compression.Wrapper.NET` ���C���X�g�[������Ă���t�H���_�ɃR�s�[���Ă��������B
-->
1. Download 7-zip from [7-zip official website](https://7-zip.opensource.jp/).
1. Please install 7-zip.
1. Make sure there is a file named `7z.dll` in the folder where 7-zip is installed (e.g. `C:\Program Files\7-Zip`).
1. Please check the folder where `SevenZip.Compression.Wrapper.NET` is installed. There should be a file named `Palmtree.SevenZip.Compression.Wrapper.NET.dll` in that folder.
1. Copy `7z.dll` to the folder where `SevenZip.Compression.Wrapper.NET` is installed.<!--

<!--
�ȏ�ŃC���X�g�[���͊����ł��B
���� 7-zip ���g�p���Ȃ��ꍇ�̓A���C���X�g�[�����Ă����܂��܂���B
-->
The installation is now complete.
If you do not use 7-zip, you can uninstall it.

<!--
�Ȃ��A`SevenZip.Compression.Wrapper.NET` �𕡐��̃A�[�L�e�N�`���ŗ��p����ꍇ�́A`7z.dll` ���R�s�[����ۂɈȉ��̂悤�Ƀt�@�C������ς��Ă��������B
-->
If you want to use `SevenZip.Compression.Wrapper.NET` on multiple architectures, please change the file name as shown below when copying `7z.dll`.

<!--
+ `x86` �� 7-zip �� `7z.dll` �̏ꍇ => `7z.win_x86.dll`
+ `x64` �� 7-zip �� `7z.dll` �̏ꍇ => `7z.win_x64.dll`
-->
+ For `7z.dll` of `x86` version 7-zip => `7z.win_x86.dll`
+ For `7z.dll` of `x64` version 7-zip => `7z.win_x64.dll`

<!--
## 3. Linux �̏ꍇ
-->
## 3. For Linux

<!--
Linux �� 7-zip �͒P��̎��s�\�t�@�C���Ƃ��Ē񋟂���Ă��āA���C�u�����Ƃ��Ă͎g�p�ł��܂���B
���̂��߁A`SevenZip.Compression.Wrapper.NET` ���� 7-zip �𗘗p���邽�߂ɂ́A7-zip �̃\�[�X�R�[�h���g�p���ăr���h���Ȃ����K�v������܂��B
-->
7-zip for Linux is provided as a single executable file and cannot be used as a library.
Therefore, in order to use 7-zip from `SevenZip.Compression.Wrapper.NET`, you need to rebuild it using the 7-zip source code.

<!--
�ȍ~�ł́ALinux ��� gcc �Ȃǂ̊J���������邱�Ƃ�O��ɁA�r���h�̎菇�̐������s���܂��B
-->
From now on, we will explain the build procedure assuming that you have a development environment such as gcc on Linux.

<!--
### 3.1 �菇 (1) �ŐV�ł̃\�[�X�R�[�h����肷��B
-->
### 3.1 Steps (1) Obtain the latest version of the source code.

<!--
[7-zip �̃_�E�����[�h�y�[�W](https://7-zip.opensource.jp/download.html) ����A7-zip �̃\�[�X�R�[�h���_�E�����[�h���Ă��������B
`".tar.xz"` �ň��k����Ă�����̂������ł��傤�B
-->
Please download the 7-zip source code from the [7-zip download page](https://7-zip.opensource.jp/download.html).
It is best to use `".tar.xz"`.


<!--
### 3.2 �菇 (2) �\�[�X�R�[�h�̉�
-->
### 3.2 Step (2) Unzip the source code

<!--
�Ⴆ�΁A�J�����g�f�B���N�g����ɓW�J����ꍇ�́A�V�F������ȉ��̃R�}���h�����s���܂��B
-->
For example, to extract to the current directory, run the following command from the shell.

```
xz -dc 7z2301-src.tar.xz | tar xfv -
```

<!--
### 3.3 �菇 (3) makefile �̃R�s�[
-->
### 3.3 Step (3) Copy makefile

<!--
makefile ����肵�āA �\�[�X�t�@�C���̃f�B���N�g����ɃR�s�[���܂��B
-->
Obtain the `makefile` and copy it to the source file directory.

<!--
+ �R�s�[���� makefile => `x64` �p�Ȃ�� [`makefile_x64.gcc`](../7z/makefile_x64.gcc)�A`x86` �p�Ȃ�� [`makefile_x86.gcc`](../7z/makefile_x86.gcc)
+ �R�s�[�� => �\�[�X�t�@�C����̃f�B���N�g�� `CPP/7zip/Bundles/Format7zF/`
-->
+ Makefile to copy => [`makefile_x64.gcc`](../7z/makefile_x64.gcc) for `x64`, [`makefile_x86.gcc`](../7z/makefile_x86.gcc) for `x86`
+ Copy destination => directory on source file `CPP/7zip/Bundles/Format7zF/`

<!--
### 3.4 �菇 (4) make �̎��s
-->
### 3.4 Step (4) Run make

<!--
`makefile` ���R�s�[�����f�B���N�g���ɃJ�����g�f�B���N�g�����ړ�������A`make` �����s���܂��B
�ȉ��̗�� `x64` �ł̃��C�u�������r���h����ꍇ�̂��̂ł��B
-->
Change the current directory to the directory where you copied the `makefile`, then run `make`.
The following example is for building the `x64` version of the library.

```
make -f makefile_x64.gcc
```

<!--
�r���h����������ƁA�J�����g�f�B���N�g���� 7-zip�̃��C�u���� `lib7z.linux_x64.so` (x86�ł̏ꍇ�� `lib7z.linux_x86.so`) ���o���Ă���͂��ł��B
-->
Once the build is complete, the 7-zip library `lib7z.linux_x64.so` (`lib7z.linux_x86.so` for the `x86` version) should be created in the current directory.

<!--
�o���� `lib7z.linux_x64.so` (���邢�� `lib7z.linux_x86.so`) ���A`SevenZip.Compression.Wrapper.NET` ���C���X�g�[������Ă���f�B���N�g����ɃR�s�[���Ă��������B
-->
Copy the created `lib7z.linux_x64.so` (or `lib7z.linux_x86.so`) to the directory where `SevenZip.Compression.Wrapper.NET` is installed.

<!--
[^1]: �g�p���Ă���R���s���[�^�� `x64` �A�[�L�e�N�`���ł����Ă��A`SevenZip.Compression.Wrapper.NET` �𗘗p����A�v���P�[�V������ 32 �r�b�g�A�v���P�[�V�����ł���ꍇ�́A`x86` �p�� 7-zip ���K�v�ɂȂ�܂��B
-->
[^1]: Even if your computer has `x64` architecture, if your application that utilizes `SevenZip.Compression.Wrapper.NET` is a 32-bit application, you will need 7zip for `x86`.
