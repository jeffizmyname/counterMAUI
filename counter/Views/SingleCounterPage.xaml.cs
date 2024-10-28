using counter.Models;
using counter.ViewModels;
using Google.Cloud.Firestore;
using Microsoft.Maui.Controls;

namespace counter.Views;

public partial class SingleCounterPage : ContentPage
{
    private Counter currentCounter;
    private CounterViewModel _viewModel;
    Shell shell;
    public SingleCounterPage(Counter counter, CounterViewModel viewModel)
    {
        InitializeComponent();
        
        page.Title = counter.name;
        currentCounter = counter;
        _viewModel = viewModel;
        BindingContext = counter;
        if(Application.Current?.MainPage != null)
        {
            shell = Application.Current.MainPage as Shell ?? throw new Exception("Error sth went wrong :<");
            Shell.SetBackgroundColor(shell, currentCounter.coutnerColor);
            Shell.SetTitleColor(shell, currentCounter.secondaryColor);
        } else
        {
            throw new Exception("Error sth went wrong :<");
        }

    }

    private void IncrementClicked(object sender, TappedEventArgs e)
    {
        _viewModel.IncrementCommand.Execute(currentCounter);
    }

    private void DecrementClicked(object sender, TappedEventArgs e)
    {
        _viewModel.DecrementCommand.Execute(currentCounter);

    }

    private void ResetClicked(object sender, TappedEventArgs e)
    {
        _viewModel.ResetCommand.Execute(currentCounter);

    }

    protected override bool OnBackButtonPressed()
    {
        if (Application.Current != null) {
            Shell.SetBackgroundColor(shell, (Color)Application.Current.Resources["Gray950"]);
            Shell.SetTitleColor(shell, (Color)Application.Current.Resources["Primary"]);
        }
        else
        {
            throw new Exception("Error sth went wrong :<");
        }

        return false; 
    }
}