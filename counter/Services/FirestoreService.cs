#if ANDROID
using Android.Content;
using Android.Runtime;
#endif
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
public partial class FirestoreService
{
    public FirestoreDb? db;

    public FirestoreService() =>
#if ANDROID
        InitializeFirestoreAndroid();
#else
        InitializeFirestoreForWindows();
#endif


    private async void InitializeFirestoreAndroid()
    {
        var path = await FileSystem.OpenAppPackageFileAsync("mauicounter-firebase.json");
        var reader = new StreamReader(path);
        var contents = reader.ReadToEnd();

        FirestoreClientBuilder fbc = new FirestoreClientBuilder { JsonCredentials = contents };
        db = FirestoreDb.Create("mauicounter", fbc.Build());
    }

    private void InitializeFirestoreForWindows()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mauicounter-firebase.json");
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
        db = FirestoreDb.Create("mauicounter");
    }

}
