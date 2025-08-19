using OpenCvSharp;
using Range = OpenCvSharp.Range;

namespace _0819_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 2D 좌표
            Point point = new Point(100, 200);
            Point point2 = new Point(x:150, y:210);

            // 실수 좌표
            Point2f ptf1 = new Point2f(100.5f, 200.0f);

            // 3D 좌표
            Point3d pt3d = new Point3d(10.0f, 20.0f, 30.0f);


            // Size
            // 이미지나 사각형의 크기
            Size imageSize = new Size(1920, 1080);


            // Scalar
            // 픽셀값 또는 색상을 표현하는 4채널 벡터
            Scalar blue = new Scalar(255, 0, 0);
            Scalar green = new Scalar(0, 255, 0);
            Scalar red = new Scalar(0, 0, 255);
                
            Scalar gray = new Scalar(125);      // 회색
            // Scalar gray2 = new Scalar(125, 125, 125);       // 회색

            Scalar transparentRed = new Scalar(0, 0, 200, 128);


            // Range
            // 히스토그램 계산이나 배열 슬라이싱에 사용
            Range fullRange = new Range(40, 200);


            // 사각형 영역 정의
            // Rect(x, y, width, height)
            Rect rect1 = new Rect(10, 20, 100, 80);


            // Mat 클래스
            // Matsms OpenCv 핵심 데이터 구조, 다차원 배열을 효율적으로 처리
            Mat image = new Mat(480, 640, MatType.CV_8UC1);

            Console.WriteLine($"Rows : {image.Rows}");                  // 높이
            Console.WriteLine($"Cols : {image.Cols}");                  // 너비
            Console.WriteLine($"Channels : {image.Channels()}");        // 채널
            Console.WriteLine($"Depth : {image.Depth()}");              // 깊이
            Console.WriteLine($"Type : {image.Type()}");                // 전체 타입
            Console.WriteLine($"ElemSize : {image.ElemSize()}");        // 원소 크기 (바이트)
            Console.WriteLine($"Total : {image.Total()}");              // 전체 원소 개수
            Console.WriteLine($"Size : {image.Size()}");                // 차원별 크기
        }
    }
}
