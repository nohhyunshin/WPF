using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _0814_ex2.Commands
{
    // Command 패턴
    // 요청 (버튼 클릭)을 객체로 캡슐화하여 UI와 로직을 분리하는 디자인 패턴

    // 기존 방식
    // 버튼 클릭 > 직접 메소드 호출 > UI 코드 실행

    // Command 클릭
    // 버튼 클릭 > Command 객체 > ViewModel 메소드 > Model 호출

    class RelayCommand : ICommand
    {
        // 실제로 실행할 메소드 저장 (필수)
        // Action<object>는 void MEthod(object param) 형태의 메소드를 가리킴
        private readonly Action<object> _execute;

        // 실행 가능 여부 판단 메소드 저장 (선택 사항)
        // Func<object, bool>은 voll Method(object param) 형태의 메소드를 가리킴
        private readonly Func<object, bool> _canExecute;

        public bool CanExecute(Object parameter)
        {
            // _canExecute가 null이면 항상 실행 가능 (true)
            // _canExecute가 있으면 그 결과를 반환
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
            // 항상 실행 가능한 Command 생성
        }
        
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            // 조건부 실행 가능한 Command 생성
            _execute = execute ?? throw new ArgumentException(nameof(execute));
            _canExecute = canExecute;       // 이건 선택 사항 (null 가능)
        }

        public void Execute(object parameter)
        {
            // 명령 실행
            _execute(parameter);
        }

        // 실행 가능 상태가 바뀔 때 WPF에 알려 주는 이벤트
        // 이 부분이 자동 상태 관리의 핵심
        public event EventHandler CanExecuteChanged
        {
            add {
                // WPF의 Command Manager와 연동
                // Command Manager가 "상태가 바뀔 수 있는 상황"을 감지하면
                // 자동으로 CanExecute를 다시 호출해서 버튼 상태 업데이트
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
