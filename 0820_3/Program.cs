using OpenCvSharp;

namespace _0820_3
{
    // ROI와 픽셀 접근
    // ROI (Region Of Interset) : 관심 영역
    // 이미지에서 특정 부분만 선택
    // 전체 이미지를 처리하는 대신 필요한 부분만 처리
    // 성능 향상과 메모리 절약 효과


    internal class Program
    {
        static void Main(string[] args)
        {
            // Rect(x, y, width, height)
            Rect region = new Rect(100, 50, 500, 450);

            using (Mat original = Cv2.ImRead("img2.jpeg"))
            using (Mat roi = new Mat(original, region))
            {
                Cv2.ImShow("roi", roi);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }
        }
    }
}
