using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dziekanat.Models
{
    public class LecturerManager
    {
        private readonly RestClient _restClient;

        public LecturerManager()
        {
            _restClient = new RestClient("https://localhost:7203");
            _restClient.AddDefaultHeader("Authorization", $"Bearer {AuthToken.Instance.JwtToken}");
        }

        public async Task<List<Course>> GetLecturerCourses(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be null or empty", nameof(employeeId));

            try
            {
                var response = await _restClient.GetAsync(new RestRequest($"Lecturer/{employeeId}/courses"));

                if (response != null && response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
                {
                    return JsonConvert.DeserializeObject<List<Course>>(response.Content)
                           ?? throw new Exception("Failed to deserialize course data");
                }

                throw new Exception("Failed to retrieve lecturer courses");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching lecturer courses: {ex.Message}", ex);
            }
        }
    }
}
