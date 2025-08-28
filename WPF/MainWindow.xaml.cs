using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClosedXML.Excel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ErrorItem> ErrorList { get; set; }
        private int errorIndex = 1;
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

            // 검색 조건이 있을 때만 필터 적용
            if (!string.IsNullOrEmpty(selectedError) || selectedDate.HasValue)
            {
                view.Filter = obj =>
                {
                    var errorItem = obj as ErrorItem;
                    if (errorItem == null) return false;

                    // OR 연산자이기 때문에 selectedError와 selectedDate가 비어 있는지를 검사하고
                    // OR 연산자는 둘 중 하나가 false면 최종적으로 false니까
                    // 값이 비어 있다면 true, 그리고 값이 있다면 일치하는가? 일치한다면 true 아니면 false
                    bool codeError = string.IsNullOrEmpty(selectedError) || errorItem.ErrorName == selectedError;
                    bool dateError = !selectedDate.HasValue || (DateTime.TryParse(errorItem.ErrorTime, out DateTime errorDateTime)) && errorDateTime.Date == selectedDate.Value.Date;

                    return codeError && dateError;
                };
            }
            else
            {
                // 검색 조건이 없다면 필터 해제 → 전체 보기
                view.Filter = null;
            }
            // 검색 후 초기화
            ErrorCodeName.Text = "코드 선택";    // 수정 필요
            ErrorCodeName.SelectedItem = null;
            ErrorDate.SelectedDate = null;
        }

        // 저장 버튼 클릭 (csv 파일로 출력)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 사용자가 파일 이름 지정 가능하게
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Excel 파일 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*",
                DefaultExt = "xlsx",
                FileName = $"Defective List_{DateTime.Now:yyyy.MM.dd}"
            };

            if (saveFile.ShowDialog() == true)
            {
                var filePath = saveFile.FileName;

                // 감지된 에러 정보 가져오기
                var items = DataGridErrors.ItemsSource as IEnumerable<ErrorItem>;
                if (items == null) return;

                XLWorkbook workbook;
                IXLWorksheet ws;

                if (File.Exists(filePath))
                {
                    // 기존 파일 열기
                    workbook = new XLWorkbook(filePath);
                    ws = workbook.Worksheet("Errors") ?? workbook.AddWorksheet("Errors");
                }
                else
                {
                    // 새 파일 생성
                    workbook = new XLWorkbook();
                    ws = workbook.AddWorksheet("Errors");
                }

                // 기존 마지막 행 확인
                int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
                // 처음 저장하는 게 아니면 마지막 행 + 1부터 저장
                int row = lastRow == 0 ? 1 : lastRow + 1;

                // 처음 저장할 때만 열 머리글 작성
                if (lastRow == 0)
                {
                    ws.Cell(1, 1).Value = "Part Number";
                    ws.Cell(1, 2).Value = "Error Code";
                    ws.Cell(1, 3).Value = "Date";
                    row = 2;
                }

                foreach (var item in items)
                {
                    ws.Cell(row, 1).Value = item.ErrorIdx;
                    ws.Cell(row, 2).Value = item.ErrorName;
                    ws.Cell(row, 3).Value = item.ErrorTime;
                    row++;
                }

                ws.Range(1, 1, row - 1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(1, 1, row - 1, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Columns().AdjustToContents();

                workbook.SaveAs(filePath);
                workbook.Dispose();
            }
            // 저장되면 리스트 초기화
            ErrorList.Clear();
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

        // 돌아가기 버튼
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(DataGridErrors.ItemsSource);

            if (view != null)
            {
                view.Filter = null;
            }
        }
    }
}