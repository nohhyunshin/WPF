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

namespace _0818_ex
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

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            int number1 = int.Parse(num1.Text);
            int number2 = int.Parse(num2.Text);
            Button btn = sender as Button;

            if (btn.Content.ToString() == "+")
            {
                int answer = number1 + number2;
                display.Text = answer.ToString();
            }
            else if (btn.Content.ToString() == "-")
            {
                int answer = number1 - number2;
                display.Text = answer.ToString();
            }
            else if (btn.Content.ToString() == "*")
            {
                int answer = number1 * number2;
                display.Text = answer.ToString();
            }
            else if (btn.Content.ToString() == "/")
            {
                int answer = (number2 != 0) ? number1 / number2 : 0;
                display.Text = answer.ToString();
            }
        }
    }
}