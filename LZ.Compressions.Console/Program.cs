using LZ.Compressions.Core.Algorithms;
using System;
using System.ComponentModel;
using System.Text;

namespace LZ.Compressions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var str1 = "aaaaaaQWaQQQQQQQwwwwwaWWWWWZZZZZaa";
            var str2 = "aaaaaaQWaQQQQQQQwwwwwaWWWWWZZZZZaa";

            new RLECompressor().Decompress(null);
        }
    }
}
