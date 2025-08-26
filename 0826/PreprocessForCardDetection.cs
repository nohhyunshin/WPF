using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using Tesseract;

namespace _0826
{
    internal class PreprocessForCardDetection
    {
        public class CardCandidate
        {
            public Point2f[] Corners { get; set; }
            public double Area { get; set; }
            public double AsepectRatio {  get; set; }
            public double Confidence {  get; set; }     // 신뢰도
            public OpenCvSharp.Rect BoundingRect {  get; set; }
            public Point[] Contour {  get; set; }
            public override string ToString()
            {
                return $"Card (신뢰도 : {Confidence:F2}, 면적 : {Area:F0}, 비율 : {AsepectRatio:F2})";
            }
        }

        // 명함 검출용 전처리
        public static Mat PreprocessForCardDetectionDemo(Mat src)
        {

            Mat gray = new Mat();
            Mat blurred = new Mat();
            Mat edges = new Mat();
            Mat dilated = new Mat();
            Mat eroded = new Mat();

            // 1단계 : 그레이 스케일로 변경
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2단계 : 노이즈 제거 = 가우시안 블러 사용
            Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

            // 3단계 : 엣지 검출 = canny 사용해 명함 경계선 찾기
            double threshold1 = 10;
            double threshold2 = 50;

            // 이미지 밝기에 따른 임계값 자동 조정
            //Scalar meanBrightness = Cv2.Mean(blurred);

            //if (meanBrightness.Val0 >= 150)
            //{
            //    // 밝은 이미지
            //    threshold1 = 75;
            //    threshold2 = 200;
            //} else if (meanBrightness.Val0 < 100)
            //{
            //    // 어두운 이미지
            //    threshold1 = 30;
            //    threshold2 = 100;
            //}

            Cv2.Canny(blurred, edges, threshold1, threshold2);

            // 4단계 : 형태학적 연산 = 끊어진 선 연결
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Dilate(edges, dilated, kernel, iterations: 2);      // iterations : 반복 횟수
            Cv2.Erode(dilated, eroded, kernel, iterations: 1);

            Cv2.ImShow("1 - 1. GrayScale", gray);
            Cv2.ImShow("1 - 2. Blurred", blurred);
            Cv2.ImShow("1 - 3. Edges", edges);
            Cv2.ImShow("1 - 4. Morphology", eroded);

            return eroded;
        }

        // 명함 후보 검출
        public static List<CardCandidate> FindCardCandidates(Mat edges, Mat original)
        {
            List <CardCandidate> candidates = new List<CardCandidate>();

            // 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 이미지 크기 정보
            int imageArea = original.Width * original.Height;
            double minArea = imageArea * 0.01;
            double maxArea = imageArea * 0.8;

            for (int i = 0; i < contours.Length; i++)
            {
                var candidate = AnalyzeContourForCard(contours[i], original.Size());

                if (candidate != null && candidate.Area >= minArea && candidate.Area <= maxArea)
                {
                    candidates.Add(candidate);
                }
            }
            return candidates;
        }

        // 최적의 명함 후보 선택
        public static CardCandidate SelectBestCandidate(List<CardCandidate> candidates)
        {
            if (!candidates.Any()) return null;

            // 신뢰도 기준으로 정렬하여 최고 후보 반환
            return candidates.OrderByDescending(c => c.Confidence).First();
        }

        // 명함 영역 추출
        public static Mat ExtracntCard(Mat original, CardCandidate bestCandidate)
        {
            // 표준 명함 크기 (픽셀) : 실제 명함 비율 반영
            int cardWidth = 350;
            int cardHeight = 220;

            // 목표 사각형 좌표 (정면에서 본 명함)
            Point2f[] dstPoint =
            {
                new Point2f(0, 0),                      // 왼쪽 위
                new Point2f(cardWidth, 0),              // 오른쪽 위
                new Point2f(cardWidth, cardHeight),     // 오른쪽 아래
                new Point2f(0, cardHeight)              // 왼쪽 아래
            };

            // 원근 변환 행렬 계산
            Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(bestCandidate.Corners, dstPoint);

            // 원근 변환 적용
            Mat extractedCard = new Mat();
            Cv2.WarpPerspective(original, extractedCard, perspectiveMatrix, new Size(cardWidth, cardHeight));

            return extractedCard;
        }

        public static void DrawDetectionResult(Mat origianl , List<CardCandidate> candidates, CardCandidate bestCandidate)
        {
            Mat result = origianl.Clone();

            // 모든 후보 표시 (낮은 신뢰도는 흐리게)
            for (int i = 0; i < Math.Min(candidates.Count, 5); i++)
            {
                var candidate = candidates[i];

                Scalar color = candidate == bestCandidate ? Scalar.Lime : Scalar.Yellow;

                int thickness = candidate == bestCandidate ? 3 : 1;

                // 윤곽선 그리기
                Point[] corners = candidate.Corners.Select(p=> new Point((int)p.X, (int)p.Y)).ToArray();

                for (int j = 0; j < 4; j++)
                {
                    Cv2.Line(result, corners[j], corners[(j + 1) % 4], color, thickness);
                }

                // 꼭짓점 표시
                foreach(var corner in corners)
                {
                    Cv2.Circle(result, corner, 5, Scalar.Red, -1);
                }

                Cv2.ImShow("",result);
            }
        }

        private static CardCandidate AnalyzeContourForCard(Point[] contour, Size imageSize)
        {
            // 면적 크기 검사
            double area = Cv2.ContourArea(contour);
            if (area < 1000) return null;

            // 윤곽선을 사각형으로 근사화
            double epsilon = 0.02 * Cv2.ArcLength(contour, true);
            Point[] approx = Cv2.ApproxPolyDP(contour, epsilon, true);

            // 4개의 꼭지점이 있어야 사각형
            if (approx.Length != 4) return null;

            // 볼록 도형인지 확인 (명함은 볼록한 사각형)
            if (!Cv2.IsContourConvex(approx)) return null;

            var candidate = new CardCandidate
            {
                Contour = contour,
                Area = area,
                BoundingRect = Cv2.BoundingRect(contour)
            };

            // 꼭짓점을 시계방향으로 정렬
            candidate.Corners = OrderCorners(approx.Select(p => new Point2f(p.X, p.Y)).ToArray());

            // 종횡비 계산
            double width = Distance(candidate.Corners[0], candidate.Corners[1]);
            double height = Distance(candidate.Corners[1], candidate.Corners[2]);
            candidate.AsepectRatio = Math.Max(width, height) / Math.Min(width, height);

            // 신뢰도 계산
            candidate.Confidence = CalculateConfidence(candidate, imageSize);

            return candidate;
        }

        private static double CalculateConfidence(CardCandidate candidate, Size imageSize)
        {
            double confidence = 0.0;

            // 1. 종횡비 점수 (명함 표준 비율 1.59:1에 가까울수록 높은 점수)
            double idealRatio = 1.59;
            double ratioDiff = Math.Abs(candidate.AsepectRatio - idealRatio);
            double ratioScore = Math.Max(0, 1.0 - (ratioDiff / idealRatio));
            confidence += ratioScore * 0.4; // 40% 가중치

            // 2. 면적 점수 (이미지의 5~40% 크기일 때 높은 점수)
            double imageArea = imageSize.Width * imageSize.Height;
            double areaRatio = candidate.Area / imageArea;
            double areaScore = 0.0;

            if (areaRatio >= 0.05 && areaRatio <= 0.4)
            {
                // 최적 범위 내에서는 높은 점수
                areaScore = Math.Min(1.0, (areaRatio - 0.05) / 0.15 + 0.5);
            }
            else if (areaRatio > 0.4)
            {
                // 너무 클 때는 점수 감소
                areaScore = Math.Max(0, 1.0 - (areaRatio - 0.4) / 0.3);
            }
            else
            {
                // 너무 작을 때는 낮은 점수
                areaScore = areaRatio / 0.05;
            }
            confidence += areaScore * 0.3; // 30% 가중치

            // 3. 중앙 위치 점수 (이미지 중앙에 가까울수록 높은 점수)
            Point2f imageCenter = new Point2f(imageSize.Width / 2.0f, imageSize.Height / 2.0f);
            Point2f candidateCenter = new Point2f(
                candidate.Corners.Average(p => p.X),
                candidate.Corners.Average(p => p.Y)
            );

            double maxDistance = Math.Sqrt(Math.Pow(imageSize.Width / 2, 2) + Math.Pow(imageSize.Height / 2, 2));
            double centerDistance = Distance(imageCenter, candidateCenter);
            double centerScore = Math.Max(0, 1.0 - (centerDistance / maxDistance));
            confidence += centerScore * 0.2; // 20% 가중치

            // 4. 각도 점수 (직각에 가까운 모서리일수록 높은 점수)
            double angleScore = 0.0;
            for (int i = 0; i < 4; i++)
            {
                Point2f p1 = candidate.Corners[i];
                Point2f p2 = candidate.Corners[(i + 1) % 4];
                Point2f p3 = candidate.Corners[(i + 2) % 4];

                double angle = CalculateAngle(p1, p2, p3);
                double angleDiff = Math.Abs(angle - 90); // 90도와의 차이
                angleScore += Math.Max(0, 1.0 - (angleDiff / 45)); // 45도 차이까지 허용
            }
            angleScore /= 4.0; // 평균 계산
            confidence += angleScore * 0.1; // 10% 가중치

            return Math.Min(1.0, confidence);
        }

        private static Point2f[] OrderCorners(Point2f[] corners)
        {
            if (corners.Length != 4)
            {
                throw new ArgumentException("정확히 4개의 점이 필요합니다.");
            }

            // 중심점 계산
            Point2f center = new Point2f(
                corners.Average(p => p.X),
                corners.Average(p => p.Y));

            // 중심점을 기준으로 각도 계산하여 시계방향 정렬
            var ordered = corners.OrderBy(p => Math.Atan2(p.Y - center.Y, p.X - center.X)).ToArray();

            // 첫 번째 점을 왼쪽 위로 조정
            int topLeftIndex = 0;
            double minDistance = double.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                // 원점 (0,0)에서 가장 가까운 점이 왼쪽 위
                double distance = ordered[i].X + ordered[i].Y;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    topLeftIndex = i;
                }
            }

            // 배열을 회전하여 왼쪽 위부터 시계방향 순서로 만들기
            Point2f[] result = new Point2f[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = ordered[(topLeftIndex + i) % 4];
            }

            return result;
        }

        // 두 점 사이 거리 계산
        private static double Distance(Point2f p1, Point2f p2)
        {
            return Math.Sqrt(Math.Pow(p1.X-p2.X, 2) + Math.Pow(p1.Y-p2.Y, 2));
        }

        // 세 점으로 이루어진 각도 계산
        private static double CalculateAngle(Point2f p1, Point2f center, Point2f p3)
        {
            // 중심선을 기준으로 한 벡터 계산
            Point2f v1 = new Point2f(p1.X - center.X, p1.Y - center.Y);
            Point2f v2 = new Point2f(p3.X - center.X, p3.Y - center.Y);

            // 내적과 벡터 크기 계산
            double dot = v1.X * v2.X + v1.Y * v2.Y;
            double mag1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
            double mag2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);

            if (mag1 == 0 || mag2 == 0) return 0;

            // 코사인 각도 계산
            double cosAngle = dot / (mag1 * mag2);
            cosAngle = Math.Max(-1, Math.Min(1, cosAngle));     // -1 ~ 1 범위로 제한

            // 라디안을 각도로 변환
            return Math.Acos(cosAngle) * 180 / Math.PI;
        }

        private static System.Drawing.Bitmap MatToBitmap(Mat mat)
        {
            // Mat을 byte 배열로 변환
            byte[] imageBytes = mat.ToBytes(".jpg");

            using (var ms = new MemoryStream(imageBytes))
            {
                return new System.Drawing.Bitmap(ms);
            }
        }

        public static string OCR(Mat src)
        {
            System.Drawing.Bitmap bitmap = MatToBitmap(src);

            // tesseract로 엔진 인스턴스 생성
            TesseractEngine ocr = new TesseractEngine("tessdata", "eng", EngineMode.LstmOnly);

            // tesseract의 page를 string으로 변환
            Page texts = ocr.Process(bitmap);

            string sentece = texts.GetText().Trim();

            return sentece;
        }
    }
}
