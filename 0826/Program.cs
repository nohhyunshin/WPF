using OpenCvSharp;
using System;

namespace _0826
{
    // 원본 이미지 > GrayScale > 블러 > 엣지 검출 > 형태학적 연산
    // 윤곽선 검출 > 사각형 필터링 > 크기 필터링 > 비율 검사
    // 면적 기준 > 중앙 위치 가중치 > 각도 보정 > 신뢰도 계산
    // 

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Mat image = Cv2.ImRead("1.webp");

                if (image.Empty())
                {
                    Console.WriteLine("이미지를 로드할 수 없습니다. 파일 경로를 확인하세요.");
                    return;
                }

                // 이미지 크기 조정
                Mat dst = new Mat();
                Cv2.Resize(image, dst, new Size(800, 600), 0, 0, InterpolationFlags.LinearExact);
               

                // 1단계 : 전처리
                Mat processed = PreprocessForCardDetection.PreprocessForCardDetectionDemo(dst);

                // 2단계 : 후보 검출
                var candidates = PreprocessForCardDetection.FindCardCandidates(processed, dst);

                if (!candidates.Any())
                {
                    Console.WriteLine("명함 후보를 찾을 수 없습니다.");
                    Console.WriteLine("다른 이미지를 시도하거나 전처리 파라미터를 조정하세요.");
                    Cv2.ImShow("Original", dst);
                    Cv2.WaitKey(0);
                    return;
                }

                // 3단계 : 최고 후보 선택
                var bestCandidate = candidates.First();

                // 상위 3개 후보 출력
                var topCandidates = candidates.OrderByDescending(c => c.Confidence).Take(3);
                Console.WriteLine("\n상위 3개 후보:");
                int rank = 1;
                foreach (var candidate in topCandidates)
                {
                    Console.WriteLine($"{rank}. {candidate}");
                    rank++;
                }

                // 4단계 : 명함 추출
                Mat extractedCard = PreprocessForCardDetection.ExtracntCard(dst, bestCandidate);

                // 5단계 : 결과 표시
                PreprocessForCardDetection.DrawDetectionResult(dst, candidates, bestCandidate);

                // 결과 표시
                Cv2.ImShow("original", dst);
                Cv2.ImShow("extractedCard", extractedCard);

                string str = PreprocessForCardDetection.OCR(extractedCard);
                Console.WriteLine($"카드 글자 : {str}");

                Cv2.WaitKey(0);

                // 메모리 정리
                image.Dispose();
                dst.Dispose();
                processed.Dispose();
                extractedCard.Dispose();

                Cv2.DestroyAllWindows();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
                Console.WriteLine($"스택 트레이스: {ex.StackTrace}");
            }
        }
    }
}
