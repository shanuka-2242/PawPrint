using PawPrint.Models;
using System.Net.Http.Json;

namespace PawPrint.Services.AuthenticateService;

public class AuthenticateService : IAuthenticateService
{
    private HttpClient _httpClient { get; }

    public AuthenticateService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://192.168.1.8:8000/");
        _httpClient = httpClient;
    }

    public async Task<Owner> GetOwnerByNICAsync(string nic)
    {
        try
        {
            var response = await _httpClient.GetAsync($"registered_owners/{nic}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Owner>();
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    async public Task<bool> SignUpOwnerAsync(MultipartFormDataContent multipartFormData)
    {
        try
        {
            var response = await _httpClient.PostAsync("register_owner", multipartFormData);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
