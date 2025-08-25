using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0814_3.Models
{
    class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Score { get; set; }

        public string Grade
        {
            get
            {
                if (Score >= 90) return "A";
                else if (Score >= 80) return "B";
                else if (Score >= 70) return "C";
                else if (Score >= 60) return "D";
                else return "F";
            }
        }

        public bool IsPassed
        {
            get
            {
                return Score >= 70;
            }
        }
    }
}
