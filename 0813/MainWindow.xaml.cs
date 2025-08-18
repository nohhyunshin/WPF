using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static System.Reflection.Metadata.BlobBuilder;

// DataGrid : 표 형태의 데이터를 보여 주는 컨트롤
// 행과 열로 데이터 정리 ex) 학급 성적표, 상품 목록표
// 엑셀 스프레드시트와 유사

// 기본 기능
// 데이터 표시 - 여러 데이터를 표 형태로 깔끔하게 보여 줌
// 편집 기능 - 사용자가 직접 데이터를 수정할 수 있음
// 정렬 기능 - 열 헤더를 클릭하면 자동으로 정렬
// 선택 기능 - 행이나 셀을 선택할 수 있음
// 스크롤 - 데이터가 많으면 자동으로 스크롤바 생성

// 필터링, 그룹화, 사용자 정의 템플릿, 데이터 바인딩

// ObservableCollection
// 데이터가 추가되면 자동으로 DataGrid에 새 행 추가
// 데이터가 삭제되면 자동으로 DataGrid의 행 삭제
// 데이터가 변경되면 자동으로 DataGrid의 행 업데이트

namespace _0813
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 변경을 자동으로 UI에게 알림
        private ObservableCollection<Student> students;
        private CollectionViewSource viewSource;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();       // 데이터 초기화

            // 필터링
            SetUpFiltering();
        }

        // 통계 정보 업데이트
        private void update()
        {
            var visibleStudent = viewSource.View.Cast<Student>().ToList();
            total.Text = $"총 학생 : {visibleStudent.Count.ToString()}명";

            if (visibleStudent.Count > 0)
            {
                double avg = visibleStudent.Average(s => s.Score);
                avgScore.Text = $"평균 점수 : {avg:F1}점";

                int pass = visibleStudent.Count(s => s.Score >= 80);
                int fail = visibleStudent.Count - pass;

                passCount.Text = $"합격 : {pass}명";
                failCount.Text = $"불합격 : {fail}명";
            } else
            {
                total.Text = $"총 학생 : {0}명";  
                avgScore.Text = $"평균 점수 : {0}점";
                passCount.Text = $"합격 : {0}명";
                failCount.Text = $"불합격 : {0}명";
            }
        }

        private void InitializeData()
        {
            // 클래스 사용
            students = new ObservableCollection<Student>
            {
                new Student("김철수", 20, 80),
                new Student("이영희", 24, 85),
                new Student("박민수", 26, 90),
                new Student("홍길동", 19, 60),
                new Student("정호석", 21, 40),
            };

            viewSource = new CollectionViewSource();
            viewSource.Source = students;
            myDataGrid.ItemsSource = viewSource.View;

            update();
        }

        private void SetUpFiltering()
        {
            viewSource.Filter += ViewSource_Filter;
        }

        // 필터 이벤트 처리
        private void ViewSource_Filter(object sender, FilterEventArgs e)
        {
            Student student = e.Item as Student;
            if (student == null)
            {
                e.Accepted = false;
                return;
            }

            // 이름 검색 필터
            string searchText = txtSearch.Text;
            bool nameMatch = string.IsNullOrEmpty(searchText) || student.Name.Contains(searchText);

            // 등급 필터
            // 이때 ?는 물음표 앞의 조건이 일치하면 가져오고 아니면 말겠다
            string selectedGrade = (cmbGradeFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
            bool gradeMatch = selectedGrade == "전체" || selectedGrade == student.Grade;

            e.Accepted = nameMatch && gradeMatch;
            update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            students.Add(new Student("나유리", 24, 40));

            MessageBox.Show("데이터 추가가 완료되었습니다.", "데이터 추가",
                MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(students.Count.ToString(), "데이터 수");

            update();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 입력값 가져오기
                string name = txtName.Text.Trim();
                int age = int.Parse(txtAge.Text.Trim());
                double score = double.Parse(txtScore.Text.Trim());

                // 유효성 검사
                if(string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("이름을 입력해 주세요!", "오류" ,MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 새 학생 객체 생성 및 추가
                Student newStudent = new Student(name, age, score);
                students.Add(newStudent);

                // 입력 창 초기화
                ClearInputFields();

                MessageBox.Show($"{name} 학생이 추가되었습니다.", "추가", MessageBoxButton.OK, MessageBoxImage.Information);
                update();

            } catch (Exception ex)
            {
                MessageBox.Show($"입력 오류 : {ex.Message}");
            }
        }

        private void ClearInputFields()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtScore.Text = "";

            txtName.Focus();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Student selected_std = myDataGrid.SelectedItem as Student;

            if (selected_std != null)
            {
                // 삭제 확인
                MessageBoxResult result = MessageBox.Show($"{selected_std.Name} 학생을 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo);

                if(result == MessageBoxResult.Yes) {
                    students.Remove(selected_std);
                    MessageBox.Show("삭제되었습니다.");
                    update();
                }
            } else
            {
                MessageBox.Show("삭제할 학생을 선택해 주세요.");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (students.Count == 0)
            {
                MessageBox.Show("삭제할 데이터가 없습니다.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("모든 학생 데이터를 삭제하시겠습니까?", "전체 삭제 확인", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                students.Clear();
                MessageBox.Show("모든 데이터가 삭제되었습니다.");
                update();
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "CSV 파일 (*.csv)|*.csv",
                    DefaultExt = "csv",
                    FileName = $"학생 목록_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    StringBuilder csv = new StringBuilder();

                    // 헤더 추가
                    csv.AppendLine("이름, 나이, 점수, 등급");

                    foreach (Student student in students)
                    {
                        csv.AppendLine($"{student.Name}, {student.Age}, {student.Score}, {student.Grade}");
                    }

                    File.WriteAllText(saveDialog.FileName, csv.ToString(), Encoding.UTF8);
                    MessageBox.Show("저장 완료!");
                    update();
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"파일 저장 오류 : {ex.Message}");
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // students.Clear();                // 기존의 목록 초기화 후 불러오기
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Filter = "CSV 파일 (*.csv)|*.csv",
                    Title = "학생 데이터 파일 선택",
                };

                if (openDialog.ShowDialog() == true)
                {
                    string[] lines = File.ReadAllLines(openDialog.FileName, Encoding.UTF8);

                    // line의 길이가 2 미만이면 데이터 없다는 뜻
                    if (lines.Length < 2)
                    {
                        MessageBox.Show("읽어 올 데이터가 없습니다.");
                        return;
                    }

                    int importCount = 0;
                    for (int i = 1; i < lines.Length; i++)
                    {
                        // 콤마를 기준으로 문자열 배열 생성
                        string[] parts = lines[i].Split(",");

                        if (parts.Length >= 3)
                        {
                            string name = parts[0].Trim();
                            if (int.TryParse(parts[1].Trim(), out int age) && double.TryParse(parts[2].Trim(), out double score))
                            {
                                students.Add(new Student(name, age, score));
                                importCount++;
                            }
                        }
                    }
                    MessageBox.Show($"{importCount}명의 학생 데이터를 가져왔습니다.");
                    update();
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"파일 리딩 오류 : {ex.Message}");
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewSource.View.Refresh();
            update();
        }

        private void cmbGradeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewSource != null)
            {
                viewSource.View.Refresh();
                update();
            }
        }
    }
}