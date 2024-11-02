using PawPrint.Models;

namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<int> RegisterOwnershipAsync(MultipartFormDataContent formDataContent);
    Task<IEnumerable<Dog>> GetRegisteredDogsByOwnerNICAsync(string nicNo);
}
