using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _0822
{
    internal class BasicVideoPlayerDemo
    {
        public static void PlayVideoFile()
        {
            // 재생할 비디오 파일 경로
            string videoPath = "video2.mp4";

            // 파일이 존재하지 않으면 함수 종료
            if (!File.Exists(videoPath))
            {
                return;
            }

            // VideoCapture 객체 이용해 영상 불러오기
            using (VideoCapture cap = new VideoCapture(videoPath))
            {
                // 영상 파일 열기에 실패하면 종료
                if (!cap.IsOpened())
                {
                    return;
                }

                double fps = cap.Get(VideoCaptureProperties.Fps);
                double totalFrames = cap.Get(VideoCaptureProperties.FrameCount);
                double width = cap.Get(VideoCaptureProperties.FrameWidth);
                double height = cap.Get(VideoCaptureProperties.FrameHeight);
                double duration = totalFrames - fps;

                Console.WriteLine($"해상도 : {width} * {height}");
                Console.WriteLine($"FPS : {fps}");
                Console.WriteLine($"총 프레임 : {totalFrames}");
                Console.WriteLine($"재생 시간 : {duration}");
                Console.WriteLine($"Space 일시정지 / 재생");
                Console.WriteLine($"방향키로 +- 10");
                
                using(Mat frame = new Mat())
                {
                    int frameDelay = (int)(1000 / fps);     // 프레임 간 대기 시간
                    bool isPaused = false;                  // 일시 정지
                    int currentFrame = 0;                   // 현재 프레임 번호

                    while (true)
                    {
                        // 영상이 재생 중일 때만 프레임 읽기
                        if(!isPaused)
                        {
                            cap.Read(frame);

                            // 영상 끝에 도달하면 종료
                            if (frame.Empty()) break;

                            // PosFramse : 현재 위치해 있는 프레임을 받아 옴
                            // 현재 프레임 위치 갱신
                            currentFrame = (int)cap.Get(VideoCaptureProperties.PosFrames);
                        }

                        // 재생 정보 표시
                        AddVideoPlayerInfo(frame, currentFrame, totalFrames, fps, isPaused);

                        // 영상 출력
                        Cv2.ImShow("Video Player", frame);

                        int waitTime = isPaused ? 0 : frameDelay;
                        int key = Cv2.WaitKey(waitTime);
                        if (key == 27) break;

                        // 조작
                        bool shouldContinue = HandledVideoPlayerKeys(key, cap, ref isPaused, totalFrames, fps);
                        if (!shouldContinue) break;

                    }
                }
                Cv2.DestroyAllWindows();
            }
        }
        
        private static void AddVideoPlayerInfo(Mat frame, int currentFrame, double totalFrame, double fps, bool isPaused)
        {
            double progress = (currentFrame / totalFrame) * 100;        // 진행률
            double currentTime = currentFrame / fps;                    // 현재 시간 (초)
            double totalTime = totalFrame / fps;                        // 전체 시간 (초)

            // 정보 배경
            Cv2.Rectangle(frame, new Rect(0, 0, frame.Width, 80), Scalar.Black, -1);

            // 시간 정보
            string timeText = $"{TimeSpan.FromSeconds(currentTime):mm\\ss} / " +
                              $"{TimeSpan.FromSeconds(totalTime):mm\\ss}";
            Cv2.PutText(frame, timeText, new Point(10, 30), HersheyFonts.HersheySimplex, 0.5, Scalar.White, 1);

            // 진행률 정보 (진행률%과 프레임 위치)
            string progressText = $"Progress : {progress:F1}% ({currentFrame} / {totalFrame})";
            Cv2.PutText(frame, progressText, new Point(10, 60), HersheyFonts.HersheySimplex, 0.5, Scalar.White, 1);

            if (isPaused)
            {
                Cv2.PutText(frame, "PAUSED", new Point(frame.Width - 150, 40), HersheyFonts.HersheySimplex, 0.5, Scalar.White, 1);
            }

            // 진행률 바
            int barWidth = frame.Width - 40;        // 바 전체 길이
            int barHeight = 10;                     // 바 높이

            Point barStart = new Point(20, frame.Height - 30);                              // 진행률 바 시작 위치
            Point barEnd = new Point(barStart.X + barWidth, barStart.Y + barHeight);        // 진행률 바 끝 위치

            // 진행률 바 배경
            Cv2.Rectangle(frame, barStart, barEnd, Scalar.Gray, -1);

            int progressWidth = (int)(barWidth * progress / 100);
            Point progressEnd = new Point(barStart.X + progressWidth, barEnd.Y);

            // 진행 상태 (녹색으로 표시)
            Cv2.Rectangle(frame, barStart, progressEnd, Scalar.Green, -1);
        }

        private static bool HandledVideoPlayerKeys(int key, VideoCapture capture, ref bool isPaused, double totalFrames, double fps)
        {
            switch(key)
            {
                // 스페이스 바
                case 32:
                    isPaused = !isPaused;
                    return true;

                // 왼쪽 방향키
                case 'a':
                case 'A':
                    {
                        double currentFrame = capture.Get(VideoCaptureProperties.PosFrames);
                        double newFrame = Math.Max(0, currentFrame - (fps * 10));   // 10초 뒤로
                        capture.Set(VideoCaptureProperties.PosFrames, newFrame);
                        return true;
                    }

                // 오른쪽 방향키
                case 'd':
                case 'D':
                    {
                        double currentFrame = capture.Get(VideoCaptureProperties.PosFrames);
                        double newFrame = Math.Min(totalFrames - 1, currentFrame + (fps * 10));   // 10초 앞으로
                        capture.Set(VideoCaptureProperties.PosFrames, newFrame);
                        return true;
                    }

                default:
                    return true;
            }
        }
    }
}
