using System.Text.Json;

namespace PawPrint.Services.RegisteredDogListService;

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

    public async Task<List<ImageSource>> GetRegisteredDogImagesByNICAsync(string nic)
    {
        try
        {
            var images = new List<ImageSource>();
            var response = await _httpClient.GetStringAsync($"registered_dog_images/{nic}");
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }
            else
            {
                var imagesBase64 = JsonSerializer.Deserialize<List<string>>(response);
                foreach (var imageBase64 in imagesBase64)
                {
                    byte[] imageBytes = Convert.FromBase64String(imageBase64);
                    images.Add(ImageSource.FromStream(() => new MemoryStream(imageBytes)));
                }
                return images;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}
