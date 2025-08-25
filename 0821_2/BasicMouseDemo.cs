using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0821_2
{
    internal class BasicMouseDemo
    {
        private static List<Point> points = new List<Point>();
        private static Mat canvas;

        public static void MouseDemo()
        {
            // Cv2.SetMouseCallback("윈도우", OnMouseEvent);

            // 매개 변수 delay
            // 0 : 무한 대기 (키가 눌릴 때까지)
            // 양수 : 해당 밀리초만큼 대기
            // int key = Cv2.WaitKey(delay);

            // 키 코드 확인
            // key == 27 => esc
            // key == 13 => enter
            // key == 32 => spacebar
            // key == 8 => backspace

            canvas = new Mat(500, 800, MatType.CV_8UC3, Scalar.White);

            // Mouse Demo라는 이름의 윈도우 생성
            Cv2.NamedWindow("Mouse Demo");

            // 마우스 콜백 등록
            Cv2.SetMouseCallback("Mouse Demo", OnMouseEvent);

            while (true)
            {
                Cv2.ImShow("Mouse Demo", canvas);

                int key = Cv2.WaitKey(30);
                if (key == 27) break;
            }

            canvas.Dispose();
            Cv2.DestroyAllWindows();
        }

        /// <param name="eventType"> 어떤 마우스 이벤트인지 </param>
        /// <param name="x"> 마우스 x 좌표 </param>
        /// <param name="y"> 마우스 y 좌표 </param>
        /// <param name="flags"> 추가 정보 </param>
        private static void OnMouseEvent(MouseEventTypes eventType, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            Point currentPoint = new Point(x, y);

            switch (eventType)
            {
                case MouseEventTypes.LButtonDown:
                    points.Add(currentPoint);
                    DrawPoint(currentPoint, new Scalar(0, 0, 255));
                    break;

                case MouseEventTypes.RButtonDown:
                    points.Add(currentPoint);
                    DrawPoint(currentPoint, new Scalar(0, 255, 0));
                    break;

                case MouseEventTypes.LButtonDoubleClick:
                    Cv2.Circle(canvas, currentPoint, 30, new Scalar(255, 0, 0), 3);
                    break;

                case MouseEventTypes.RButtonDoubleClick:
                    Cv2.Circle(canvas, currentPoint, 30, new Scalar(0, 255, 0), 3);
                    break;

                case MouseEventTypes.MouseMove:
                    ShowCurrentCoordinates(currentPoint);
                    break;

                case MouseEventTypes.MouseWheel:
                    points.Add(currentPoint);
                    ClearCanvase();
                    break;
            }
        }

        private static void DrawPoint(Point point, Scalar color)
        {
            // 작은 원 찍기
            Cv2.Circle(canvas, point, 5, color, -1);

            // 좌표 텍스트 표시
            string str1 = $"{point.X}, {point.Y}";
            Cv2.PutText(canvas, str1, point, HersheyFonts.HersheySimplex, 0.5, Scalar.Black, 1);
        }

        private static void ShowCurrentCoordinates(Point point)
        {
            // 임시로 좌표 표시 (다음 프레임에서 지워짐)
            Mat temp = canvas.Clone();

            string CoordText = $"{point.X} : {point.Y}";
            Cv2.PutText(temp, CoordText, new Point(point.X + 15, point.Y + 15),
                HersheyFonts.HersheySimplex, 0.5, Scalar.Black, 1);

            Cv2.ImShow("Mouse Demo", temp);
            temp.Dispose();
        }

        // 캔버스 초기화
        private static void ClearCanvase()
        {
            canvas.SetTo(Scalar.White);
        }
    }
}
