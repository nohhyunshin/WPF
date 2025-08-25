using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0822
{
    internal class CameraControlDemo
    {
        public static void CameraWithControls()
        {
            using (VideoCapture cap = new VideoCapture(0))
            {
                if (!cap.IsOpened())
                {
                    return;
                }

                cap.Set(VideoCaptureProperties.FrameWidth, 800);
                cap.Set(VideoCaptureProperties.FrameHeight, 600);

                // 제어 변수
                // 제어 변수를 bool로 잡으면 On/Off 개념에 가까움!
                bool showInfo = true;
                bool showCrosshair = false;
                int frameCount = 0;
                DateTime startTime = DateTime.Now;

                using(Mat frame = new Mat())
                {
                    while(true)
                    {
                        cap.Read(frame);
                        if (frame.Empty()) break;

                        frameCount++;

                        // 정보 표시
                        if (showInfo)
                        {
                            // 정보 표시 함수 사용
                            AddFrameInfo(frame, frameCount, startTime);
                        }

                        // 십자선 표시
                        if(showCrosshair)
                        {
                            // 초점 그리드 표시 함수 사용
                            AddCrossHair(frame);
                        }

                        // 조작 안내
                        AddControlGuide(frame);

                        Cv2.ImShow("Camera Control", frame);

                        int key = Cv2.WaitKey(30);
                        if (key == 27) break;

                        // 조작
                        HandledControlKeys(key, frame, ref showInfo, ref showCrosshair);
                    }
                }
                Cv2.DestroyAllWindows();
            }
        }

        // 정보 표시 함수
        private static void AddFrameInfo(Mat frame, int frameCount, DateTime startTime)
        {
            // 배경 박스
            Cv2.Rectangle(frame, new Rect(10, 10, 300, 120), Scalar.Black, -1);

            // 시간 정보
            TimeSpan elapsed = DateTime.Now - startTime;
            string timeText = $"Time : {elapsed:mm\\ss}";
            Cv2.PutText(frame, timeText, new Point(20,40), HersheyFonts.HersheySimplex, 0.7, Scalar.White, 2);

            // 프레임 정보
            string framText = $"Frame : {frameCount}";
            Cv2.PutText(frame, framText, new Point(20,70), HersheyFonts.HersheySimplex, 0.7, Scalar.White, 2);

            // FPS 정보
            double fps = frameCount / elapsed.TotalSeconds;
            string fpsText = $"FPS : {fps:F1}";
            Cv2.PutText(frame, fpsText, new Point(20,100), HersheyFonts.HersheySimplex, 0.7, Scalar.White, 2);
        }

        // 중앙 그리드 표시 함수 (십자선)
        private static void AddCrossHair(Mat frame)
        {
            Point center = new Point(frame.Width / 2, frame.Height / 2);    // 중앙
            int lineLength = 30;

            // 가로선
            Cv2.Line(frame, new Point(center.X - lineLength, center.Y), new Point(center.X + lineLength, center.Y), Scalar.Red, 2);

            // 세로선 (-가 위)
            Cv2.Line(frame, new Point(center.X, center.Y - lineLength), new Point(center.X, center.Y + lineLength), Scalar.Red, 2);

            // 십자선 중심점
            Cv2.Circle(frame, center, 3, Scalar.Red, -1);
        }

        // 조작 안내 함수
        private static void AddControlGuide(Mat frame)
        {
            int y = frame.Height - 60;
            Cv2.Rectangle(frame, new Rect(0, y, frame.Width, 60), Scalar.Black, -1);

            string guide = "S: ScreenShot | F: Info | C: Crosshair | ESC: Exit";
            Cv2.PutText(frame, guide, new Point(10, y+35), HersheyFonts.HersheySimplex, 0.7, Scalar.White, 2);
            
        }

        // 조작 함수
        private static void HandledControlKeys(int key, Mat frame, ref bool showInfo, ref bool showCrosshair)
        {
            switch(key)
            {
                case (int)'s': case (int)'S':
                    SaveScreesnShot(frame);
                    break;

                case (int)'f': case (int)'F':
                    showInfo = !showInfo; 
                    break;

                case (int)'c': case (int)'C':
                    showCrosshair = !showCrosshair;
                    break;

                default:
                    break;
            }
        }

        private static void SaveScreesnShot(Mat frame)
        {
            string filename = $"Camera Capture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

            try
            {
                bool success = Cv2.ImWrite(filename, frame);
                if (success)
                {
                    Console.WriteLine("저장 성공!");
                }
                else
                {
                    Console.WriteLine("저장 실패");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"오류 : {e.Message}");
            }
        }
    }
}
