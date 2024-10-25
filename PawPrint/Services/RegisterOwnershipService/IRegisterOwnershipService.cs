using PawPrint.Models;

namespace PawPrint.Services.RegisterOwnershipService;

public interface IRegisterOwnershipService
{
    Task<bool> RegisterOwnership(RegisterInformation registerInformation);
}
