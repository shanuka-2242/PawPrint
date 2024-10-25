namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<bool> RegisterOwnership(MultipartFormDataContent formDataContent);
}
