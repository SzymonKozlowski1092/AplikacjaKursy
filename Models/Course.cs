using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziekanat.Models
{
    public class Course : ObservableObject
    {
        string _name;
        string _description;
        public int? Id { get; set; }
        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public string? LecturerName {  get; set; }
        public float? Grade { get; set; }
    }
}
