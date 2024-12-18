﻿using PawPrint.Models;
using System.Net.Http.Json;

namespace PawPrint.Services.RegisterOwnershipService;

public class RegisterOwnershipService : IRegisterOwnershipService
{
    private HttpClient _httpClient { get; }

    public RegisterOwnershipService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://10.0.2.2:8000/");
        _httpClient = httpClient;
    }

    public async Task<int> RegisterOwnershipAsync(MultipartFormDataContent formDataContent)
    {
        try
        {
            var response = await _httpClient.PostAsync("register_dog", formDataContent);
            if (response.IsSuccessStatusCode)
            {
                return (int)response.StatusCode;
            }
            return (int)response.StatusCode;
        }
        catch (Exception)
        {
            return 500;
        }
    }

    public async Task<List<Dog>> GetRegisteredDogsByOwnerNICAsync(string ownerNIC)
    {
        try
        {
            var response = await _httpClient.GetAsync($"registered_dog/{ownerNIC}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dog>>();
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
