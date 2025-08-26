using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    internal class FindDefection
    {
        public static void FindDefection(string imgPath)
        {
            // 이미지 읽어 오기
            Mat src = Cv2.ImRead(imgPath);

        }
    }
}
