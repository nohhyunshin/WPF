using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// 라이브러리 : 재사용 가능한 코드의 집합
// 개발자가 필요에 따라 호출해서 사용
// 제어의 주도권이 개발자에게 있음

// 프레임워크 : 어플리케이션의 기본 구조 제공
// 개발자의 코드가 프레임워크 내에서 실행
// 제어의 주도권이 프레임워크에게 있음 (제어의 역전)

// NuGet : .NET 생태계의 패키지 관리자
// 전 세계 개발자들이 만든 라이브러리를 쉽게 설치
// 버전 및 의존성 관리 자동화

// ScottPlot.WPF : 디지털 그래프 용지

namespace _0814
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window        // 프레임워크
    {
        public MainWindow()
        {
            InitializeComponent();

            // 그래프 정의 함수 호출
            // CreateFirstGraph();

            SetUpPlot();
        }

        //private void CreateFirstGraph()
        //{
        //    double[] xData = { 1, 2, 3, 4, 5 };
        //    double[] yData = { 2, 4, 6 ,8 ,10 };

        //    // 그래프에 데이터 추가
        //    MyPlot.Plot.Add.Scatter(xData, yData);

        //    // 그래프 새로 고침
        //    MyPlot.Refresh();
        //}

        private void SetUpPlot()
        {
            // 축 위치별 라벨 폰트 설정
            MyPlot.Plot.Axes.Title.Label.FontName = "맑은 고딕";
            MyPlot.Plot.Axes.Bottom.Label.FontName = "맑은 고딕";
            MyPlot.Plot.Axes.Left.Label.FontName = "맑은 고딕";

            // 범례 폰트 설정
            MyPlot.Plot.Legend.FontName = "맑은 고딕";     
            
            // x축 이름 폰트 설정
            MyPlot.Plot.Axes.Bottom.TickLabelStyle.FontName = "맑은 고딕";
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            // 기존 그래프 지우기
            MyPlot.Plot.Clear();

            // 간단한 데이터 준비
            double[] xData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] yData = { 2, 4, 6 ,8 ,10, 22, 33, 25, 53, 63 };

            var line = MyPlot.Plot.Add.Scatter(xData, yData);
            line.LineWidth = 3;
            line.MarkerSize = 0;        // 표식 없는 그래프

            MyPlot.Plot.Title("선형 그래프");
            MyPlot.Plot.XLabel("x축");           // x축 이름
            MyPlot.Plot.YLabel("y축");           // y축 이름

            // 새로 고침
            MyPlot.Refresh();
        }

        private void btnScatter_Click(object sender, RoutedEventArgs e)
        {
            // 기존 그래프 지우기
            MyPlot.Plot.Clear();

            // 간단한 데이터 준비
            double[] xData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] yData = { 2, 4, 6, 8, 10, 22, 33, 25, 53, 63 };

            var dot = MyPlot.Plot.Add.ScatterPoints(xData, yData);
            dot.MarkerSize = 5;

            MyPlot.Plot.Title("점선형 그래프");
            MyPlot.Plot.XLabel("x축");           // x축 이름
            MyPlot.Plot.YLabel("y축");           // y축 이름

            // 새로 고침
            MyPlot.Refresh();
        }

        private void btnBar_Click(object sender, RoutedEventArgs e)
        {
            // 기존 그래프 지우기
            MyPlot.Plot.Clear();

            // 간단한 데이터 준비
            double[] xData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] yData = { 2, 4, 6, 8, 10, 22, 33, 25, 53, 63 };

            var bar = MyPlot.Plot.Add.Bars(xData, yData);
            

            MyPlot.Plot.Title("점선형 그래프");
            MyPlot.Plot.XLabel("x축");           // x축 이름
            MyPlot.Plot.YLabel("y축");           // y축 이름

            // 새로 고침
            MyPlot.Refresh();
        }

        private void btnMultiLine_Click(object sender, RoutedEventArgs e)
        {
            // 기존 그래프 지우기
            MyPlot.Plot.Clear();

            // 간단한 데이터 준비
            double[] xData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] yData1 = { 2, 4, 6, 8, 10, 22, 33, 25, 53, 63 };
            double[] yData2 = { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30 };
            double[] yData3 = { 5, 13, 15, 28, 25, 13, 35, 40, 25, 50 };

            var line1 = MyPlot.Plot.Add.Scatter(xData, yData1);
            line1.LineWidth = 3;
            line1.MarkerSize = 0;
            line1.LegendText = "Y1";

            var line2 = MyPlot.Plot.Add.Scatter(xData, yData2);
            line2.LineWidth = 3;
            line2.MarkerSize = 0;
            line2.LegendText = "Y2";

            var line3 = MyPlot.Plot.Add.Scatter(xData, yData3);
            line3.LineWidth = 3;
            line3.MarkerSize = 0;
            line3.LegendText = "Y3";


            MyPlot.Plot.Title("다중 선형 그래프");
            MyPlot.Plot.XLabel("x축");           // x축 이름
            MyPlot.Plot.YLabel("y축");           // y축 이름
            MyPlot.Plot.ShowLegend();

            // 새로 고침
            MyPlot.Refresh();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if(MyPlot.Plot != null)
            {
                MessageBox.Show("그래프를 삭제하시겠습니까?");
                MyPlot.Plot.Clear();
                MyPlot.Refresh();
            }
        }

        
    }
}