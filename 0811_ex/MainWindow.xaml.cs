using System.Net.Http.Headers;
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

namespace _0811_ex
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string id = "admin";
            string pswd = "1234";

            if (txtID.Text == "" || txtPSWD.Password == "")
            {
                MessageBox.Show("아이디와 비밀번호 모두 입력해 주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else if (txtID.Text == id && txtPSWD.Password == pswd)
            {
                MessageBox.Show($"로그인 성공! 환영합니다.\n로그인 시도 : {txtID.Text} / {txtPSWD.Password}", "로그인 성공", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("아이디 혹은 비밀번호가 일치하지 않습니다.", "로그인 실패", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}