using CommunityToolkit.Maui.Views;
using counter.ViewModels;
using Google.Cloud.Firestore;
using System.Diagnostics;

namespace counter;

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
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        string counterName = CounterNameEntry.Text;
        int baseValue = int.Parse(StartCountervalue.Text);

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
        SelectedColorLabel.Text = $"Selected Color: RGB({red}, {green}, {blue})";
    }

    private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue) && !int.TryParse(e.NewTextValue, out _))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }
    }
}