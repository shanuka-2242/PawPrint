﻿namespace PawPrint.Services.RegisterOwnershipService;

public class RegisterOwnershipService : IRegisterOwnershipService
{
    private HttpClient _httpClient { get; }

    public RegisterOwnershipService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://10.0.2.2:8000/");
        _httpClient = httpClient;
    }

    public async Task<bool> RegisterOwnership(MultipartFormDataContent formDataContent)
    {
        try
        {
            var response = await _httpClient.PostAsync("register_dog", formDataContent);
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