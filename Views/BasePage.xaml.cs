namespace Makara.Views;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(this.Window != null)
        {
            this.Window.Activated += AutoFocusEntry;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if(this.Window != null)
        {
            this.Window.Activated -= AutoFocusEntry;
        }
    }

    private void AutoFocusEntry(object? sender, EventArgs e)
    {
        TestInput.Focus();
    }
}