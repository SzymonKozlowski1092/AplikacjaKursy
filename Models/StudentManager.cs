using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dziekanat.Models
{
    public class StudentManager
    {
        private readonly RestClient _restClient;

        public StudentManager()
        {
            _restClient = new RestClient("https://localhost:7203");
            _restClient.AddDefaultHeader("Authorization", $"Bearer {AuthToken.Instance.JwtToken}");
        }

        public async Task<List<Course>> GetStudentCourses(string studentIndex)
        {
            if (string.IsNullOrWhiteSpace(studentIndex))
                throw new ArgumentException("Student index cannot be null or empty", nameof(studentIndex));

            try
            {
                var response = await _restClient.GetAsync(new RestRequest($"Student/{studentIndex}/Courses"));

                if (response != null && response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
                {
                    return JsonConvert.DeserializeObject<List<Course>>(response.Content)
                           ?? throw new Exception("Wystąpił błąd przy deserializacji danych");
                }

                throw new Exception("Nie udało się pobrać kursów studenta");
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd podczas pobierania kursów dla studenta: {ex.Message}", ex);
            }
        }
    }
}
