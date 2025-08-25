using OpenCvSharp;

namespace _0822
{
    // 카메라와 비디오 처리
    // 비디오 : 연속된 이미지(프레임)의 집합
    // 1초에 30장의 사진을 빠르게 보여 주면 동영상
    // 각 사진은 프레임이고 초당 프레임 수는 FPS(Frames Per Second)

    internal class Program
    {
        static void Main(string[] args)
        {
            // BasicCameraDemo.BasicCameraUsage();     // 카메라
            // CameraControlDemo.CameraWithControls();

            // BasicVideoPlayerDemo.PlayVideoFile();       // 비디오
            CameraEffectsPracticeDemo.CameraEffectsPractice();   // 실습
        }
    }
}
