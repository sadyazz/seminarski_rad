using Firebase.Storage;

public class FirebaseService
{
    private readonly string _bucketName = "bucket-5c7b9.appspot.com";

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile.Length == 0) return null;

        var stream = imageFile.OpenReadStream();
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        var task = new FirebaseStorage(_bucketName)
            .Child("images")
            .Child(fileName)
            .PutAsync(stream);

        task.Progress.ProgressChanged += (s, e) =>
            Console.WriteLine($"Progress: {e.Percentage} %");

        var downloadUrl = await task;

        return downloadUrl; 
    }
}