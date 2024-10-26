using PawPrint.Models;

namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<bool> RegisterOwnershipAsync(MultipartFormDataContent formDataContent);
    Task<List<Dog>> GetRegisteredDogsByOwnerNICAsync(string nicNo);
}
