using Firebase.Storage;

public class FirebaseService
{
    private readonly string _bucketName = "bucket-5c7b9.appspot.com"; // Replace with your bucket name

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile.Length == 0) return null;

        var stream = imageFile.OpenReadStream();
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        var task = new FirebaseStorage(_bucketName)
            .Child("images") // You can specify your folder structure here
            .Child(fileName)
            .PutAsync(stream);

        // Track progress of the upload
        task.Progress.ProgressChanged += (s, e) =>
            Console.WriteLine($"Progress: {e.Percentage} %");

        // Await the task to get the download URL after upload completes
        var downloadUrl = await task;

        return downloadUrl; // This will return the URL of the uploaded image
    }
}
