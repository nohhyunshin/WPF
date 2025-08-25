using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0822
{
    /*
    VideoCapture camera = new VideoCapture(0);      // 기본 카메라
    VideoCapture camera1 = new VideoCapture(1);     // 두 번째 카메라
    
    // 해상도 설정
    camera.Set(VideoCaptureProperties.FrameWidth, 640);
    camera.Set(VideoCaptureProperties.FrameHeight, 480);
    
    // FPS 설정
    camera.Set(VideoCaptureProperties.Fps, 30);
    
    // 밝기, 대비 등 (카메라에 따라 지원 여부 다름)
    camera.Set(VideoCaptureProperties.Brightness, 128);
    camera.Set(VideoCaptureProperties.Contrast, 128);
    
    double height = camera.Get(VideoCaptureProperties.FrameHeight);
    double width = camera.Get(VideoCaptureProperties.FrameWidth);
    
    VideoCapture viedo = new VideoCapture("비디오 제목.확장자");                // 상대 경로 설정
    VideoCapture vied1 = new VideoCapture(@"C:\video\비디오 제목.확장자");      // 절대 경로 설정
    */
    internal class BasicCameraDemo
    {
        public static void BasicCameraUsage()
        {
            // 기본 카메라 사용법
            using (VideoCapture capture = new VideoCapture(0))      // 카메라 초기화
            {
                if (!capture.IsOpened())
                {
                    Console.WriteLine("카메라 작동 불가");
                    return;
                }

                capture.Set(VideoCaptureProperties.FrameWidth, 640);
                capture.Set(VideoCaptureProperties.FrameHeight, 480);

                double width = capture.Get(VideoCaptureProperties.FrameWidth);
                double height = capture.Get(VideoCaptureProperties.FrameHeight);
                double fps = capture.Get(VideoCaptureProperties.Fps);

                Console.WriteLine($"해상도 : {width} {height}");
                Console.WriteLine($"FPS : {fps}");      // 30이 기본값

                using (Mat frame = new Mat())
                {
                    while (true)
                    {
                        // 프레임 읽기
                        bool success = capture.Read(frame);
                        if (!success || frame.Empty())
                        {
                            return;
                        }

                        string timeText = DateTime.Now.ToString("HH:mm:ss");
                        Cv2.PutText(frame, timeText, new Point(10, 30), HersheyFonts.HersheyScriptSimplex, 1, Scalar.Black, 2);
                        Cv2.ImShow("카메라", frame);

                        int key = Cv2.WaitKey(30);
                        if (key == 27) break;
                    }
                }
                Cv2.DestroyAllWindows();
            }
        }
    }
}
