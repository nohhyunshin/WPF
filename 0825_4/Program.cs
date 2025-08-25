namespace _0825_4
{
    // Contour
    // 동일한 색상이나 밝기를 가진 연속된 점들의 경계선

    // Edge vs Contour
    // edge
    // 픽셀 단위의 경계 검출
    // 개별 점들의 집합
    // 연결성 보장 안 됨

    // contour
    // 연결된 경계선 검출
    // 닫힌 모양 형성
    // 물체의 완전한 윤곽

    // External Contour(외부 윤곽선)
    // 물체의 가장 바깥쪽 경계
    // 가장 중요한 윤곽선

    // Internal Contour(내부 윤곽선)
    // 물체 내부의 구멍이나 빈 공간의 경계
    // 도넛의 가운데 구멍 같은 부분

    // 계층 구조 (hierachy)
    // 물체 안에 물체가 있을 때 포함 관계를 나타냄

    // 큰 사각형 --- 작은 사각형 --- 더 작은 원
    //          |   
    //           --- 작은 사각형 2

    // Hierachy 배열 구조
    // Next : 같은 레벨의 다음 contour
    // Previous : 같은 레벨의 이전 contour
    // First_child : 첫 번째 자식 contour
    // Parent : 부모 contour

    internal class Program
    {
        static void Main(string[] args)
        {
            BasicContour.BasicContourDemo();
        }
    }
}
