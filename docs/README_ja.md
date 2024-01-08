����: **���{��** [English](README.md)

# SevenZip.Compression.Wrapper.NET

## 0. �ڎ�

+ [1. �T�v](#1-�T�v)
+ [2. �{�\�t�g�E�F�A�̋@�\](#3-�{�\�t�g�E�F�A�̋@�\)
+ [3. �K�v�Ȋ�](#2-�K�v�Ȋ�)
+ [4. �ݒ�ɂ���](#4-�ݒ�ɂ���)
  + [4.1 7-zip �Ɋւ���ݒ�](#41-7-zip-�Ɋւ���ݒ�)
+ [5. �T���v���\�[�X�R�[�h](#5-�T���v���\�[�X�R�[�h)
  + [5.1 Deflate �A���S���Y���Ńt�@�C�������k����](#51-Deflate-�A���S���Y���Ńt�@�C�������k����)
  + [5.2 Deflate �A���S���Y���Ńt�@�C����L������ (1)](#52-Deflate-�A���S���Y���Ńt�@�C����L������-1)
  + [5.3 Deflate �A���S���Y���Ńt�@�C����L������ (2)](#53-Deflate-�A���S���Y���Ńt�@�C����L������-2)
  + [5.4 LZMA �A���S���Y���Ńt�@�C�������k����](#54-LZMA-�A���S���Y���Ńt�@�C�������k����)
  + [5.5 LZMA �A���S���Y���Ńt�@�C����L������ (1)](#55-LZMA-�A���S���Y���Ńt�@�C����L������-1)
  + [5.6 LZMA �A���S���Y���Ńt�@�C����L������ (2)](#56-LZMA-�A���S���Y���Ńt�@�C����L������-2)
  + [5.7 �i���󋵂̒ʒm���󂯂�N���X (`ProgressReporter`) �̎����T���v��](#57-�i���󋵂̒ʒm���󂯂�N���X-(`ProgressReporter`)-�̎����T���v��)
+ [6. ���C�Z���X](#6-���C�Z���X)
+ [7. ���ӎ���](#7-���ӎ���)
  + [7.1 PPMd �A���S���Y���̌݊����ɂ���](#71-PPMd-�A���S���Y���̌݊����ɂ���)
+ [8. �Ɛӎ���](#8-�Ɛӎ���)

## 1. �T�v

�{�\�t�g�E�F�A (`SevenZip.Compression.Wrapper.NET`) �́A7-zip ���񋟂��Ă���ꕔ�̋@�\ (�X�g���[���̈��k/�L��)�� .NET �A�v���P�[�V�������痘�p���邽�߂̃��C�u�����ł��B

## 2. �{�\�t�g�E�F�A�̋@�\

�{�\�t�g�E�F�A�̃N���X�𗘗p���邱�Ƃɂ��A7-zip �ɂ��f�[�^�X�g���[���̈��k����ѐL��������@�\�𗘗p���邱�Ƃ��o���܂��B
�{�\�t�g�E�F�A�ŃT�|�[�g����Ă��鈳�k/�L���͈ȉ��̒ʂ�ł��B

| ���k���� | ���k�̂��߂̃N���X | �L���̂��߂̃N���X |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |

�Ȃ� �{�\�t�g�E�F�A�͂����܂ŒP�̂̃f�[�^�X�g���[���̈��k/�L����������̂ł���A`.zip` �� `.7z` �Ȃǂ̏��ɂւ̃A�N�Z�X�̓T�|�[�g����Ă��Ȃ����Ƃɒ��ӂ��Ă��������B

## 3. �K�v�Ȋ�

| ���� | ���� |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET �����^�C�� | 7.0 / 8.0 |
| 7-zip | 7-zip 23.01 �œ���m�F�� |

## 4. �ݒ�ɂ���

### 4.1 7-zip �Ɋւ���ݒ�

�{�\�t�g�E�F�A�𗘗p���邽�߂ɂ́A7-zip ���C���X�g�[������K�v������܂��B
�܂��A����ɉ����āA�K�؂Ȑݒ���s���K�v������܂��B
�ڍׂɂ��ẮA"[`SevenZip.Compression.Wrapper.NET` ���� 7-zip ���g�p�\�ɂ�����@]( (HowToInstall7z_ja.md))" ���Q�Ƃ��Ă��������B

## 5. �T���v���\�[�X�R�[�h

### 5.1 Deflate �A���S���Y���Ńt�@�C�������k����

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

...


        // ����́A�t�@�C���̓��e�� Deflate �ň��k���ĕʂ̃t�@�C���֕ۑ����郁�\�b�h�ł��B
        public static void CompressWithDeflate(string uncompressedFilePath, string compressedFilePath)
        {
            // ���̓t�@�C�����J���܂��B
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // �o�̓t�@�C�����J���܂��B
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Deflate �G���R�[�_�I�u�W�F�N�g���쐬���܂��B
            using var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal });

            // ���k���J�n���܂��B
            deflateEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // �i���󋵂̕\�����s�v�ȏꍇ�ɂ́A"new ProgressReporter()" �̑���� "null" ���w�肵�܂��B
        }

```

### 5.2 Deflate �A���S���Y���Ńt�@�C����L������ (1)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // ����́ADeflate �ň��k���ꂽ�t�@�C����L�����ĕʂ̃t�@�C���ɕۑ����郁�\�b�h�ł��B
        public static void UncompressWithDeflate_1(string compressedFilePath, string uncompressedFilePath)
        {
            // ���̓t�@�C�����J���܂��B
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // �o�̓t�@�C�����J���܂��B
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Deflate �f�R�[�_�I�u�W�F�N�g���쐬���܂��B
            using var deflateDecoder = DeflateDecoder.CreateDecoder();

            // ���k���J�n���܂��B
            deflateDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // �i���󋵂̕\�����s�v�ȏꍇ�ɂ́A"new ProgressReporter()" �̑���� "null" ���w�肵�܂��B
        }

```

### 5.3 Deflate �A���S���Y���Ńt�@�C����L������ (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Deflate;

...

        // ����́ADeflate �ň��k���ꂽ�t�@�C����L�����郁�\�b�h�̕ʂ̃o�[�W�����ł��B
        public static Stream UncompressWithDeflate_2(string compressedFilePath)
        {
            // ���̃��\�b�h���Ԃ��� Stream �I�u�W�F�N�g����ǂݍ��ރf�[�^�͐L�����ꂽ�f�[�^�ł��B 
            return DeflateDecoder.CreateDecoderStream(new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None), null);
        }

```

### 5.4 LZMA �A���S���Y���Ńt�@�C�������k����

LZMA ����� LZMA2 �A���S���Y���ł̈��k�̍ۂɂ́A���k���s���O�ɁA�G���R�[�_�� `WriteCoderProperties()` ���\�b�h���Ăяo���Ĉ��k��̃t�@�C���ɏ����ȃw�b�_���������܂Ȃ��Ă͂Ȃ�܂���B
�{�\�t�g�E�F�A�ł͂��̏����ȃw�b�_�� **�R���e���c�v���p�e�B** �ƌď̂��Ă��܂��B
�R���e���c�v���p�e�B�̏ڍׂɂ��ẮA[�R���e���c�v���p�e�B�ɂ���](AboutContentProperties_ja.md) ���Q�Ƃ��Ă��������B

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Lzma;

...


        // ����́A�t�@�C���̓��e�� LZMA �ň��k���ĕʂ̃t�@�C���֕ۑ����郁�\�b�h�ł��B
        public static void CompressWithLzma(string uncompressedFilePath, string compressedFilePath)
        {
            // ���̓t�@�C�����J���܂��B
            using var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // �o�̓t�@�C�����J���܂��B
            using var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA �G���R�[�_�I�u�W�F�N�g���쐬���܂��B
            using var lzmaEncoder = LzmaEncoder.CreateEncoder(new LzmaEncoderProperties { Level = CompressionLevel.Normal, EndMarker = true });
            
            // LZMA �Ȃǂ������̃G���R�[�_�ł͂��̎菇���K�v�ł��B
            lzmaEncoder.WriteCoderProperties(outCompressedStream);

            // ���k���J�n���܂��B
            lzmaEncoder.Code(
                inUncompressedStream,
                outCompressedStream,
                (ulong)inUncompressedStream.Length,
                null,
                new ProgressReporter()); // �i���󋵂̕\�����s�v�ȏꍇ�ɂ́A"new ProgressReporter()" �̑���� "null" ���w�肵�܂��B
        }
```

### 5.5 LZMA �A���S���Y���Ńt�@�C����L������ (1)

LZMA ����� LZMA2 �A���S���Y���ł̐L���̍ۂɂ́A�܂����k����Ă���f�[�^�̐擪���珬���ȃw�b�_��ǂݍ��݁A�ǂݍ��񂾃w�b�_���f�R�[�_�ɗ^���Ȃ���΂Ȃ�܂���B
�{�\�t�g�E�F�A�ł͂��̏����ȃw�b�_�� **�R���e���c�v���p�e�B** �ƌď̂��Ă��܂��B
�R���e���c�v���p�e�B�̒��� (�o�C�g��) �͈��k�A���S���Y���̎�ނɂ���Č��܂��Ă���ALZMA�f�R�[�_�̏ꍇ�͒萔 `SevenZip.Compression.Lzma.LzmaDecoder.CONTENT_PROPERTY_SIZE` �ɒ�`����Ă��܂��B
�R���e���c�v���p�e�B�̏ڍׂɂ��ẮA[�R���e���c�v���p�e�B�ɂ���](AboutContentProperties_ja.md) ���Q�Ƃ��Ă��������B

```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // ����́ALZMA �ň��k���ꂽ�t�@�C����L�����ĕʂ̃t�@�C���ɕۑ����郁�\�b�h�ł��B
        public static void UncompressWithLzma_1(string compressedFilePath, string uncompressedFilePath)
        {
            // ���̓t�@�C�����J���܂��B
            using var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA �Ȃǂ������̃f�R�[�_�ł͂��̎菇���K�v�ł��B
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // �o�̓t�@�C�����J���܂��B
            using var outUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // LZMA �f�R�[�_�I�u�W�F�N�g���쐬���܂��B
            using var lzmaDecoder = LzmaDecoder.CreateDecoder(contentProperties);

            // ���k���J�n���܂��B
            lzmaDecoder.Code(
                inCompressedStream,
                outUncompressedStream,
                (ulong)inCompressedStream.Length,
                null,
                new ProgressReporter()); // �i���󋵂̕\�����s�v�ȏꍇ�ɂ́A"new ProgressReporter()" �̑���� "null" ���w�肵�܂��B
        }
```

### 5.6 LZMA �A���S���Y���Ńt�@�C����L������ (2)

```csharp
using System;
using System.IO;
using SevenZip.Compression.Lzma;

...

        // ����́ALZMA �ň��k���ꂽ�t�@�C����L�����郁�\�b�h�̕ʂ̃o�[�W�����ł��B
        // ���̃��\�b�h���Ԃ��� Stream �I�u�W�F�N�g����ǂݍ��ރf�[�^�͐L�����ꂽ�f�[�^�ł��B
        public static Stream UncompressWithLzma_2(string compressedFilePath)
        {
            // ���̓t�@�C�����J���܂��B
            var inCompressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // LZMA �Ȃǂ������̃f�R�[�_�ł͂��̎菇���K�v�ł��B
            Span<byte> contentProperties = stackalloc byte[LzmaDecoder.CONTENT_PROPERTY_SIZE];
            if (inCompressedStream.ReadBytes(contentProperties) != contentProperties.Length)
                throw new UnexpectedEndOfStreamException();

            // ���̃��\�b�h���Ԃ��� Stream �I�u�W�F�N�g����ǂݍ��ރf�[�^�͐L�����ꂽ�f�[�^�ł��B 
            return LzmaDecoder.CreateDecoderStream(inCompressedStream, null, contentProperties);
        }
```

### 5.7 �i���󋵂̒ʒm���󂯂�N���X (`ProgressReporter`) �̎����T���v��

```csharp
using System;

...

        // ���̃N���X�͈��k�̐i���󋵂��R���\�[���ɕ\�����邽�߂̂��̂ł��B
        // �K�������K�v�ł͂���܂���B
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


## 6. ���C�Z���X

�{�\�t�g�E�F�A�̃\�[�X�R�[�h�ɂ� MIT ���C�Z���X���K�p����܂��B

7-zip �̃��C�Z���X�ɂ��ẮA[7-zip �̌����T�C�g](https://www.7-zip.org/) ���Q�Ƃ��Ă��������B


## 7. ���ӎ���

### 7.1 PPMd �A���S���Y���̌݊����ɂ���

PPMd �A���S���Y���ɂ͌݊����̂Ȃ��������̃o�[�W���������݂��܂��B
�{�\�t�g�E�F�A�ŃT�|�[�g����Ă���̂́A**"PPMd version H"** �ƌĂ΂�Ă���A���S���Y���ł���A(�����炭) ����� 7-zip �� `.7z` �`���̏��ɂō̗p����Ă���A���S���Y���ł��B

����ɑ΂��āA`.zip` �ō̗p����Ă���̂� **"PPMd version I, Rev 1"** �ƌĂ΂�Ă���A���S���Y���ł��B
7-zip �� `.zip` �`���̏��ɂł� **"PPMd version I, Rev 1"** ����������Ă���̂ł����A����͖{�\�t�g�E�F�A�ł̓T�|�[�g����Ă��܂���B
���̗��R�́A7-zip �̃��C�u������ **"PPMd version I, Rev 1"** �̎������O���Ɍ��J���Ă��Ȃ�����ł��B

7-zip �̃��C�u�����ɂ����� **"PPMd version I, Rev 1"** �̎��������J����Ă��Ȃ����R�ɂ��ẮA[SourceForge.net �� 7-zip �̃t�H�[�����̋L��](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) ���Q�Ƃ��Ă��������B

## 8. �Ɛӎ���

�{�\�t�g�E�F�A�̗��p�ɂ����艽�炩�̕s���g���u�����������Ƃ��Ă��A�{�\�t�g�E�F�A�̊J���҂͈�؂̐ӔC����肩�˂܂��B���������������B
