using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziekanat.Models
{
    public class Student : ObservableObject
    {
        double? _grade;
        public int? Index { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public double? Grade
        {
            get => _grade; set => SetProperty(ref _grade, value);
        }
    }
}
