using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _0818_ex2.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // PropertyChanged 이벤트가 구독되어 있으면 발생시킴
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 1단계 : 값이 실제로 바뀌었는지 확인 (성능 최적화)
            if (Equals(field, value))
            {
                // 같은 값이면 아무 것도 안 하겠다
                return false;
            }

            // 2단계 : 새 값으로 설정
            field = value;

            // 3단계 : 변경 알림 발생
            OnPropertyChanged(propertyName);

            // 4단계 : 변경되었음을 알려 줌
            return true;
        }

        protected virtual bool SetProperty<T>(ref T field, T value, System.Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref field, value, propertyName))
            {
                onChanged?.Invoke();
                return true;
            }
            return false;
        }
    }
}
