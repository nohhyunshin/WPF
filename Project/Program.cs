using System;
using OpenCvSharp;

namespace Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 데이터셋 폴더 경로
            string dataPath = @"C:\Users\user\Downloads\NEU Metal Surface Defects Data";
            string[] files = Directory.GetFiles(dataPath, "*.bmp");

            foreach(var file in files)
            {
                Console.Write($"{file} 있음!");
            }

            
        }
    }
}
