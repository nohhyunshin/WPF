using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0822
{
    public class CameraEffectsPracticeDemo
    {
        public enum EffectType
        {
            None,           // 원본
            Grayscale,      // 흑백
            Mirror,         // 좌우 반전
            UpsideDown,     // 상하 반전
            Zoom,           // 확대 (중앙 부분만)
            Corner          // 4분할 화면
        }

        public static void CameraEffectsPractice()
        {
            Console.WriteLine("=== 카메라 효과 적용기 실습 ===");
            Console.WriteLine("키보드 조작:");
            Console.WriteLine("1: 원본");
            Console.WriteLine("2: 흑백");
            Console.WriteLine("3: 좌우 반전");
            Console.WriteLine("4: 상하 반전");
            Console.WriteLine("5: 줌 효과");
            Console.WriteLine("6: 4분할 화면");
            Console.WriteLine("ESC: 종료\n");

            using (VideoCapture capture = new VideoCapture(0))
            {
                if (!capture.IsOpened())
                {
                    Console.WriteLine("카메라를 열 수 없습니다.");
                    return;
                }

                EffectType currentEffect = EffectType.None;

                using (Mat frame = new Mat())
                using (Mat processed = new Mat())
                {
                    while (true)
                    {
                        capture.Read(frame);
                        if (frame.Empty()) break;

                        // TODO: 학생들이 각 효과를 구현
                        ApplyEffect(frame, processed, currentEffect);

                        // 현재 효과 정보 표시
                        string effectName = currentEffect.ToString();
                        Cv2.PutText(processed, $"Effect: {effectName}", new Point(10, 30),
                                   HersheyFonts.HersheySimplex, 1, Scalar.Yellow, 2);

                        Cv2.ImShow("Camera", frame);
                        Cv2.ImShow("Camera Effects", processed);

                        int key = Cv2.WaitKey(30);
                        if (key == 27) break;

                        // 효과 변경
                        if (key >= (int)'1' && key <= (int)'6')
                        {
                            currentEffect = (EffectType)(key - (int)'1');
                            Console.WriteLine($"효과 변경: {currentEffect}");
                        }
                    }
                }

                Cv2.DestroyAllWindows();
            }
        }

        private static void ApplyEffect(Mat input, Mat output, EffectType effect)
        {
            switch (effect)
            {
                case EffectType.None:
                    // TODO: 원본 그대로 복사
                    input.CopyTo(output);
                    break;

                case EffectType.Grayscale:
                    // TODO: 흑백으로 변환 후 다시 3채널로
                    // Cv2.CvtColor() 사용
                    // Cv2.CvtColor(input, output, ColorConversionCodes.BGR2GRAY);
                    // Cv2.CvtColor(output, output, ColorConversionCodes.GRAY2BGR);

                    using(Mat gray = new Mat())
                    {
                        // BGR2GRAY : grayscale 로 전환
                        Cv2.CvtColor(input, gray, ColorConversionCodes.BGR2GRAY);
                        // GRAY2BGR : grayscale을 다시 3채널로 변경
                        Cv2.CvtColor(gray, output, ColorConversionCodes.GRAY2BGR);
                    }
                    break;

                case EffectType.Mirror:
                    // TODO: 좌우 반전
                    // Cv2.Flip() 사용, flipCode = 1
                    Cv2.Flip(input, output, FlipMode.Y);
                    // Cv2.Flip(input, output, flipCode: (FlipMode)1);
                    break;

                case EffectType.UpsideDown:
                    // TODO: 상하 반전
                    // Cv2.Flip() 사용, flipCode = 0
                    Cv2.Flip(input, output, FlipMode.X);
                    // Cv2.Flip(input, output, flipCode: (FlipMode)0);
                    break;

                case EffectType.Zoom:
                    // TODO: 중앙 부분만 확대
                    // ROI 사용해서 중앙 50% 영역을 전체 화면으로 확대
                    int width = input.Width;
                    int height = input.Height;

                    int x = width / 4;
                    int y = height / 4;

                    // (x,y) 좌표에서 width/2 만큼, height/2 만큼의 길이로 사각형
                    Rect roi = new Rect(x, y, width / 2, height / 2);
                    using(Mat cropped = new Mat(input, roi))
                    {
                        Cv2.Resize(cropped, output, new Size(width, height), 0, 0, InterpolationFlags.Linear);
                    }
                    break;

                case EffectType.Corner:
                    // TODO: 4분할 화면 효과
                    // 같은 영상을 4개 구역에 작게 표시
                    using (Mat quadrant = new Mat(input.Width, input.Height, MatType.CV_8UC3))
                    {

                    }
                        break;
                default:
                    input.CopyTo(input);
                    break;

            }
        }
    }
}
