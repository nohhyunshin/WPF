using System;
using OpenCvSharp;

namespace _0820_2
{
    internal class Program
    {
        // Mat 클래스
        // Mat : Matrix(행렬)의 약어
        // OpenCv에서 모든 이미지는 Mat 객체
        // 숫자들의 격자판이라고 생각하면 됨!

        // Mat 내부 구조
        // Mat 객체 = 헤더 + 데이터
        // 헤더 (가벼움)
        static void Main(string[] args)
        {
            // 기본 생성 문법
            // Mat 이미지 이름 = new Mat(높이, 너비, 타입, 색상)

            // MatType
            // CV_<비트 수><데이터 형><채널 수>
            // 8 : 8비트 (0부터 25까지의 수를 사용하겠다)
            // U : Unsigned (양수만)
            // C : Channel (채널)
            // 1 : 하나의 색상만 사용
            Mat img = new Mat(480, 600, MatType.CV_8UC1, new Scalar(128));

            // 0 행렬 : 모든 값이 0
            Mat zeros = Mat.Zeros(300, 300, MatType.CV_8UC1);
            Cv2.ImShow("Zero", zeros);

            // 1 행렬 : 모든 값이 1
            Mat ones = Mat.Ones(300, 300, MatType.CV_8UC1);
            Cv2.ImShow("One", ones);

            // 단위 행렬 : 대각선이 1
            Mat eye = Mat.Eye(300, 300, MatType.CV_8UC1) * 255;
            Cv2.ImShow("Eye", eye);

            // 난수 행렬 (0부터 255 사이의 난수)
            Mat random = new Mat(300, 300, MatType.CV_8UC3);
            Cv2.Randu(random, 0, 255);
            Cv2.ImShow("Random", random);

            // Scalar 클래스 (색상 표현)
            // Scalar(B, G, R)
            // 1 채널
            new Scalar(128);        // 회색
            new Scalar(0);          // 검은색
            new Scalar(255);        // 흰색

            // 3 채널 (세 가지 색상 사용)
            new Scalar(255, 0, 0);      // 파랑색
            new Scalar(0, 255, 0);      // 초록색
            new Scalar(0, 0, 255);      // 빨강색

            // Point
            Point pt1 = new Point(100, 50);     // 우로 100, 아래로 50 이동

            // 이미지 생성 (높이 300, 너비 400, 채널 수, 컬러)
            using (Mat myImg = new Mat(300, 400, MatType.CV_8UC3, new Scalar(200, 100, 50)))
            {
                // 화면 표시
                Cv2.ImShow("Image Test", myImg);
                
                // 콘솔 창에 표시
                Console.WriteLine("이미지 정보");
                Console.WriteLine($"높이 :{myImg.Height}");
                Console.WriteLine($"너비 : {myImg.Width}");
                Console.WriteLine($"채널 : {myImg.Channels()}");

                // 키 입력 대기
                Cv2.WaitKey(0);

                // 창 닫기
                Cv2.DestroyAllWindows();
            }   // 이후 자동으로 메모리 해제 = 메모리 누수 방지

            // 실습
            using (Mat yellow = new Mat(400, 500, MatType.CV_8UC3, new Scalar(0, 255, 255)))
            {
                Mat copy1 = yellow;             // 헤더만 복사 (데이터 공유)
                Mat copy2 = yellow.Clone();     // 데이터까지 완전 복사 (내용 안 변함)

                yellow.SetTo(new Scalar(0, 0, 255));

                Cv2.ImShow("Image Copy1", copy1);
                Cv2.ImShow("Image Copy2", copy2);

                // 콘솔 창에 표시
                Console.WriteLine("이미지 정보");
                Console.WriteLine($"높이 :{yellow.Height}");
                Console.WriteLine($"너비 : {yellow.Width}");
                Console.WriteLine($"채널 : {yellow.Channels()}");

                // 텍스트 추가
                Cv2.PutText(yellow,
                            "Hello OpenCv!",                        // 글자
                            new Point(100, 50),                     // 위치
                            HersheyFonts.HersheySimplex,            // 폰트
                            1.5,                                    // 크기
                            new Scalar(0),                          // 색상
                            3);                                     // 두께

                Cv2.Circle(yellow, new Point(300, 400), 50, new Scalar(0), -1);
                Cv2.Rectangle(yellow, new Point(10, 50), new Point(100, 500), new Scalar(0), -1);

                // 화면 표시
                Cv2.ImShow("Image Test", yellow);

                // 키 입력 대기
                Cv2.WaitKey(0);

                // 창 닫기
                Cv2.DestroyAllWindows();
            }
        }
    }
}
