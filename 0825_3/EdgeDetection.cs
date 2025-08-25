using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0825_3
{
    internal class EdgeDetection
    {
        public static void EdgeDetectionDemo()
        {
            Mat src = Cv2.ImRead("img4.jpeg");

            if (src.Empty())
            {
                return;
            }

            // 결과 저장용 Mat 생성
            Mat sobel = new Mat();
            Mat sobelX = new Mat();
            Mat sobelY = new Mat();
            Mat scharr = new Mat();
            Mat scharrX = new Mat();
            Mat scharrY = new Mat();
            Mat laplacian = new Mat();
            Mat canny = new Mat();

            // sobel 미분 적용
            // X방향 sobel
            Cv2.Sobel(src, sobelX, MatType.CV_64F, 1, 0, ksize: 3);

            // Y방향 sobel
            Cv2.Sobel(src, sobelY, MatType.CV_64F, 0, 1, ksize: 3);

            // sobelX와 sobelY를 sobel에 합침
            Cv2.Magnitude(sobelX, sobelY, sobel);
            sobel.ConvertTo(sobel, MatType.CV_8U);

            // scharr 필터 적용
            // X방향 scharr
            Cv2.Scharr(src, scharrX, MatType.CV_64F, 1, 0);

            // Y방향 scharr
            Cv2.Scharr(src, scharrY, MatType.CV_64F, 0, 1);

            // scharrX와 scharrY를 scharr에 합침
            Cv2.Magnitude(scharrX, scharrY, scharr);
            scharr.ConvertTo(scharr, MatType.CV_8U);

            // laplacian 적용
            Cv2.Laplacian(src, laplacian, MatType.CV_64F, ksize: 3);
            laplacian.ConvertTo(laplacian, MatType.CV_8U);

            // canny 적용 : canny(입력값, 출력값, 하한선, 상하선)
            // 상한선과 하한선의 값에 따라 깔끔한 정도가 달라짐
            Cv2.Canny(src, canny, 70, 200);

            Cv2.ImShow("original", src);
            Cv2.ImShow("sobel", sobel);
            Cv2.ImShow("scharr", scharr);
            Cv2.ImShow("laplacian", laplacian);
            Cv2.ImShow("canny", canny);
  
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
