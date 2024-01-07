using System;
using System.IO;
using Palmtree.IO;

namespace Experiment
{
    public class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>")]
        public static void Main(string[] args)
        {
            // TODO: 7-zipのインストール方法とかREADMEの執筆
            var path = typeof(Program).Assembly.Location;
            var dir = Path.GetDirectoryName(path) ?? throw new Exception();
            var newPath1 = Path.Combine(dir, ".", "a.txt");
            var newPath2 = Path.Combine(dir, "..", "a.txt");
            var newPath3 = Path.Combine(dir, "..", "C:\\temp.txt");

            var fileInfo1 = new FilePath(newPath1).FullName;
            var fileInfo2 = new FilePath(newPath2).FullName;
            var fileInfo3 = new FilePath(newPath3).FullName;

            Console.WriteLine("完了しました。");
            Console.Beep();
            _ = Console.ReadLine();
        }
    }
}
