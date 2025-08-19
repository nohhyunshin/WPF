using _0818_ex2.Commands;
using _0818_ex2.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace _0818_ex2.ViewModels
{
    class TodoViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<TodoList> _todoItem = new ObservableCollection<TodoList>();

        private readonly TodoList _todolist = new TodoList();

        private string _todo;
        private TodoList _selectedTodo;
        private string _selectedImg;
        private bool _isPopupOpen;
        

        public string Todo
        {
            get { return _todo; } 
            set => SetProperty(ref _todo, value, () =>
            {
                CommandManager.InvalidateRequerySuggested();
            });
        }

        public ObservableCollection<TodoList> TodoItems
        {
            get { return _todoItem; }
            set => SetProperty(ref _todoItem, value);
        }

        public TodoList SelectedTodo
        {
            get { return _selectedTodo; }
            set
            {
                if (SetProperty(ref _selectedTodo, value))
                {
                    if(SelectedTodo != null && SelectedTodo.ImagePath != null)
                    {
                        IsPopupOpen = true; 
                    }
                }
            }
        }

        public string SelectedImg
        {
            get => _selectedImg;
            set {
                _selectedImg = value;
                OnPropertyChanged(nameof(SelectedImg)); 
            }
        }

        // 이미지 팝업창
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        public ICommand AddCommand { get; }
        public ICommand AddImgCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DelCommand { get; }

        public TodoViewModel()
        {
            AddCommand = new RelayCommand(
                execute => Input()
            );

            AddImgCommand = new RelayCommand(
                execute => ImgInput()
            );

            CloseCommand = new RelayCommand(
                execute => IsPopupOpen = false
            );

            DelCommand = new RelayCommand(
                execute: _ => Delete()
            );
        }


        private void Input()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Todo) || !string.IsNullOrWhiteSpace(SelectedImg))
                {
                    TodoItems.Add(new TodoList
                    {
                        Text = Todo,
                        ImagePath = SelectedImg
                    });
                    Todo = "";
                    SelectedImg = "";
                }
            }
            catch (Exception ex)
            {
                TodoItems.Add(new TodoList { Text = $"오류 : {ex.Message}" });
            }
        }

        private void ImgInput()
        {
            OpenFileDialog imgDialog = new OpenFileDialog();
            imgDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";

            // dialog 박스 열기
            Nullable<bool> result = imgDialog.ShowDialog();

            if (result == true)
            {
                // 단순 경로 저장
                SelectedImg = imgDialog.FileName;
            }
        }

        private void Delete()
        {
            Todo = "";
            TodoItems.Remove(SelectedTodo);
        }
    }
}
