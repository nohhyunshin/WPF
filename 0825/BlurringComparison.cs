using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0825
{
    internal class BlurringComparison
    {
        public static void CompareBlurring()
        {
            using (Mat original = CreateNoisyTestImage())
            {
                using(Mat gaussian = new Mat())
                using(Mat box = new Mat())
                using(Mat median = new Mat())
                using(Mat bilateral = new Mat())
                {
                    ApplyBlurringMethods(original, gaussian, box, median, bilateral);

                    // 이미지 출력
                    DisplayBlur(original, gaussian, box, median, bilateral);

                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
            }
        }

        private static Mat CreateNoisyTestImage()
        {
            Mat image = Cv2.ImRead("img3.jpeg");

            //Cv2.Rectangle(image, new Rect(350, 150, 150, 100), Scalar.Red, -1);
            //Cv2.Circle(image, new Point(50, 50), 60, Scalar.Yellow, -1);
            //Cv2.Line(image, new Point(150, 100), new Point(250, 350), Scalar.LightBlue, 1);

            AddNoise(image);
            return image;
        }

        private static void AddNoise(Mat image)
        {
            Random random = new Random();
            var indexer = image.GetGenericIndexer<Vec3b>();

            // 소금 - 후추 노이즈
            for (int i = 0; i < 10000; i++)
            {
                int x = random.Next(0, image.Width);
                int y = random.Next(0, image.Height);

                if (random.NextDouble() < 0.5)
                {
                    indexer[y, x] = new Vec3b(255, 255, 255);
                }
                else
                {
                    indexer[y, x] = new Vec3b(0, 0, 0);
                }
            }
        }

        private static void ApplyBlurringMethods(Mat original, Mat gaussian, Mat box, Mat median, Mat bilateral)
        {
            // 커넬 : 값이 클수록 주변 픽셀을 더 많이 고려하고 이는 더 강한 블러 효과로 이어짐
            //      : 값이 작을수록 세밀한 블러링 가능
            // 이미지를 처리할 때 한 픽셀만 보는 게 아니라 주변 픽셀까지 포함한 작은 영역을 하나의 덩어리로? 사용
            Size kernelSize = new Size(15, 15);

            // 가우시안 블러
            Cv2.GaussianBlur(original, gaussian, kernelSize, 0);

            // 박스 필터
            Cv2.BoxFilter(original, box, MatType.CV_8UC3, kernelSize);

            // 메디안 필터
            Cv2.MedianBlur(original, median, 15);

            // 양방향 필터
            Cv2.BilateralFilter(original, bilateral, 15, 80, 80);
        }

        private static void DisplayBlur(Mat original, Mat gaussian, Mat box, Mat median, Mat bilateral)
        {
            Cv2.ImShow("original", original);
            Cv2.ImShow("gaussian", gaussian);
            Cv2.ImShow("box", box);
            Cv2.ImShow("median", median);
            Cv2.ImShow("bilateral", bilateral);
        }
    }
}
