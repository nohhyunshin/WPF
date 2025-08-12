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

namespace _0812_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool btn_image1 = true;
        Uri urlImg1;
        Uri urlImg2;

        public MainWindow()
        {
            InitializeComponent();

            this.urlImg1 = new Uri(@"pack://application:,,,/Resources/img1.jpeg", UriKind.Absolute);
            this.urlImg2 = new Uri(@"pack://application:,,,/Resources/img2.jpeg", UriKind.Absolute);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var uriSource = new Uri(@"/0812_2;component/Resources/img2.jpeg", UriKind.Relative);
            //imgBox.Source = new BitmapImage(uriSource);

            if(this.btn_image1)
            {
                btnImage.Background = new ImageBrush(new BitmapImage(this.urlImg1));
                imgBox.Source = new BitmapImage(this.urlImg2);
                this.btn_image1 = false;
            } else
            {
                btnImage.Background = new ImageBrush(new BitmapImage(this.urlImg2));
                imgBox.Source = new BitmapImage(this.urlImg1);
                this.btn_image1 = true;
            }
        }
    }
}