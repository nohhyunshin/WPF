using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0821_2
{
    // 1. 캔버스 생성 (400x600)
    // 2. 마우스 콜백 등록
    // 3. 왼쪽 클릭 시 카운터 증가
    // 4. 오른쪽 클릭 시 카운터 리셋
    // 5. 현재 카운트를 화면에 표시
    // 6. 클릭한 위치에 숫자 표시
    
    internal class ClickCounter
    {
        private static Mat canvas;
        private static int clickCount = 0;

        public static void ClickCounterDemo()
        {
            canvas = new Mat(600, 800, MatType.CV_8UC3);

            Cv2.NamedWindow("ClickCounter");
            Cv2.SetMouseCallback("ClickCounter", OnClickCounterEvent);

            while (true)
            {
                Cv2.ImShow("ClickCounter", canvas);

                int key = Cv2.WaitKey(30);
                if (key == 27) break;
            }
            canvas.Dispose();
            Cv2.DestroyAllWindows();
        }

        private static void OnClickCounterEvent(MouseEventTypes eventType, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            switch (eventType)
            {
                case MouseEventTypes.LButtonDown:
                    clickCount++;
                    DrawClickInfo(new Point(x, y));
                    break;
                case MouseEventTypes.RButtonDown:
                    clickCount = 0;
                    canvas.SetTo(Scalar.White);
                    break;
                default:
                    break;
            }
        }

        private static void DrawClickInfo(Point point)
        {
            Cv2.Circle(canvas, point, 5, Scalar.Black, -1);

            string str1 = $"Count {clickCount}";
            Cv2.PutText(canvas, str1, point, HersheyFonts.HersheySimplex, 0.3, Scalar.Black, 1);
        }
    }
}
