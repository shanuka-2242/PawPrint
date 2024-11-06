namespace PawPrint.Services.RegisteredDogListService;

public interface IRegisteredDogListService
{
    Task<List<ImageSource>> GetRegisteredDogImagesByNICAsync(string nic);
    Task<bool> RemoveRegisteredDogAsync(int entryId);
}
