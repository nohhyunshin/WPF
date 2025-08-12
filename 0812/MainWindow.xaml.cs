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

namespace _0812
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            // lblSliderValue가 아직 생성되지 않았을 수도 있으므로 if문 사용
            if (lblSliderValue != null )
            {
                lblSliderValue.Text = $"값 : {slider.Value:F0}";
                // 진행률 바도 슬라이더 값과 함께 없데이트
                progressBar.Value = slider.Value;
            }
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("일반 버튼 클릭!");
        }

        private void btnToggle_Checked(object sender, RoutedEventArgs e)
        {
            btnToggle.Content = "토글 on";
            btnDisabled.IsEnabled = false;
        }

        private void btnToggle_UnChecked(object sender, RoutedEventArgs e)
        {
            btnToggle.Content = "토글 off";
            btnDisabled.IsEnabled = true;
        }
    }
}