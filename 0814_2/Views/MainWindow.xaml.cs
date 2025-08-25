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
using _0814_2.ViewModels;

// MVVM 패턴
// 요리사 (Model)
// 주문서 (ViewModel)
// 웨이터 (View)

// ① 고객이 웨이터(View)에게 주문
// ② 웨이터(View)가 주문서(ViewModel)에 기록
// ③ 주문서(ViewModel)를 보고 요리사(Model)이 음식 준비
// ④ 음식이 준비되면 요리사(Model) > 웨이터(ViewModel) > 손님에게 전달

// Model : 데이터와 비즈니스 로직
// View : 사용자 인터페이스 (XAML)
// ViewModel : View와 Model 사이의 중간 연결자

namespace _0814_2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new StudentViewModel();
        }
    }
}

// 코드가 뒤섞임 : UI 로직 + 데이터 로직 + 비즈니스 로직
// 테스트, 재사용, 유지보수 모두 어려움 > 팀 협업 어려움

// MVVM
// 명확한 역할 분담 가능
// 테스트와 유지보수가 쉽고 재사용성이 높음 > 팀 협업 용이