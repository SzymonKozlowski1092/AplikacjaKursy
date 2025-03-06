using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Dziekanat.Models
{
    public class CourseManager
    {
        private readonly RestClient _restClient;

        public CourseManager()
        {
            _restClient = new RestClient("https://localhost:7203");
            _restClient.AddDefaultHeader("Authorization", $"Bearer {AuthToken.Instance.JwtToken}");
        }

        public async Task<List<Course>> GetCourses()
        {
            try
            {
                var response = await _restClient.GetAsync(new RestRequest("/Course"));

                if (response is not null && response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
                {
                    return JsonConvert.DeserializeObject<List<Course>>(response.Content)
                           ?? throw new Exception("Failed to deserialize course data");
                }

                throw new Exception("Failed to retrieve courses");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Course>();
            }
        }

        public async Task<bool> EnrollStudent(string studentIndex, string courseId)
        {
            if (string.IsNullOrWhiteSpace(studentIndex) || string.IsNullOrWhiteSpace(courseId))
            {
                MessageBox.Show("Invalid student index or course ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var response = await _restClient.PostAsync(new RestRequest($"Course/{courseId}/student/{studentIndex}"));
                return response.IsSuccessful;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<List<Student>> GetCourseStudents(string courseId)
        {
            try
            {
                var request = new RestRequest($"/Course/{courseId}/students", Method.Get);
                //request.AddHeader("Authorization", $"Bearer {AuthToken.Instance.JwtToken}");
                var response = await _restClient.ExecuteAsync(request);

                if (response is not null && response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
                {
                    return JsonConvert.DeserializeObject<List<Student>>(response.Content)
                           ?? throw new Exception("Failed to deserialize student data");
                }

                throw new Exception("Failed to retrieve course students");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Student>();
            }
        }

        public async Task<bool> AddCourse(Course course, string lecturerId)
        {
            if (course == null || string.IsNullOrWhiteSpace(lecturerId))
            {
                MessageBox.Show("Invalid course data or lecturer ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var request = new RestRequest("/Course", Method.Post).AddJsonBody(new
                {
                    course.Name,
                    course.Description,
                    LecturerId = lecturerId
                });

                var response = await _restClient.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> UpdateCourse(Course course, string id)
        {
            if (course == null || string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Invalid course data or ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var request = new RestRequest($"Course/{id}", Method.Put).AddJsonBody(new
                {
                    course.Name,
                    course.Description,
                    course.LecturerName
                });

                var response = await _restClient.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> UpdateGrade(string courseId, string studentIndex, string grade)
        {
            if (string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(studentIndex) || string.IsNullOrWhiteSpace(grade))
            {
                MessageBox.Show("Invalid course ID, student index, or grade", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var request = new RestRequest($"Course/{courseId}/Student/{studentIndex}/ZmienOcene/{grade}", Method.Put);
                var response = await _restClient.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
