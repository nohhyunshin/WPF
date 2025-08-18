using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _0814_ex2.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // 속성이 변경될 때 발생하는 이벤트
        // WPF 바인딩 엔진이 이 이벤트를 구독하여 UI를 자동으로 업데이트
        public event PropertyChangedEventHandler? PropertyChanged;

        // 속성 변경 알림을 보내는 기본 메소드
        // 변경된 속성의 이름이 자동으로 채워짐
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // PropertyChanged 이벤트가 구독되어 있으면 발생시킴
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 속성 값을 설정하고 필요 시 변경 알림을 보내는 헬퍼 메소드
        // BasicViewModel의 핵심 기능!
        // 변경된 속성의 이름이 자동으로 채워짐
        // T : 속성의 타입
        // field : 값을 저장할 private 필드 (ref로 전달)
        // value : 새로 설정할 값 
        // return 값 : 값이 실제로 변경되었으면 True, 아니면 False
        protected virtual bool SetProperty<T> (ref T field, T value, [CallerMemberName] string propertyName = null)
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

        // 속성 값을 설정하고 추가 작업을 수행하는 헬퍼 메소드
        // 값이 변경되었을 때에만 추가 작업이 실행됨
        // T : 속성의 타입
        // field : 값을 저장할 private 필드 (ref로 전달)
        // value : 새로 설정할 값 
        // return 값 : 값이 실제로 변경되었으면 True, 아니면 False
        // onChanged : 값이 변경되었을 때 실행할 추가 작업
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
