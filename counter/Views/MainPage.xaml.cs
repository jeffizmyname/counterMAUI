using CommunityToolkit.Maui.Views;
using counter.Models;
using counter.ViewModels;
using Google.Cloud.Firestore;
using System.Collections.ObjectModel;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace counter.Views
{
    public partial class MainPage : ContentPage
    {
        FirestoreDb db;
        private CounterViewModel _counterViewModel;
        public MainPage()
        {
            InitializeComponent();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"mauicounter-firebase-adminsdk-z6cbb-d210d3dd25.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("mauicounter");

            _counterViewModel = new CounterViewModel(db);
            BindingContext = _counterViewModel;
        }

        private void NewCounterBtn_Clicked(object sender, EventArgs e)
        {
            var popup = new NewCounterPopup(_counterViewModel, db);
            this.ShowPopup(popup);
        }
    }

}
