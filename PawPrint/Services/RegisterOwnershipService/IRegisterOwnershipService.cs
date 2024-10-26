namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<bool> RegisterOwnershipAsync(MultipartFormDataContent formDataContent);
}
