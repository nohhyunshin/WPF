using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0825_4
{
    internal class BasicContour
    {
        public static void BasicContourDemo()
        {
            Mat src = Cv2.ImRead("img1.jpeg");
            Mat gray = new Mat();
            Mat binary = new Mat();

            // 전처리 (GrayScale > 이진화)
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, binary, 127, 255, ThresholdTypes.Binary);

            // Contour 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 원본이 바뀌면 안 되니까 복사
            Mat result = src.Clone();

            Random random = new Random();
            for (int i = 0; i < contours.Length; i++)
            {
                Scalar color = new Scalar(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));

                Cv2.DrawContours(result, contours, i, color, 2);

            }
            Cv2.ImShow("original", src);
            Cv2.ImShow("binary", binary);
            Cv2.ImShow("contour", result);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
