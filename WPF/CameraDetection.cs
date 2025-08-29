using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    class CameraDetection
    {
        // 에러 발생 시 MainWindow로 전달하기 위함 (에러 감지 시 호출)
        public static MainWindow MainWin { get; set; }

        // 물품 인식을 위한 배경 제거 > YOLO 도입 시 불필요
        // private static BackgroundSubtractorMOG2 elimination = BackgroundSubtractorMOG2.Create();

        // 전역 변수 선언
        private static int objR2L = 0;      // 오 > 왼
        private static int objL2R = 0;      // 왼 > 오
        private static int nextIdx = 0;
        private static int errorIndex = 1;  // WPF에서 에러로 인식한 객체의 일련번호
        private static int globalIdx = 1;

        private static Dictionary<int, Point> preventC = new Dictionary<int, Point>();
        private static Dictionary<int, bool> cntL2R = new Dictionary<int, bool>();
        private static Dictionary<int, bool> cntR2L = new Dictionary<int, bool>();

        // 출력값
        private static int displayValue = 0;
        private static bool dispL2R = true;     // 현재 출력되는 값이 L2R인지 R2L인지 판별

        // 에러 발생 여부 확인
        private static bool isError = true;

        // YOLO 세션
        private static InferenceSession yolo;

        // YOLO 작동 시 감지되는 오류 이름
        private static readonly string[] classNames = 
        { "Crazing", "Inclusion", "Patches", "Pitted-Surface", "Rolled-in-Scale", "Scratches" };

        // 
        private static readonly string[] errorNames =
        { "CRZ", "INC", "PAT", "PSF", "RIS", "SCR" };

        // 클래스 초기화할 때 YOLO 로드
        static CameraDetection()
        {
            string yoloPath = "best.onnx";      // deeplab 모델이니까 yolo 모델로 변경 필요
            yolo = new InferenceSession(yoloPath);
        }

        public static void CamerDetectionDemo()
        {
            using (VideoCapture cam = new VideoCapture(0))
            {
                if (!cam.IsOpened()) return;

                // 카메라 송출 화면 크기 지정
                cam.Set(VideoCaptureProperties.FrameWidth, 720);
                cam.Set(VideoCaptureProperties.FrameHeight, 405);

                // 제어 변수
                bool showInfo = true;
                int frameCount = 0;

                using (Mat video = new Mat())
                {
                    while (true)
                    {
                        cam.Read(video);
                        if (video.Empty()) break;

                        frameCount++;

                        // 물체 카운트 로직 호출
                        // YOLO 기반 객체 탐지
                        Countdown(video);

                        // 정보 표시
                        if (showInfo)
                        {
                            AddVideoInfo(video, frameCount, DateTime.Now);
                        }

                        Cv2.ImShow("Camera Detection", video);

                        int key = Cv2.WaitKey(30);
                        if (key == 27) break;
                    }
                }
                Cv2.DestroyAllWindows();
            }
        }

        // 카메라 화면 정보 출력
        private static void AddVideoInfo(Mat video, int frameCount, DateTime now)
        {
            // 시간 정보
            string txtTime = $"{now:yyyy-MM-dd HH:mm:ss}";
            Cv2.PutText(video, txtTime, new Point(440, 25), HersheyFonts.HersheyComplex, 0.5, Scalar.Tomato);
        }

        // YOLO 전처리
        private static DenseTensor <float> Preprocess (Mat image, int inputWidth, int inputHeight)
        {
            Mat resized = new Mat();
            Cv2.Resize(image, resized, new Size(inputWidth, inputHeight));
            Cv2.CvtColor(resized, resized, ColorConversionCodes.BGR2GRAY);

            float[] inputData = new float[3 * inputWidth * inputHeight];
            int idx = 0;

            for (int y = 0; y < inputHeight; y++)
            {
                for (int x = 0; x < inputWidth; x++)
                {
                    Vec3b color = resized.At<Vec3b>(y, x);
                    inputData[idx++] = color.Item2 / 255.0f; // R
                    inputData[idx++] = color.Item1 / 255.0f; // G
                    inputData[idx++] = color.Item0 / 255.0f; // B
                }
            }
            return new DenseTensor<float>(inputData, new[] { 1, 3, inputHeight, inputWidth });
        }

        // YOLO 추론
        // 오류 탐지가 안 되면 윤곽선도 나타나지 않고 카운트도 안 됨
        private static List <(Rect box, float score, int classId)> RunYolo (Mat image)
        {
            int inuptHeight = 640;      // YOLO 학습 시 이미지 크기
            int inputWidth = 640;       // 옵션 적용 여부 확인 후 변경 필요

            var tensor = Preprocess(image, inputWidth, inuptHeight);

            var inputs = new List<NamedOnnxValue>
            {
                // 입력 노드 이름 확인
                NamedOnnxValue.CreateFromTensor("image", tensor)
            };

            using (var results = yolo.Run(inputs))
            {
                var output = results.First().AsEnumerable<float>().ToArray();

                int numDetections = output.Length / 85;
                List<(Rect, float, int)> detections = new List<(Rect, float, int)>();

                for (int i = 0; i < numDetections; i++)
                {
                    float conf = output[i * 85 + 4];
                    if (conf < 0.5) continue;

                    var classScores = output.Skip(i * 85 + 5).Take(80).ToArray();
                    int classId = Array.IndexOf(classScores, classScores.Max());
                    float score = classScores.Max() * conf;

                    if (score < 0.5) continue;

                    float cx = output[i * 85 + 0] * image.Cols / inputWidth;
                    float cy = output[i * 85 + 1] * image.Rows / inuptHeight;
                    float w = output[i * 85 + 2] * image.Cols / inputWidth;
                    float h = output[i * 85 + 3] * image.Rows / inuptHeight; 

                    int x = (int)(cx - w / 2);
                    int y = (int)(cy - h / 2);
                    detections.Add((new Rect(x, y, (int)w, (int)h), score, classId));
                }
                return detections;
            }
        }

        // 오류 품번 측정
        private static void Countdown(Mat video)
        {
            var detections = RunYolo(video);

            // 화면 중앙을 기준으로 이동 방향 감지
            int centerLine = video.Cols / 2;

            foreach (var (box, score, classId) in detections)
            {
                string code = $"{classNames[classId]}";
                string accu = $"{score:F2}%";

                // 객체 윤곽선 표시
                Cv2.Rectangle(video, box, Scalar.Magenta, 2);

                Rect txtBg = new Rect(box.X, box.Y - 15, code.Length * 10, 15);
                Cv2.Rectangle(video, txtBg, Scalar.Magenta, -1);
                Cv2.PutText(video, code, new Point(box.X, box.Y - 5),
                            HersheyFonts.HersheyComplex, 0.4, Scalar.White, 1);

                // 중심점 좌표
                var center = new Point(box.X + box.Width / 2, box.Y + box.Height / 2);

                int objIdx = -1;
                double minDist = double.MaxValue;

                foreach (var keyV in preventC)
                {
                    double dist = Math.Sqrt(Math.Pow(center.X - keyV.Value.X, 2) +
                                            Math.Pow(center.Y - keyV.Value.Y, 2));
                    if (dist < 100 && dist < minDist)
                    {
                        objIdx = keyV.Key;
                        minDist = dist;
                    }
                }

                // 새 객체이면 추가
                if (objIdx == -1)
                {
                    objIdx = nextIdx++;
                    preventC[objIdx] = center;
                    cntL2R[objIdx] = false;
                    cntR2L[objIdx] = false;
                }

                // 라인 통과 감지
                if (preventC.ContainsKey(objIdx))
                {
                    Point prev = preventC[objIdx];
                    bool crossedLine = false;

                    // 왼→오
                    if (!cntL2R[objIdx] && prev.X < centerLine && center.X >= centerLine)
                    {
                        objL2R++;
                        cntL2R[objIdx] = true;
                        cntR2L[objIdx] = false;
                        if (!dispL2R) displayValue = 0;
                        displayValue = objL2R;
                        dispL2R = true;
                        crossedLine = true;
                    }

                    // 오→왼
                    if (!cntR2L[objIdx] && prev.X >= centerLine && center.X < centerLine)
                    {
                        objR2L++;
                        cntL2R[objIdx] = false;
                        cntR2L[objIdx] = true;
                        if (dispL2R) displayValue = 0;
                        displayValue = objR2L;
                        dispL2R = false;
                        crossedLine = true;
                    }

                    // MainWindow에 에러 전달
                    if (classId >= 0 && classId < classNames.Length)
                    {
                        string defection = $"{errorNames[classId]} {globalIdx:00}";
                        MainWin?.Dispatcher.Invoke(() =>
                        {
                            MainWin.AddError(defection, code, DateTime.Now, accu);
                        });
                        globalIdx++;
                    }

                    // 오류 감지 시 캡처
                    // errorIdx : 에러 종류 여섯 개를 인덱스 매핑
                    string filename = $"error_{classNames[classId]}_{DateTime.Now:yyMMdd-HHmmss_ff}.jpg";
                    try
                    {
                        if (isError)
                        {
                            // 영상 범위 벗어나지 않게 Rect 조정
                            int x = Math.Max(box.X, 0);
                            int y = Math.Max(box.Y, 0);
                            int width = Math.Min(box.Width, video.Cols - x);
                            int height = Math.Min(box.Height, video.Rows - y);
                            Rect roi = new Rect(x, y, width, height);

                            Mat cropped = new Mat(video, roi);

                            Cv2.ImWrite(filename, cropped);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"예외 발생 : {e.Message}");
                    }
                }
                // 객체 중심점 업데이트
                preventC[objIdx] = center;
            }
            // 개수 표시
            if (displayValue != 0)
            {
                int length = displayValue.ToString().Length;
                Cv2.Rectangle(video, new Rect(10, 10, length * 20, 35), Scalar.Black, -1);
                Cv2.PutText(video, $"{displayValue}", new Point(15, 35), HersheyFonts.HersheyComplex, 0.5, Scalar.White, 1);
            }

            /*
            Mat objCnt = new Mat();
            Cv2.CvtColor(video, objCnt, ColorConversionCodes.BGR2GRAY);
            // elimination.Apply(video, objCnt);

            // 전처리
            Cv2.Threshold(objCnt, objCnt, 80, 255, ThresholdTypes.Binary);     // 원본 데이터 필요 없음
            // 미세한 노이즈 제거
            Cv2.MorphologyEx(objCnt, objCnt, MorphTypes.Open,
                             Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5)));

            // 인식된 물품 윤곽선
            Cv2.FindContours(objCnt, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 화면 중앙을 기준으로 카운트
            int centerLine = video.Cols / 2;

            foreach (var contour in contours)
            {
                var rect = Cv2.BoundingRect(contour);
                string a = "에러";    // errorcode 연동 필요

                // 작은 사이즈의 잡음 제거
                if (rect.Width < 30 || rect.Height < 30) continue;

                // 중앙 좌표
                var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

                int objIdx = -1;
                double minDist = double.MaxValue;

                foreach (var keyV in preventC)
                {
                    double dist = Math.Sqrt(Math.Pow(center.X - keyV.Value.X, 2) + Math.Pow(center.Y - keyV.Value.Y, 2));

                    if (dist < 100 && dist < minDist)
                    {
                        objIdx = keyV.Key;
                        minDist = dist;
                    }
                }

                // 새 객체 탐지
                if (objIdx == -1)
                {
                    objIdx = nextIdx++;
                    preventC[objIdx] = center;
                    cntL2R[objIdx] = false;
                    cntR2L[objIdx] = false;
                }

                // 라인 통과 감지 (이전 중심점과 비교)
                if (preventC.ContainsKey(objIdx))
                {
                    Point prev = preventC[objIdx];
                    bool crossedLine = false;       // 중앙선 통과 감지

                    // 컨베이어 이동 방향 : 왼 > 오
                    if (!cntL2R[objIdx] && prev.X < centerLine && center.X >= centerLine)
                    {
                        objL2R++;

                        cntL2R[objIdx] = true;
                        cntR2L[objIdx] = false;

                        if (!dispL2R) displayValue = 0;
                        displayValue = objL2R;
                        dispL2R = true;
                        crossedLine = true;
                    }

                    // 컨베이어 이동 방향 : 오 > 왼
                    if (!cntR2L[objIdx] && prev.X >= centerLine && center.X < centerLine)
                    {
                        objR2L++;

                        cntL2R[objIdx] = false;
                        cntR2L[objIdx] = true;

                        if (!dispL2R) displayValue = 0;
                        displayValue = objR2L;
                        dispL2R = false;
                        crossedLine = true;
                    }

                    if (crossedLine)
                    {
                        int idx = errorIndex++;

                        // MainWindow에 에러 전달
                        MainWin?.Dispatcher.Invoke(() =>
                        {
                            MainWin.AddError(idx, a, DateTime.Now);
                        });
                    }
                }
                // List에 center 좌표 값 저장
                preventC[objIdx] = center;

                // 객체 윤곽선 표시
                Cv2.Rectangle(video, rect, Scalar.Magenta, 2);

                Rect txtBg = new Rect(rect.X, rect.Y - 15, a.Length * 15, 15);
                // 에러일 때
                Cv2.Rectangle(video, txtBg, Scalar.Magenta, -1);
                Cv2.PutText(video, a, new Point(rect.X, rect.Y - 5), HersheyFonts.HersheyComplex, 0.4, Scalar.White, 1);

                // 정상 객체
                // Cv2.Rectangle(video, txtBg, Scalar.White, -1);
            }

            // 개수 표시
            if (displayValue != 0)
            {
                int length = displayValue.ToString().Length;
                Cv2.Rectangle(video, new Rect(10, 10, length * 20, 35), Scalar.Black, -1);
                Cv2.PutText(video, $"{displayValue}", new Point(15, 35), HersheyFonts.HersheyComplex, 0.5, Scalar.White, 1);
            }
            */
        }
    }
}