using CommunityToolkit.Mvvm.ComponentModel;

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
