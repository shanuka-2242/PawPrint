using PawPrint.Models;

namespace PawPrint.Services.AuthenticateService;

public interface IAuthenticateService
{
    Task<Owner> GetOwnerByNICAsync(string nic);
}
