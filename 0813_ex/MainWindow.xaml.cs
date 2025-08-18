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

namespace _0813_ex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Book> books;
        public MainWindow()
        {
            InitializeComponent();
            LoadSimpleData();
        }

        private void LoadSimpleData()
        {
            books = new List<Book>
            {
                new Book("폭우 속 우주", "청예", 15000, "소설"),
                new Book("여름 피치 스파클링", "차정은", 10000, "시"),
                new Book("돌이킬 수 있는", "문목하", 18000, "소설")
            };
            // 바인딩
            BookData.ItemsSource = books;
        }
            

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            books.Add(new Book("사랑과 결함", "예소현", 8000, "소설"));

            MessageBox.Show("데이터 추가가 완료되었습니다.", "데이터 추가",
                MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(books.Count.ToString(), "데이터 수");

            // 갱신
            BookData.Items.Refresh();
        }
    }
}