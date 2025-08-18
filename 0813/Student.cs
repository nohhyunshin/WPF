using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0813
{
    class Student
    {
        // 속성 (Property) 정의
        public string Name { get; set; }
        public int Age { get; set; }
        public double Score { get; set; }
        public string Grade { get; set; }

        // 생성자 (Constructor) - 객체 생성할 때 실행
        public Student(string name, int age, double score)
        {
            Name = name;
            Age = age;
            Score = score;
            Grade = CalculateGrade(score);
        }

        private string CalculateGrade(double score)
        {
            if (Score >= 90) return "A";
            else if (Score >= 80) return "B";
            else if (Score >= 70) return "C";
            else if (Score >= 60) return "D";
            else  return "F";

        }
    }
}
