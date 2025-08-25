using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

// 매개 변수 delay
// 0 : 무한 대기 (키가 눌릴 때까지)
// 양수 : 해당 밀리초만큼 대기
// int key = Cv2.WaitKey(delay);

// 키 코드 확인
// key == 27 => esc
// key == 13 => enter
// key == 32 => spacebar
// key == 8 => backspace

namespace _0821_2
{
    internal class KeyBoardEvents
    {
        public static void KeyBoardDemo()
        {
            Size canvasSize = new Size(600, 400);

            using (Mat canvas = new Mat(canvasSize, MatType.CV_8UC3, Scalar.White))
            {
                while (true)
                {
                    Cv2.ImShow("KeyBoard Demo", canvas);

                    int key = Cv2.WaitKey(0);
                    if (key == 27) break;

                    // 키보드 이벤트
                    bool handled = HandledKeyEvent(key, canvas);
                    if (handled)
                    {
                        Console.WriteLine($"알 수 없는 키 : {key}");
                    }
                }
                Cv2.DestroyAllWindows();
            }
        }

        private static bool HandledKeyEvent(int key, Mat canvas)
        {
            switch(key)
            {
                case (int)'r': case (int)'R':
                    canvas.SetTo(Scalar.Red);
                    return true;

                case (int)'g': case (int)'G':
                    canvas.SetTo(Scalar.Green);
                    return true;

                case (int)'b': case (int)'B':
                    canvas.SetTo(Scalar.Blue);
                    return true;

                case (int)'1': 
                    canvas.SetTo(Scalar.Black);
                    return true;

                case (int)'2': 
                    canvas.SetTo(Scalar.Tomato);
                    return true;

                case (int)'3': 
                    canvas.SetTo(Scalar.SkyBlue);
                    return true;

                case (int)'4': 
                    canvas.SetTo(Scalar.DarkOliveGreen);
                    return true;

                case (int)'5': 
                    canvas.SetTo(Scalar.LightPink);
                    return true;

                case (int)'c': 
                    canvas.SetTo(Scalar.White);
                    return true;

                case (int)'z':
                    SetRandomColor(canvas);
                    return true;

                case (int)'x':
                    SaveScreenShot(canvas);
                    return true;

                default:
                    return false;
            }
        }

        private static void SetRandomColor(Mat canvas)
        {
            Random rand = new Random();

            byte b = (byte)rand.Next(0, 256);
            byte g = (byte)rand.Next(0, 256);
            byte r = (byte)rand.Next(0, 256);

            Scalar randColor = new Scalar(b, g, r);
            canvas.SetTo(randColor);
        }

        private static void SaveScreenShot(Mat canvas)
        {
            string filename = $"screenshot_{DateTime.Now:yyyyMMdd_hhmmss}.png";

            try
            {
                bool success = Cv2.ImWrite(filename, canvas);
                if (success)
                {
                    Mat temp = canvas.Clone();
                    Cv2.PutText(temp, $"Saved : {filename}", new Point(10, canvas.Height - 30), HersheyFonts.HersheySimplex, 0.6, Scalar.White, 2);

                    Cv2.ImShow("KeyBoard Demo", temp);
                    Cv2.WaitKey(1000);      // 1초동안 메세지 표시
                    temp.Dispose();
                }
                else
                {
                    Console.WriteLine("실패");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"저장 오류 : {e.ToString()}");
            }
        }
    }
}
