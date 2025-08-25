using OpenCvSharp.Internal.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0822_2
{
    internal class BasicColorConvertion
    {
        public static void ConvertionDemo()
        {
            using (Mat colorImage = CreateColorTestImg())
            {
                using(Mat grayImage = new Mat())
                using(Mat hsvImage = new Mat())
                using(Mat labImage = new Mat())
                {
                    Cv2.CvtColor(colorImage, grayImage, ColorConversionCodes.BGR2GRAY);
                    Cv2.CvtColor(colorImage, hsvImage, ColorConversionCodes.BGR2HSV);
                    Cv2.CvtColor(colorImage, labImage, ColorConversionCodes.BGR2Lab);

                    Cv2.ImShow("1. Origin", colorImage);
                    Cv2.ImShow("2. Gray", grayImage);
                    Cv2.ImShow("3. HSV", hsvImage);
                    Cv2.ImShow("4. Lab", labImage);

                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
            }
        }
        
        private static Mat CreateColorTestImg()
        {
            Mat image = new Mat(400, 600, MatType.CV_8UC3, Scalar.White);

            var colorBlocks = new[]
            {
                new {Rect = new Rect(50, 50, 100, 100), Color = new Scalar(0, 0, 255), Name = "Red"},
                new {Rect = new Rect(200, 50, 100, 100), Color = new Scalar(0, 255, 0), Name = "Green"},
                new {Rect = new Rect(350, 50, 100, 100), Color = new Scalar(255, 0, 0), Name = "Blue"},
                new {Rect = new Rect(50, 150, 100, 100), Color = new Scalar(0,255, 255), Name = "Yellow"},
                new {Rect = new Rect(250, 250, 100, 100), Color = new Scalar(255, 255, 0), Name = "Cyan"},
            };

            foreach (var block in colorBlocks)
            {
                Cv2.Rectangle(image, block.Rect, block.Color, -1);

                Point textPos = new Point(block.Rect.X + 10, block.Rect.Y + 20);
                Cv2.PutText(image, block.Name, textPos, HersheyFonts.HersheySimplex, 0.5, Scalar.Black, 2);
            }
            
            // 제목
            Cv2.PutText(image, "Color Conversion Image", new Point(150, 30), HersheyFonts.HersheySimplex, 0.5, Scalar.Black, 2);

            return image;
        }
    }
}
