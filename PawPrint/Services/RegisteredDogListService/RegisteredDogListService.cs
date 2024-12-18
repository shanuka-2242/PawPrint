﻿namespace PawPrint.Services.RegisteredDogListService;

public class RegisteredDogListService : IRegisteredDogListService
{
    private HttpClient _httpClient { get; }

    public RegisteredDogListService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://10.0.2.2:8000/");
        _httpClient = httpClient;
    }

    public async Task<bool> RemoveRegisteredDogAsync(int entryId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"registered_dog/{entryId}");
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
