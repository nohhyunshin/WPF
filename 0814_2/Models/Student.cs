using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

// 비즈니스 로직
// 프로그램이 수행해야 하는 핵심 규칙, 절차, 계산 조건 등

namespace _0814_2.Models
{
    class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Score { get; set; }
        public string Grade => CalculateGrade(); // 비즈니스 로직

        private string CalculateGrade()
        {
            if (Score >= 90) return "A";
            else if (Score >= 90) return "B";
            else if (Score >= 80) return "C";
            else if (Score >= 70) return "D";
            else return "F";
        }
    }
}
