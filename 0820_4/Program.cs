using OpenCvSharp;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace _0820_4
{
    // ROI와 픽셀 접근
    // ROI (Region Of Interset) : 관심 영역
    // 이미지에서 특정 부분만 선택
    // 전체 이미지를 처리하는 대신 필요한 부분만 처리
    // 성능 향상과 메모리 절약 효과

    // ROI 생성 방법
    // ① new Mat(original, rect) : Rect 사용
    // ② original[rowRange, colRange] : Range 사용

    // 메모리 공유 특성
    // ROI는 원본 데이터를 복사하지 않음
    // ROI는 수정 시 원본도 함께 수정됨
    // 메모리 효율적이지만 주의가 필요

    // 픽셀 접근 핵심
    // GetGenericIndexer 사용
    // indexer[y, x] : [행, 열] 순서 주의
    // 빠른 성능, 권장 방법

    // 데이터 타입 선택
    // 1채널 byte 또는 GetGenericIndexer<byte>()
    // 1채널 Vec3b 또는 GetGenericIndexer<Vec3b>()

    // ※ 주의할 점 ※
    // 범위 체크 : 이미지 크기를 벗어나는 접근 방지
    // ROI 메모리 공유 : 의도치 않은 원본 변경 주의

    internal class Program
    {
        static void Main(string[] args)
        {
            // Rect(x, y, width, height)
            Rect region = new Rect(100, 100, 200, 200);

            // using (Mat original = Cv2.ImRead("img2.jpeg"))
            using(Mat original = new Mat(500, 500, MatType.CV_8UC3, new Scalar(255, 0, 0)))
            using (Mat roi = new Mat(original, region))
            {
                // ROI는 원본 데이터를 복사하지 않음
                // 원본 데이터의 일부분을 가리키기만 함
                // 메모리 관리에 효율적이지만 주의해야 함!
                roi.SetTo(new Scalar(0, 0, 255));

                Cv2.ImShow("original", original);
                Cv2.ImShow("roi", roi);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

            // Range를 이용한 ROI
            OpenCvSharp.Range rowRange = new OpenCvSharp.Range(50, 200);
            OpenCvSharp.Range colRange = new OpenCvSharp.Range(100, 300);

            using (Mat original = Cv2.ImRead("img2.jpeg"))
            // using (Mat original = new Mat(500, 500, MatType.CV_8UC3, new Scalar(255, 0, 0)))
            using (Mat roi2 = original[rowRange, colRange])
            {
                Cv2.ImShow("original1", original);
                Cv2.ImShow("roi2", roi2);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

            // 픽셀 단위 접근
            // GetGenericIndexer
            Mat image1 = new Mat(500, 500, MatType.CV_8UC3);
            var indexer = image1.GetGenericIndexer<Vec3b>();

            int x = 0, y = 0;

            // 픽셀 읽기
            Vec3b pixel = indexer[300, 300];    // [행, 열]
            byte blue = pixel.Item0;
            byte green = pixel.Item1;
            byte red = pixel.Item2;

            // 픽셀 쓰기
            indexer[y, x] = new Vec3b(255, 0, 0);


            // At 함수
            Mat image2 = new Mat(500, 500, MatType.CV_8UC3);
            Vec3b pixel2 = image2.At<Vec3b>(y, x);              // 픽셀 읽기
            image2.Set<Vec3b>(y, x, new Vec3b(255, 0, 0));      // 픽셀 쓰기

            using (Mat original = Cv2.ImRead("img2.jpeg"))
            {
                int width = original.Width;         // 이미지의 너비
                int height = original.Height;       // 이미지의 높이

                // 이미지의 중심
                // 왼쪽 상단의 좌표는 중심 - (사각형의 너비 / 2)
                Rect centerRegion = new Rect(width / 2 - 50, height / 2 - 50, 100, 100);
                Mat roi1 = new Mat(original, centerRegion);

                roi1.SetTo(new Scalar(0, 255, 0));

                Cv2.ImShow("original", original);
                Cv2.ImShow("roi1", roi1);

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

            using (Mat original = CreateCheckBoard(400, 600))
            {
                Cv2.ImShow("original", original);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

            using (Mat original = CreatedCrossPattern(400, 400))
            {
                Cv2.ImShow("cross", original);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

            using (Mat original = CreatedQuadrant(400, 400))
            {
                Cv2.ImShow("quadrant", original);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }
        }

        private static Mat CreateCheckBoard(int height, int width)
        {
            Mat board = new Mat(height, width, MatType.CV_8UC3);
            var indexer = board.GetGenericIndexer<Vec3b>();

            int squareSize = 50;

            for (int y=0; y < height; y++)
            {
                for(int x=0; x < width; x++)
                {
                    // 체크보드 패턴 계산
                    bool isWhite = ((x / squareSize) + (y / squareSize)) % 2 == 0;

                    if (isWhite) indexer[y, x] = new Vec3b(255, 255, 255);
                    else indexer[y, x] = new Vec3b(0, 0, 0); 
                }
            }
            return board;
        }

        // 십자가 패턴
        private static Mat CreatedCrossPattern(int height, int width)
        {
            Mat cross = new Mat(height, width, MatType.CV_8UC3);
            var indexer = cross.GetGenericIndexer<Vec3b>();

            int thick = 50;     // 십자가 두께

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 십자가 패턴 계산
                    if (Math.Abs(x - width / 2) < thick || Math.Abs(y - height / 2) < thick)
                        indexer[y, x] = new Vec3b(0, 0, 0);
                    else
                        indexer[y, x] = new Vec3b(255, 255, 255);

                }
            }
            return cross;
        }

        // 4가지 색상의 사분면 이미지 생성
        // 왼쪽 위는 빨강, 오른쪽 위는 파랑
        // 왼쪽 아래는 초록, 오른쪽 아래는 하양
        private static Mat CreatedQuadrant(int height, int width)
        {
            Mat quadrant = new Mat(height, width, MatType.CV_8UC3);
            var indexer = quadrant.GetGenericIndexer<Vec3b>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // 사분할
                        if (x < width / 2 && y < height / 2) 
                            indexer[y, x] = new Vec3b(0, 0, 255);
                        else if (x < width / 2 && y > height / 2) 
                            indexer[y, x] = new Vec3b(0, 255, 0);
                        else if (x > width / 2 && y < height / 2)
                            indexer[y, x] = new Vec3b(255, 0, 0);
                        else indexer[y, x] = new Vec3b(255, 255, 255);
                    }
                }
            return quadrant;
        }
    }
}
