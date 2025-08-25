namespace _0821_2
{   
    // 마우스와 키보드 이벤트 처리
    // 이벤트 기반 프로그래밍
    // 이벤트 : 사용자가 마우스를 클릭하거나 키보드를 누르는 등의 행동
    // 이벤트 처리 : 이러한 행동에 반응하여 프로그램이 동작하는 방식

    // 마우스 이벤트 종류
    // MouseEventTypes
    // MouseMove : 움직임
    // LButtonDown : 좌클릭
    // RButtonDown : 우클릭
    // MButtonDown
    // LButtonUp : 좌클릭 후 떼어짐
    // RButtonUp : 우클릭 후 떼어짐
    // MButtonUp
    // LButtonDoubleClick : 좌 더블 클릭
    // RButtonDoubleClick : 우 더블 클릭
    // MButtonDoubleClick
    // MouseWheel : 휠 움직임
    // MouseHWheel : 가로 휠 움직임

    internal class Program
    {
        static void Main(string[] args)
        {
            // BasicMouseDemo.MouseDemo();
            // KeyBoardEvents.KeyBoardDemo();
            // BasicDragDrawing.DragDrawingDemo();

            ClickCounter.ClickCounterDemo();
        }
    }
}
