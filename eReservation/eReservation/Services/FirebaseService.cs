using FirebaseAdmin;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FirebaseService
{
    private readonly string _bucketName = "images-5f6f1.appspot.com";  // Firebase Storage Bucket name
    private readonly string _pathToServiceAccountKey = "C:\\Users\\Krhan\\Desktop\\seminarski_rad\\images-5f6f1.json"; // Path to your Firebase service account key file

    // Initialize Firebase Admin SDK
    public FirebaseService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(_pathToServiceAccountKey)
            });
        }
    }

    // Upload image to Firebase Storage
    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile.Length == 0) return null;

        var stream = imageFile.OpenReadStream();
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        // Create an instance of FirebaseStorage
        var firebaseStorage = new FirebaseStorage(_bucketName);

        // Get reference to Firebase Storage
        var storageReference = firebaseStorage.Child("images").Child(fileName);

        // Upload the file to Firebase Storage
        await storageReference.PutAsync(stream);

        // Get the download URL of the uploaded file
        var downloadUrl = await storageReference.GetDownloadUrlAsync();

        return downloadUrl;  // Return the URL of the uploaded image
    }
}
