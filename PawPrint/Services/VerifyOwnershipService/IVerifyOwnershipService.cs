using PawPrint.Models;

namespace PawPrint.Services.VerifyOwnershipService;

public interface IVerifyOwnershipService
{
    Task<VerifiedInfomation> GetOwnerVerifiedInfoAsync(MultipartFormDataContent content);
}
