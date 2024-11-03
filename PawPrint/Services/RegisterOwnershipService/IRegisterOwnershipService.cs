using PawPrint.Models;

namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<int> RegisterOwnershipAsync(MultipartFormDataContent formDataContent);
    Task<List<Dog>> GetRegisteredDogsByOwnerNICAsync(string nicNo);
}
