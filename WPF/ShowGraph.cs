using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    internal class ShowGraph
    {
        public static void ShowGraphDemo() 
        {
            var csvRead = new FileStream($"DefectiveList.csv", FileMode.Open, FileAccess.Read);
        }
    }
}
