namespace PawPrint.Models;

public class RegisterInformation
{
    public MultipartFormDataContent NoseImage { get; set; }
    public MultipartFormDataContent DogImage { get; set; }
    public string DogName { get; set; }
    public string Breed { get; set; }
    public string Age { get; set; }
    public string NIC { get; set; }
}
