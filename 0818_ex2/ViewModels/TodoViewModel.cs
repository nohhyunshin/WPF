using _0818_ex2.Commands;
using _0818_ex2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace _0818_ex2.ViewModels
{
    class TodoViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<string> _todoItem = new ObservableCollection<string>();        

        private readonly TodoList _todolist = new TodoList();

        private string _todo;
        private string _selectedTodo;
        
        public string Todo
        {
            get { return _todo; } 
            set => SetProperty(ref _todo, value, () =>
            {
                CommandManager.InvalidateRequerySuggested();
            });
        }

        public ObservableCollection<string> TodoItems
        {
            get { return _todoItem; }
            set => SetProperty(ref _todoItem, value);
        }

        public string SelectedTodo
        {
            get { return _selectedTodo; }
            set => SetProperty(ref _selectedTodo, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DelCommand { get; }

        public TodoViewModel()
        {
            AddCommand = new RelayCommand(
                execute => Input()
            );

            DelCommand = new RelayCommand(
                execute: _ => Delete()
            );
        }


        private void Input()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Todo))
                {
                    TodoItems.Add(Todo);
                    Todo = "";
                }
            }
            catch (Exception ex)
            {
                TodoItems.Add($"오류 : {ex.Message}");
            }
        }
        private void Delete()
        {
            Todo = "";
            TodoItems.Remove(SelectedTodo);
        }
    }
}
