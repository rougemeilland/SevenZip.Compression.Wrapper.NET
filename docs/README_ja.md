# Palmtree.SevenZip.Compression.Wrapper.NET

## 1. �T�v

`Palmtree.SevenZip.Compression.Wrapper.NET` �́A7-zip �̋@�\�𗘗p���ăo�C�i���[�f�[�^�X�g���[�������k���邢�͉𓀂��邽�߂̃��b�p�[���C�u�����ł��B

## 2. �K�v�Ȋ�

| ���� | ���� |
| --- | --- |
| CPU | x64 / x86 | 
| OS | Windows / Linux |
| .NET �����^�C�� | 7.0 / 8.0 |
| 7-zip | 7-zip 23.01 �œ���m�F�� |

## 3. �{���C�u�����̋@�\

�{���C�u�����̃N���X�𗘗p���邱�Ƃɂ��A7-zip �ɂ��f�[�^�X�g���[���̈��k����ѐL��������@�\�𗘗p���邱�Ƃ��o���܂��B
�{���C�u�����ŃT�|�[�g����Ă��鈳�k/�L���͈ȉ��̒ʂ�ł��B

| ���k���� | ���k�̂��߂̃N���X | �L���̂��߂̃N���X |
| --- | --- | --- |
| BZIP2 | `SevenZip.Compression.Bzip2.Bzip2Encoder` | `SevenZip.Compression.Bzip2.Bzip2Decoder` |
| Deflate | `SevenZip.Compression.Deflate.DeflateEncoder` | `SevenZip.Compression.Deflate.DeflateDecoder` |
| Deflate64 | `SevenZip.Compression.Deflate64.Deflate64Encoder` | `SevenZip.Compression.Deflate64.Deflate64Decoder` |
| LZMA | `SevenZip.Compression.Lzma.LzmaEncoder` | `SevenZip.Compression.Lzma.LzmaDecoder` |
| LZMA2 | `SevenZip.Compression.Lzma2.LzmaEncoder` | `SevenZip.Compression.Lzma2.LzmaDecoder` |
| PPMd version H | `SevenZip.Compression.Ppmd7.Ppmd7Encoder` | `SevenZip.Compression.Ppmd7.Ppmd7Decoder` |

�Ȃ� �{���C�u�����͂����܂ŒP�̂̃f�[�^�X�g���[���̈��k/�L����������̂ł���A`.zip` �� `.7z` �Ȃǂ̏��ɂւ̃A�N�Z�X�̓T�|�[�g����Ă��Ȃ����Ƃɒ��ӂ��Ă��������B

## 4. 7-zip �Ɋւ���ݒ�

�{���C�u�����𗘗p���邽�߂ɂ́A7-zip ���C���X�g�[�����邾���ł͂Ȃ��A�K�؂Ȑݒ���s���K�v������܂��B
�ڍׂɂ��ẮA"[`Palmtree.SevenZip.Compression.Wrapper.NET` ���� 7-zip ���g�p�\�ɂ�����@]( (HowToInstall7z_ja.md))" ���Q�Ƃ��Ă��������B

## 5. �T���v���\�[�X�R�[�h

### 5.1 deflate �A���S���Y���Ńt�@�C�������k����v���O�����̃T���v��

```csharp
using System;
using System.IO;
using SevenZip.Compression;
using SevenZip.Compression.Deflate;

namespace Sample
{
    public class SampleClass
    {
        // ���̃N���X�͈��k�̐i���󋵂��R���\�[���ɕ\�����邽�߂̂��̂ł��B
        // ����͕K�������K�v�ł͂���܂���B
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
            // ���̓t�@�C�����J���܂��B
            using (var inUncompressedStream = new FileStream(uncompressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            // �o�̓t�@�C�����J���܂��B
            using (var outCompressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            // deflate �G���R�[�_�I�u�W�F�N�g���쐬���܂��B���k���x���͒ʏ�Ƃ��܂��B
            using (var deflateEncoder = DeflateEncoder.CreateEncoder(new DeflateEncoderProperties { Level = CompressionLevel.Normal }))
            {
                // ���k���J�n���܂��B
                deflateEncoder.Code(
                    inUncompressedStream,
                    outCompressedStream,
                    (ulong)inUncompressedStream.Length,
                    null,
                    new ProgressReporter()); // �i���󋵂̕\�����s�v�ȏꍇ�ɂ́A"new ProgressReporter()" �̑���� "null" ���w�肵�܂��B
            }
        }
    }
}

```






## ���ӎ���

### PPMd �A���S���Y���ɂ���

PPMd �A���S���Y���ɂ͌݊����̂Ȃ��������̃o�[�W���������݂��܂��B
�{���C�u�����ŃT�|�[�g����Ă���̂́A"PPMd version H" �ƌĂ΂�Ă���A���S���Y���ł���A(�����炭) ����� 7-zip �� `.7z` �`���̏��ɂō̗p����Ă���A���S���Y���ł��B

����ɑ΂��āA`.zip` �ō̗p����Ă���̂� "PPMd version I, Rev 1" �ƌĂ΂�Ă���A���S���Y���ł��B
���̃A���S���Y���� 7-zip �� `.zip` �`���̏��ɂŗ��p����Ă���̂ł����A�{���C�u�����ł͂���̓T�|�[�g����Ă��܂���B
���̗��R�́A7-zip �̃��C�u������ "PPMd version I, Rev 1" �̃A���S���Y�����O���Ɍ��J���Ă��Ȃ�����ł��B
7-zip �̃��C�u�����ɂ����� "PPMd version I, Rev 1" �����J����Ă��Ȃ����R�ɂ��ẮA[SourceForge.net �� 7-zip �̃t�H�[�����̋L��](https://sourceforge.net/p/sevenzip/discussion/45798/thread/6b7a43b987/?limit=25#00b7) ���Q�Ƃ��Ă��������B
