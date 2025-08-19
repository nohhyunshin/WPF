using OpenCvSharp;
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
using Point = OpenCvSharp.Point;


// Vector 구조체
// 2D (x, y) / 3D (x, y, z) 좌표를 표현하는 기본 구조체

// OpenCv 기본 데이터 구조
// OpenCv는 효율적인 이미지 처리를 위해 다양한 기하학적 객체와 데이터 구조 제공
// 이러한 구조들은 메모리 효율성과 여산 속도를 최적화하도록 설계됨

namespace _0819
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            textBox.Text = Cv2.GetVersionString();

            Mat image = Cv2.ImRead("img1.jpeg");

            Cv2.PutText(image, "Hello OpenCv", new Point(50, 150),
                        HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 255, 255), 2);

            Cv2.ImShow("image test", image);
            ImageBox.Source = OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(image);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}