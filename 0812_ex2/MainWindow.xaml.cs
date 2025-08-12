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

namespace _0812_ex2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            sliderR.Value = 0;
            sliderG.Value = 0;
            sliderB.Value = 0;

            Update();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (optGray == null || optInvert == null) return;
            Update();
        }

        public void Update()
        {
            byte r = (byte)(sliderR.Value);
            byte g = (byte)(sliderG.Value);
            byte b = (byte)(sliderB.Value);

            if (optGray.IsChecked == true)
            {
                byte gray = (byte)((r + g + b) / 3);
                r = g = b = gray;

                sliderR.Value = r;
                sliderG.Value = g;
                sliderB.Value = b;
            }
            else if (optInvert.IsChecked == true)
            {
                r = (byte)(255 - r);
                g = (byte)(255 - g);
                b = (byte)(255 - b);
            }

            Color color = Color.FromRgb(r, g, b);
            SolidColorBrush brush = new SolidColorBrush(color);
            rgbCanvas.Background = brush;
        }
    }
}