using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _0818_ex2.Commands
{
    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;

        private readonly Func<object, bool> _canExecute;

        public bool CanExecute(Object parameter)
        {
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
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
