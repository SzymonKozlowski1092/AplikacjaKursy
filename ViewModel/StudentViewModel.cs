using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dziekanat.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Dziekanat.ViewModel
{
    public class StudentViewModel : ObservableObject
    {
        private readonly CourseManager _courseManager;
        private readonly StudentManager _studentManager;

        private ObservableCollection<Course> _availableCourses;
        private ObservableCollection<Course> _enrolledCourses;

        private Course? _selectedCourse;
        public string UserId { get; }
        public ObservableCollection<Course> AvailableCourses
        {
            get => _availableCourses;
            set => SetProperty(ref _availableCourses, value);
        }
        public ObservableCollection<Course> EnrolledCourses
        {
            get => _enrolledCourses;
            set => SetProperty(ref _enrolledCourses, value);
        }
        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set => SetProperty(ref _selectedCourse, value);
        }

        public StudentViewModel(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            _courseManager = new CourseManager();
            _studentManager = new StudentManager();

            AvailableCourses = new ObservableCollection<Course>();
            EnrolledCourses = new ObservableCollection<Course>();

            LoadCourses();
        }

        public ICommand EnrollStudentCommand => new AsyncRelayCommand(EnrollStudentAsync);

        private async Task EnrollStudentAsync()
        {
            try
            {
                if (SelectedCourse is null)
                {
                    throw new InvalidOperationException("Nie wybrano kursu.");
                }

                var success = await _courseManager.EnrollStudent(UserId, SelectedCourse.Id.ToString());
                if (success)
                {
                    EnrolledCourses.Add(SelectedCourse);
                    AvailableCourses.Remove(SelectedCourse);
                }
                else
                {
                    throw new InvalidOperationException("Nie udało się zapisać na kurs.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Wystąpił błąd podczas zapisywania na kurs: {ex.Message}", "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void LoadCourses()
        {
            try
            {
                var enrolledCoursesList = await _studentManager.GetStudentCourses(UserId);
                EnrolledCourses = new ObservableCollection<Course>(enrolledCoursesList);

                var allCourses = await _courseManager.GetCourses();
                var availableCoursesList = allCourses
                    .Where(course => !enrolledCoursesList.Any(enrolled => enrolled.Id == course.Id))
                    .ToList();

                AvailableCourses = new ObservableCollection<Course>(availableCoursesList);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Wystąpił błąd podczas ładowania kursów: {ex.Message}", "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
