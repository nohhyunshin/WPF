using OpenCvSharp;
using System.Runtime.CompilerServices;

namespace _0821
{
    // 이미지 입출력
    // 디지털 이미지 파일
    // 컴퓨터에서 이미지를 저장하는 방법은 여러가지 있으며
    // 각각 다른 특징과 용도를 가지고 있음
    
    // 주요 이미지 파일 형식

    // JPEG(.jpg .jpeg)
    // 웹사이트 이미지, 소셜 미디어 업로드, 파일 크기가 중요한 경우
    // 특징 ① 압출 방식 : 손실 압축
    // 특징 ② 파일 크기 : 작음 (압축률 높음)
    // 특징 ③ 품질 : 압축 시 일부 정보 손실
    // 특징 ④ 투명도 : 지원 안 함

    // PNG (.png)
    // 로고, 아이콘, 스크린샷, 투명 배경이 필요한 이미지, 품질이 중요한 그래픽
    // 특징 ① 압축 방식 : 무손실 압축
    // 특징 ② 파일 크기 : 중간 (JPEG보다 크지만 품질 손실 없음)
    // 특징 ③ 품질 : 원본과 동일
    // 특징 ④ 투명도 : 지원 (알파 채널)

    // BMP (.bmp)
    // 의료 영상 분석, 정밀한 이미지 처리, 임시 작업 파일
    // 특징 ① 압축 방식 : 주로 비압축
    // 특징 ② 파일 크기 : 매우 큼
    // 특징 ③ 품질 : 최고 (무손실)
    // 특징 ④ 호환성 : 모든 시스템에서 지원
    
    // TIFF (.tif .tiff)
    // 과학 / 의료 영상 인쇄용 고품질 이미지, 아카이브 저장용
    // 특징 ① 압축 방식 : 무손실 압축 가능
    // 특징 ② 파일 크기 : 큼
    // 특징 ③ 품질 : 최고
    // 특징 ④ 메타 데이터 : 풍부한 정보 저장 가능

    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            // 안전 처리 : 파일 경로를 따로 생성하고 존재 여부 확인
            // 항상 확인할 것을 권장함!!
            string imagePath = "img3.jpeg";

            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"이미지 파일을 찾을 수 없습니다.");
                return;
            }

            using (Mat unChangedimage = Cv2.ImRead(imagePath, ImreadModes.Unchanged))
            using (Mat colorimage = Cv2.ImRead(imagePath, ImreadModes.Color))
            using (Mat grayimage = Cv2.ImRead(imagePath, ImreadModes.Grayscale))
            {
                if (unChangedimage.Empty()) {
                    Console.WriteLine("이미지를 읽을 수 없습니다.";
                }

                Cv2.ImShow("unChanged", unChangedimage);
                Cv2.ImShow("colorimage", colorimage);
                Cv2.ImShow("grayimage", grayimage);

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }
            */

            // 이미지 저장
            using (Mat testImage = CreateTestImage())
            {
                // 생성된 이미지 출력
                Cv2.ImShow("Test Image", testImage);

                string outputDir = "saved_images";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                    Console.WriteLine($"출력 폴더 생성 : {outputDir}");
                }

                SaveMultipleFormater(testImage, outputDir);
                Console.WriteLine("파일 저장 완료!");

                CompareFileSizes(outputDir);

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }

        }

        // 이미지 생성
        private static Mat CreateTestImage()
        {
            Mat image = new Mat(400, 600, MatType.CV_8UC3, Scalar.White);
            var indexer = image.GetGenericIndexer<Vec3b>();

            for (int i=0; i<image.Height; i++)
            {
                for (int j=0; j<image.Width; j++)
                {
                    byte blue = (byte)(j * 255 / image.Width);
                    byte green = (byte)(i * 255 / image.Width);
                    indexer[i, j] = new Vec3b(blue, green, 128);
                }
            }
            return image;
        }

        // 이미지 저장
        private static void SaveMultipleFormater(Mat image, string outputDir)
        {
            // png 저장 (무손실)
            string pngPath = Path.Combine(outputDir, "test_image.png");
            bool pngSuccess = Cv2.ImWrite(pngPath, image);
            Console.WriteLine($"png 저장 : {(pngSuccess ? "성공" : "실패")} - {pngPath}");

            // bmp 저장 (비압축)
            string bmpPath = Path.Combine(outputDir, "test_image.bmp");
            bool bmpSuccess = Cv2.ImWrite(bmpPath, image);
            Console.WriteLine($"png 저장 : {(bmpSuccess ? "성공" : "실패")} - {bmpPath}"); 

            // jpeg 저장 (높은 품질)
            string jpegHighPath = Path.Combine(outputDir, "text_image_high.jpg");
            var jpegHighParams = new int[] {(int)ImwriteFlags.JpegQuality, 95 };
            bool jpegHighSuccess = Cv2.ImWrite(jpegHighPath, image, jpegHighParams);
            Console.WriteLine($"png 저장 : {(jpegHighSuccess ? "성공" : "실패")} - {jpegHighPath}"); 

            // jpeg 저장 (보통 품질)
            string jpegMedPath = Path.Combine(outputDir, "text_image_med.jpg");
            var jpegMedParams = new int[] {(int)ImwriteFlags.JpegQuality, 50 };
            bool jpegMedSuccess = Cv2.ImWrite(jpegMedPath, image, jpegMedParams);
            Console.WriteLine($"png 저장 : {(jpegMedSuccess ? "성공" : "실패")} - {jpegMedPath}");

            // jpeg 저장 (낮은 품질)
            string jpegLowPath = Path.Combine(outputDir, "text_image_low.jpg");
            var jpegLowParams = new int[] {(int)ImwriteFlags.JpegQuality, 10 };
            bool jpegLowSuccess = Cv2.ImWrite(jpegLowPath, image, jpegLowParams);
            Console.WriteLine($"png 저장 : {(jpegLowSuccess ? "성공" : "실패")} - {jpegLowPath}"); 
        }

        // 파일 크기 비교
        private static void CompareFileSizes(string outputDir)
        {
            Console.WriteLine("파일 크기 비교");

            var files = new[]
            {
                "test_image.png",
                "test_image.bmp",
                "text_image_high.jpg",
                "text_image_med.jpg",
                "text_image_low.jpg"
            };

            foreach (string file in files)
            {
                string fullPath = Path.Combine(outputDir, file);
                if(File.Exists(fullPath))
                {
                    FileInfo fileInfo = new FileInfo(fullPath);
                    Console.WriteLine($"{file} : {fileInfo.Length:N0} bytes ({fileInfo.Length / 1024.0:F1} KB)");
                }
            }
        }
    }
}
