using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dziekanat.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Dziekanat.ViewModel
{
    public class LecturerViewModel : ObservableObject
    {
        private readonly LecturerManager _lecturerManager;
        private readonly CourseManager _courseManager;

        private ObservableCollection<Course> _lecturerCourses;
        private ObservableCollection<Student>? _selectedCourseStudents;
        private Course? _selectedCourse;
        private string _selectedCourseName;
        private string _selectedCourseDescription;

        public string SelectedCourseName
        {
            get => _selectedCourseName;
            set => SetProperty(ref _selectedCourseName, value);
        }
        public string SelectedCourseDescription
        {
            get => _selectedCourseDescription;
            set => SetProperty(ref _selectedCourseDescription, value);
        }
        public string UserId { get; }
        public ObservableCollection<Course> LecturerCourses
        {
            get => _lecturerCourses;
            set => SetProperty(ref _lecturerCourses, value);
        }
        public ObservableCollection<Student>? SelectedCourseStudents
        {
            get => _selectedCourseStudents;
            set => SetProperty(ref _selectedCourseStudents, value);
        }
        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set => SetProperty(ref _selectedCourse, value);
        }
        public Student? SelectedStudent { get; set; }

        public LecturerViewModel(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            _lecturerManager = new LecturerManager();
            _courseManager = new CourseManager();
            LoadLecturerCourses();
        }

        public async void LoadLecturerCourses()
        {
            try
            {
                var lecturerCourses = await _lecturerManager.GetLecturerCourses(UserId);
                LecturerCourses = new ObservableCollection<Course>(lecturerCourses);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas ładowania kursów: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand EditCourseCommand => new RelayCommand(async () =>
        {
            if (SelectedCourse == null) return;

            try
            {
                SelectedCourseName = SelectedCourse.Name;
                SelectedCourseDescription = SelectedCourse.Description;

                var courseStudents = await _courseManager.GetCourseStudents(SelectedCourse.Id.ToString());
                if(courseStudents is not null)
                {
                    SelectedCourseStudents = new ObservableCollection<Student>(courseStudents);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas edytowania kursu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        public ICommand AddNewCourseCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(SelectedCourseName))
            {
                MessageBox.Show("Nazwa kursu nie może być pusta.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Czy chcesz stworzyć kurs o nazwie: {SelectedCourseName}?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var newCourse = new Course
                {
                    Name = SelectedCourseName,
                    Description = SelectedCourseDescription,
                };

                if (await _courseManager.AddCourse(newCourse, UserId))
                {
                    LecturerCourses.Add(newCourse);
                    MessageBox.Show("Dodawanie powiodło się.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kursu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadLecturerCourses();
        });

        public ICommand SaveCourseChangesCommand => new RelayCommand(async () =>
        {
            if (SelectedCourse == null)
            {
                MessageBox.Show("Żaden kurs nie został wybrany.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Czy chcesz zmienić dane kursu?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var course = new Course
                {
                    Name = SelectedCourseName,
                    Description = SelectedCourseDescription,
                    LecturerName = SelectedCourse.LecturerName
                };

                if (await _courseManager.UpdateCourse(course, SelectedCourse.Id.ToString()))
                {
                    SelectedCourse.Name = course.Name;
                    SelectedCourse.Description = course.Description;
                    MessageBox.Show("Zmiany zostały zapisane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania zmian: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        public ICommand SaveGradeCommand => new RelayCommand(async () =>
        {
            if (SelectedStudent == null || SelectedCourse == null)
            {
                MessageBox.Show("Brak wybranego studenta lub kursu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var isUpdated = await _courseManager.UpdateGrade(SelectedCourse.Id.ToString(), SelectedStudent.Index.ToString(), SelectedStudent.Grade.ToString());
                if (isUpdated)
                {
                    MessageBox.Show("Ocena została zapisana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nie udało się zapisać oceny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania oceny: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
    }
}
