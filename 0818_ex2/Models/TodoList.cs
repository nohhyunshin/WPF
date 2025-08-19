using _0818_ex2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _0818_ex2.Models
{
    class TodoList : BaseViewModel
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }

        public bool _isDone;
        private TextDecorationCollection _textdecoration;

        // 체크박스 체크 여부
        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (SetProperty(ref _isDone, value))
                {
                    TextDecoration = _isDone ? new TextDecorationCollection(TextDecorations.Strikethrough) : null;
                }
            }
        }
        public TextDecorationCollection TextDecoration
        {
            get => _textdecoration;
            set => SetProperty(ref _textdecoration, value);
        }


        public string Add(string str)
        {
            return str;
        }

        public string Input(string str)
        {
            return str;
        }
    }
}
