using CommunityToolkit.Maui.Views;

namespace counter.Popups;

public partial class ConfirmationPopup : Popup
{

    public event Action<bool>? OnResult;
    public ConfirmationPopup(string Question)
	{
		InitializeComponent();
        QuestionLabel.Text = Question;
    }

    private void OnYesClicked(object sender, EventArgs e)
    {
        OnResult?.Invoke(true);
        Close();
    }

    private void OnNoClicked(object sender, EventArgs e)
    {
        OnResult?.Invoke(false);
        Close();
    }

}