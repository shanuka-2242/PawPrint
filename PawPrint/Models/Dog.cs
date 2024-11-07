namespace PawPrint.Models;

public class Dog
{
    public int EntryID { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public string Age { get; set; }
    public string DogImage { get; set; }
    public ImageSource DogImageSource
    {
        get
        {
            if (string.IsNullOrEmpty(DogImage))
            {
                return null;
            }
            byte[] imageBytes = Convert.FromBase64String(DogImage);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}
