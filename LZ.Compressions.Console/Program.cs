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
            new LZ78Compressor().Compress("ababcbababaa");
        }
    }
}
