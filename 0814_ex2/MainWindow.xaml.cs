using System.Diagnostics;
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
using _0814_ex2.ViewModels;

namespace _0814_ex2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new CalculatorViewModel();
        }

        // code - behind 패턴
        // 하나의 메소드가 너무 많은 책임을 가지고 있다
        // UI와 로직이 강하게 결합
        // 코드 수정 시 다른 부분에 영향을 줄 수 있다

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            int number1 = int.Parse(num1.Text);
            int number2 = int.Parse(num2.Text);
            Button btn = sender as Button;

            if (btn.Content.ToString() == "+")
            {
                int answer = number1 + number2;
                display.Text = answer.ToString();
            } else if (btn.Content.ToString() == "-")
            {
                int answer = number1 - number2;
                display.Text = answer.ToString();
            } else if (btn.Content.ToString() == "*")
            {
                int answer = number1 * number2;
                display.Text = answer.ToString();
            } else if (btn.Content.ToString() == "/")
            {
                int answer = (number2 != 0) ? number1 / number2 : 0;
                display.Text = answer.ToString();
            }
        }
    }
}