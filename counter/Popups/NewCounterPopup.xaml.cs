using CommunityToolkit.Maui.Views;
using counter.ViewModels;
using Google.Cloud.Firestore;
using System.Diagnostics;

namespace counter.Popups;

public partial class NewCounterPopup : Popup
{
    Color selectedColor = Color.FromRgb(0, 0, 0);
    private CounterViewModel _viewModel; 
    FirestoreDb db;
    public NewCounterPopup(CounterViewModel viewModel, FirestoreDb databaseRef)
    {
        InitializeComponent();
        _viewModel = viewModel;
        db = databaseRef;

        if(DeviceInfo.Platform == DevicePlatform.Android)
        {
            GridColor.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6, GridUnitType.Star)});
            GridColor.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
        } else
        {
            GridColor.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            GridColor.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

        }
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        string counterName = CounterNameEntry.Text;
        int baseValue;

        if (string.IsNullOrWhiteSpace(counterName))
        {
            counterName = "defaultCounterName";
        }

        if (string.IsNullOrWhiteSpace(StartCountervalue.Text))
        {
            baseValue = 1;
        }
        else
        {
            baseValue = int.TryParse(StartCountervalue.Text, out int parsedValue) ? parsedValue : 1;
        }

        _viewModel.AddCounter(counterName, baseValue, selectedColor);
        
        Close();
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        int red = (int)RedSlider.Value;
        int green = (int)GreenSlider.Value;
        int blue = (int)BlueSlider.Value;

        selectedColor = Color.FromRgb(red, green, blue);

        SelectedColorBox.Color = selectedColor;
        //SelectedColorLabel.Text = $"Selected Color: RGB({red}, {green}, {blue})";
    }

    private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue) && !int.TryParse(e.NewTextValue, out _))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }
    }
}