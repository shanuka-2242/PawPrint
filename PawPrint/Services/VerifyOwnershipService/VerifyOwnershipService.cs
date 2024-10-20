using PawPrint.Models;
using System.Net.Http.Json;

namespace PawPrint.Services.VerifyOwnershipService;

public class VerifyOwnershipService : IVerifyOwnershipService
{
    private HttpClient _httpClient { get; }

    public VerifyOwnershipService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://10.0.2.2:8000/");
        _httpClient = httpClient;
    }
    public async Task<VerifiedInfomation> GetOwnerVerifiedInfoAsync(MultipartFormDataContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync("verify-ownership", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<VerifiedInfomation>();
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
