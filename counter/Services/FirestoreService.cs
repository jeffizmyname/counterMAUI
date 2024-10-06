#if ANDROID
using Android.Content;
using Android.Runtime;
#endif
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;
using System.Reflection;
using FirebaseAdmin;
using System.IO;
public partial class FirestoreService
{
    public FirestoreDb db;

    public FirestoreService()
    {
#if ANDROID
        InitializeFirestoreAndroid();
#else
        InitializeFirestoreForWindows();
#endif
    }

    private async void InitializeFirestoreAndroid()
    {
        var path = await FileSystem.OpenAppPackageFileAsync("mauicounter-firebase-adminsdk-z6cbb-d210d3dd25.json");
        var reader = new StreamReader(path);
        var contents = reader.ReadToEnd();

        FirestoreClientBuilder fbc = new FirestoreClientBuilder { JsonCredentials = contents };
        db = FirestoreDb.Create("mauicounter", fbc.Build());
    }

    private void InitializeFirestoreForWindows()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mauicounter-firebase-adminsdk-z6cbb-d210d3dd25.json");
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
        db = FirestoreDb.Create("mauicounter");
    }

}
