using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0825_2
{
    internal class ThresholdingComparison
    {
        public static void CompareThresholding()
        {
            using (Mat testImg = CreateNoisyTestImage())
            {
                using(Mat simpleThresh = new Mat())
                using(Mat adaptiveMean = new Mat())
                using(Mat adaptiveGaussian = new Mat())
                using(Mat otsuThresh = new Mat())
                {
                    Cv2.Threshold(testImg, simpleThresh, 128, 255, ThresholdTypes.Binary);
                    Cv2.AdaptiveThreshold(testImg, adaptiveMean, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 11, 2);
                    Cv2.AdaptiveThreshold(testImg, adaptiveGaussian, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);
                    Cv2.Threshold(testImg, otsuThresh, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    Cv2.ImShow("original image", testImg);
                    Cv2.ImShow("simpleThresh", simpleThresh);
                    Cv2.ImShow("adaptiveMean", adaptiveMean);
                    Cv2.ImShow("adaptiveGaussian", adaptiveGaussian);
                    Cv2.ImShow("otsuThresh", otsuThresh);

                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
            }
        }

        private static Mat CreateNoisyTestImage()
        {
            Mat image = new Mat(400, 600, MatType.CV_8UC1, Scalar.White);

            Cv2.Rectangle(image, new Rect(350, 150, 150, 100), Scalar.Red, -1);
            Cv2.Circle(image, new Point(50, 50), 60, Scalar.Yellow, -1);
            Cv2.Line(image, new Point(150, 100), new Point(250, 350), Scalar.Blue, 1);

            AddNoise(image);
            return image;
        }

        private static void AddNoise(Mat image)
        {
            Random random = new Random();
            var indexer = image.GetGenericIndexer<byte>();

            // 소금 - 후추 노이즈
            for (int i = 0; i < 10000; i++)
            {
                int x = random.Next(0, image.Width);
                int y = random.Next(0, image.Height);

                // 소금 노이즈
                if (random.NextDouble() < 0.5)
                    indexer[y, x] = 255;

                // 후추 노이즈
                else
                    indexer[y, x] = 0;
            }
        }
    }
}
