using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _0814_ex2.Commands;
using _0814_ex2.Models;

namespace _0814_ex2.ViewModels
{
    class CalculatorViewModel : BaseViewModel
    {
        private readonly Calculator _calculator = new Calculator();

        private string _firstNumber;
        private string _secondNumber;
        private string _resultNumber;

        // 사용자가 TextBox에 입력 > ViewModel의 FirstNumber의 속성 변경
        // ViewModel에서 Result 속성 변경 > UI의 TextBox 값은 업데이트 x

        // WPF는 ViewModel의 속성이 언제 바뀌었는지 모름
        // 속성이 바뀌었다는 알림이 없으면 UI를 업데이트 하지 않음

        // INotifyPropertyChanged
        // ViewModel 속성 변경 > PropertyChanged 이벤트 발생
        // 이후에 WPF Binding 엔진이 이벤트 감지 > 해당 속성의 새 값을 가져와서 UI 변경

        public event PropertyChangedEventHandler? PropertyChanged;

        public string FirstNumber
        {
            get { return _firstNumber; }
            set => SetProperty(ref _firstNumber, value, () =>
            {
                // 값이 실제로 변경되었을 때만 실행되는 추가 작업
                CommandManager.InvalidateRequerySuggested();
            });
        }

        public string SecondNumber
        {
            get { return _secondNumber; }
            set => SetProperty(ref _secondNumber, value, () =>
            {
                CommandManager.InvalidateRequerySuggested();
            });   
        }

        public string ResultNumber
        {
            get { return _resultNumber; }
            set => SetProperty(ref _resultNumber, value, () =>
            {
                CommandManager.InvalidateRequerySuggested();
            });
        }

        // Command : 각 버튼에 연결된 명령들
        public ICommand AddCommand { get; }
        public ICommand SubCommand { get; }
        public ICommand MulCommand { get; }
        public ICommand DivCommand { get; }
        public ICommand ClearCommand { get; }

        // 생성자
        public CalculatorViewModel()
        {
            AddCommand = new RelayCommand(
                execute => Calculate("+"),         // 실행할 메소드
                canExecute: _ => CanCalculate()     // 실행 가능 조건
            );

            SubCommand = new RelayCommand(
                execute => Calculate("-"),         // 실행할 메소드
                canExecute: _ => CanCalculate()     // 실행 가능 조건
            );

            MulCommand = new RelayCommand(
                execute => Calculate("*"),         // 실행할 메소드
                canExecute: _ => CanCalculate()     // 실행 가능 조건
            );
            
            DivCommand = new RelayCommand(
                execute => Calculate("/"),         // 실행할 메소드
                canExecute: _ => CanCalculate()     // 실행 가능 조건
            );
            
            ClearCommand = new RelayCommand(
                execute: _ => Clear()         // 실행할 메소드
            );
        }

        private bool CanCalculate()
        {
            return _calculator.IsValidNumber(_firstNumber) &&
                _calculator.IsValidNumber(_secondNumber);
        }

        private void Calculate(string operation)
        {
            try
            {
                double num1 = _calculator.ParseNumber(_firstNumber);
                double num2 = _calculator.ParseNumber(_secondNumber);

                double result = _calculator.Calculate(num1, num2, operation);
                ResultNumber = $"{num1} {operation} {num2} = {result}";
            }
            catch (Exception ex)
            {
                ResultNumber = $"오류 : {ex.Message}";
            }
        }

        private void Clear()
        {
            FirstNumber = "";
            SecondNumber = "";
            ResultNumber = "";
        }
    }
}