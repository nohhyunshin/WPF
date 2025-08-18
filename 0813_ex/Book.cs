using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0813_ex
{
    internal class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Grade { get; set; }

        public Book(string title, string author, int price, string category)
        {
            Title = title;
            Author = author;
            Price = price;
            Category = category;
            Grade = CalGrade(price);
        }

        private string CalGrade(int price)
        {
            if (price >= 10000) return "고급";
            else if (price >= 5000) return "중급";
            else return "저급";
        }
    }
}
