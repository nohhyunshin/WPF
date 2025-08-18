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

namespace _0814_ex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUpPlot();
        }

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

        // 일주일 단위 요일별 운동 기록 그래프
        private void btnWeek_Click(object sender, RoutedEventArgs e)
        {
            MyPlot.Plot.Clear();

            string[] week = { "월", "화", "수", "목", "금", "토", "일" };
            double[] weekData = { 1, 2, 3, 4, 5, 6, 7 };
            double[] timeData = { 30, 45, 0, 60, 30, 90, 120 };

            // x축 눈금에 요일 이름 붙이기
            MyPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(weekData, week);

            var exc = MyPlot.Plot.Add.Scatter(weekData, timeData);
            exc.LineWidth = 2;
            exc.MarkerSize = 0;

            MyPlot.Plot.Title("운동 기록");
            MyPlot.Plot.XLabel("요일");
            MyPlot.Plot.YLabel("운동 시간 (단위 : 분)");

            // 새로 고침
            MyPlot.Refresh();
        }

        // 지점별 월별 매출 그래프
        private void btnMonthly_Click(object sender, RoutedEventArgs e)
        {
            MyPlot.Plot.Clear();

            // 간단한 데이터 준비
            string[] xData1 = { "강남점", "홍대점", "잠실점" };
            double[] xData = { 1, 2, 3 };
            double[] yData1 = { 100, 120, 140 };
            double[] yData2 = { 80, 90, 95 };
            double[] yData3 = { 120, 110, 130 };

            // x축 눈금에 지점 이름 붙이기
            MyPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(xData, xData1);
            var line1 = MyPlot.Plot.Add.Scatter(xData, yData1);
            line1.LineWidth = 3;
            line1.MarkerSize = 0;
            line1.LegendText = "강남점";

            var line2 = MyPlot.Plot.Add.Scatter(xData, yData2);
            line2.LineWidth = 3;
            line2.MarkerSize = 0;
            line2.LegendText = "홍대점";

            var line3 = MyPlot.Plot.Add.Scatter(xData, yData3);
            line3.LineWidth = 3;
            line3.MarkerSize = 0;
            line3.LegendText = "잠실점";


            MyPlot.Plot.Title("지점별 매출 그래프");
            MyPlot.Plot.XLabel("지점");           // x축 이름
            MyPlot.Plot.YLabel("매출");           // y축 이름
            MyPlot.Plot.ShowLegend();

            // 새로 고침
            MyPlot.Refresh();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (MyPlot.Plot != null)
            {
                MessageBox.Show("그래프를 삭제하시겠습니까?");
                MyPlot.Plot.Clear();
                MyPlot.Refresh();
            }
        }
    }
}