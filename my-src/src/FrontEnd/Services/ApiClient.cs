using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ConferenceDTO;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddAttendeeAsync(Attendee attendee)
        {
            using (var response = await _httpClient.PostAsJsonAsync($"/api/attendees", attendee))
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return false;
                }

                response.EnsureSuccessStatusCode();
            }

            return true;
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            using (var response = await _httpClient.GetAsync($"/api/attendees/{name}"))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<AttendeeResponse>();
            }
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            using (var response = await _httpClient.GetAsync($"/api/sessions/{id}"))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<SessionResponse>();
            }
        }

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {
            using (var response = await _httpClient.GetAsync("/api/sessions"))
            {
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<List<SessionResponse>>();
            }
        }

        public async Task DeleteSessionAsync(int id)
        {
            using (var response = await _httpClient.DeleteAsync($"/api/sessions/{id}"))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            using (var response = await _httpClient.GetAsync($"/api/speakers/{id}"))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<SpeakerResponse>();
            }
        }

        public async Task<List<SpeakerResponse>> GetSpeakersAsync()
        {
            using (var response = await _httpClient.GetAsync("/api/speakers"))
            {
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<List<SpeakerResponse>>();
            }
        }

        public async Task PutSessionAsync(Session session)
        {
            using (var response = await _httpClient.PutAsJsonAsync($"/api/sessions/{session.Id}", session))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}