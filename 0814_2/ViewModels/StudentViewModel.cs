using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using _0814_2.Models;

namespace _0814_2.ViewModels
{
    class StudentViewModel
    {
        public ObservableCollection<Student> Students { get; set; }
        public string NewStudentName { get; set; }
        public ICommand AddStudentCommand { get; set; }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>();
            AddStudentCommand = new RelayCommand(AddStudent);
        }

        private void AddStudent()
        {
            if (!string.IsNullOrEmpty(NewStudentName))
            {
                Students.Add(new Student { Name = NewStudentName });
                NewStudentName = "";
            }
        }
    }
}
