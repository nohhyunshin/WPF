using _0814_3.Models;
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

namespace _0814_3.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var student = new Student
            {
                Name = "김철수",
                Age = 20,
                Score = 80
            };

            MessageBox.Show($"이름 : {student.Name}");
            MessageBox.Show($"나이 : {student.Age}");
            MessageBox.Show($"점수 : {student.Score}");
            MessageBox.Show($"등급 : {student.Grade}");
            MessageBox.Show($"합격 여부 : {student.IsPassed}");
        }
    }
}