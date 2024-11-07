namespace PawPrint.Services.RegisteredDogListService;

public interface IRegisteredDogListService
{
    Task<bool> RemoveRegisteredDogAsync(int entryId);
}
