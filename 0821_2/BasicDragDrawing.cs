using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace _0821_2
{
    internal class BasicDragDrawing
    {
        private static Mat canvas;
        private static bool isDrawing = false;
        private static Point lastPoint;
        private static Scalar currentColor = Scalar.Black;
        private static int brushSize = 3;

        public static void DragDrawingDemo()
        {
            //Console.WriteLine("드래그 그리기 실습");
            //Console.WriteLine("마우스 드래그 그리기");
            //Console.WriteLine("키보드 조작");
            //Console.WriteLine("1 to 9 브러시 크기 조작");
            //Console.WriteLine("RGB");
            //Console.WriteLine("C 캔버스 지우기");

            canvas = new Mat(600, 800, MatType.CV_8UC3, Scalar.White);

            Cv2.NamedWindow("Drag Drawing");
            Cv2.SetMouseCallback("Drag Drawing", OnDrawingMouseEvent);

            while (true)
            {
                Cv2.ImShow("Drag Drawing", canvas);

                int key = Cv2.WaitKey(30);
                if (key == 27) break;

                HandledDrawingKeyEvent(key);
            }
            canvas.Dispose();
            Cv2.DestroyAllWindows();
        }

        private static void OnDrawingMouseEvent(MouseEventTypes eventTypes, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            Point currentPoint = new Point(x, y);

            switch(eventTypes)
            {
                case MouseEventTypes.LButtonDown:
                    isDrawing = true;
                    lastPoint = currentPoint;

                    Cv2.Circle(canvas, currentPoint, brushSize, currentColor, -1);
                    break;

                case MouseEventTypes.MouseMove:
                    if (isDrawing)
                    {
                        Cv2.Line(canvas, lastPoint, currentPoint, currentColor, brushSize);
                        lastPoint = currentPoint;
                    }
                    break;

                case MouseEventTypes.LButtonUp:
                    isDrawing = false;
                    break;
            }
        }

        private static void HandledDrawingKeyEvent(int key)
        {
            bool needUpdate = false;

            if (key >= (int)'1' && key <= (int)'9')
            {
                brushSize = key - (int)'0';
                needUpdate = true;
            }

            switch (key)
            {
                case (int)'r': case (int)'R':
                    currentColor = Scalar.Red;
                    needUpdate = true;
                    break;

                case (int)'g': case (int)'G':
                    currentColor = Scalar.Green;
                    needUpdate = true;
                    break;

                case (int)'b': case (int)'B':
                    currentColor = Scalar.Blue;
                    needUpdate = true;
                    break;

                // Clear
                case (int)'c': case (int)'C':
                    canvas.SetTo(Scalar.White);
                    needUpdate = true;
                    break;
            }

            //if (needUpdate)
            //{
            //    UpdateStatusDisplay();
            //}
        }

        //private static void UpdateStatusDisplay()
        //{
            
        //}
    }
}
