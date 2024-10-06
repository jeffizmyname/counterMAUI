using CommunityToolkit.Maui.Views;
using counter.Models;
using counter.Popups;
using counter.ViewModels;
using Google.Cloud.Firestore;
using MauiIcons.Core;
using System.Diagnostics;

namespace counter.Views
{
    public partial class MainPage : ContentPage
    {
        private FirestoreDb db;
        private CounterViewModel _counterViewModel;

        public MainPage()
        {
            InitializeComponent();
            _ = new MauiIcon();

            var shell = Application.Current.MainPage as Shell;

            FirestoreService services = new FirestoreService();
            db = services.db;

            _counterViewModel = new CounterViewModel(db);
            BindingContext = _counterViewModel;
        }

        private void NewCounterBtn_Clicked(object sender, EventArgs e)
        {
            var popup = new NewCounterPopup(_counterViewModel, db);
            this.ShowPopup(popup);
        }

        private async void CounterClicked(object sender, EventArgs e)
        {
            var parameter = ((TappedEventArgs)e).Parameter;
            Counter? tappedCounter = parameter as Counter;

            if (tappedCounter != null)
            {
                await Navigation.PushAsync(new SingleCounterPage(tappedCounter, _counterViewModel));
            }
        }
    }
}
