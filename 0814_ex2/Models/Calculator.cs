using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace _0814_ex2.Models
{
    // 계산 기능을 담당하는 Model 클래스
    // UI와 완전히 독립적인 순수한 계산 로직만 포함

    class Calculator
    {
        // region 기본 수학 연산

        // 덧셈
        public double Add(double x, double y)
        {
            return x + y;
        }

        // 뺄셈
        public double Sub(double x, double y)
        {
            return x - y;
        }

        // 곱셈
        public double Mul(double x, double y)
        {
            return x * y;
        }

        // 나눗셈
        public double Div(double x, double y)
        {
            if (y == 0)
            {
                throw new DivideByZeroException("0으로 나눌 수 없습니다.");
            }
            return x / y;
        }

        // 문자열이 유효한지 확인
        // 유효하면 True, 유효하지 않으면 False
        public bool IsValidNumber(string input)
        {
            // double.TryParse는 반환 성공 시 True, 실패 시 False 반환
            return double.TryParse(input, out var _);
        }

        // 문자열을 안전하게 숫자로 변환
        // 변환된 숫자 (반환 실패 시 0)
        public double ParseNumber(string input)
        {
            if (double.TryParse(input, out var result))
            {
                return result;
            }
            return 0;
        }

        public double Calculate(double x, double y, string operation)
        {
            return operation switch
            {
                "+" => Add(x, y),
                "-" => Sub(x, y),
                "*" => Mul(x, y),
                "/" => Div(x, y),
                _ => throw new AggregateException($"지원하지 않는 연산자입니다.")
            };
        }
    }
}
