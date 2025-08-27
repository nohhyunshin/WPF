using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ErrorItem> ErrorList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ErrorList = new ObservableCollection<ErrorItem>();
            DataGridErrors.ItemsSource = ErrorList;
        }

        // DataGrid에 표시할 데이터 구조
        public class ErrorItem
        {
            public int ErrorIdx { get; set; }
            public string ErrorName { get; set; }
            public string ErrorTime { get; set; }
        }

        public void AddError(int idx, string errorName, DateTime time)
        {
            // UI 스레드 안전하게 업데이트
            Dispatcher.Invoke(() =>
            {
                ErrorList.Add(new ErrorItem
                {
                    ErrorIdx = idx,
                    ErrorName = errorName,
                    ErrorTime = time.ToString("yyyy-MM-dd HH:mm:ss")
                });
            });
        }

        // 검색 버튼 클릭
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //WPF의 ItemsControl은 View를 통해 데이터 표시
            ICollectionView view = CollectionViewSource.GetDefaultView(DataGridErrors.ItemsSource);

            // 선택된 항목 - ErrorCode
            var selectedName = ErrorCodeName.SelectedItem as ComboBoxItem;
            string selectedError = selectedName?.Content.ToString();

            // 선택된 항목 - Date
            var selectedDate = ErrorDate.SelectedDate;

            if (view != null)
            {
                view.Filter = obj =>
                {
                    var errorItem = obj as ErrorItem;

                    // OR 연산자이기 때문에 selectedError와 selectedDate가 비어 있는지를 검사하고
                    // OR 연산자는 둘 중 하나가 false면 최종적으로 false니까
                    // 값이 비어 있다면 true, 그리고 값이 있다면 일치하는가? 일치한다면 true 아니면 false
                    return errorItem != null &&
                           (string.IsNullOrEmpty(selectedError) || errorItem.ErrorName == selectedError) &&
                           (!selectedDate.HasValue || (DateTime.TryParse(errorItem.ErrorTime, out DateTime errorDateTime)) && errorDateTime.Date == selectedDate.Value.Date);
                };
            }
            // 검색 후 초기화
            ErrorCodeName.Text = ErrorCodeName.Text;
            ErrorCodeName.SelectedItem = null;
            ErrorDate.SelectedDate = null;
        }

        // 저장 버튼 클릭 (csv 파일로 출력)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // 삭제 버튼 클릭 (선택 항목 개별 삭제)
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            // 데이터 그리드 내에서 선택된 항목
            var selected = DataGridErrors.SelectedItem as ErrorItem;
            if (selected != null)
            {
                ErrorList.Remove(selected);
            }
        }
    }
}