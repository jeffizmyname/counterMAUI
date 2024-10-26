using CommunityToolkit.Maui.Views;
using counter.Models;
using counter.Popups;
using counter.ViewModels;
using Google.Cloud.Firestore;
using MauiIcons.Core;

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

#if !ANDROID
            this.SizeChanged += MainPage_SizeChanged;
#endif
            FirestoreService services = new FirestoreService();
            db = services.db;

            _counterViewModel = new CounterViewModel(db);
            BindingContext = _counterViewModel;
        }

        private void MainPage_SizeChanged(object? sender, EventArgs e)
        {
            double pageWidth = this.Width;
            double minItemWidth = 500; 
            int numberOfColumns = (int)(pageWidth / minItemWidth);
            numberOfColumns = Math.Max(1, numberOfColumns);
            var gridItemsLayout = (GridItemsLayout)countersCollectionView.ItemsLayout;
            gridItemsLayout.Span = numberOfColumns;
            if(pageWidth < minItemWidth*2-1)
            {
                //this.WidthRequest = minItemWidth * 2 - 1;
            } 
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
