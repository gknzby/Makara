using Makara.ViewModels;

namespace Makara.Views;

public partial class ItemsPage : ContentPage
{
    private readonly ItemsViewModel vm;

    public ItemsPage(ItemsViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = this.vm;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            this.vm?.LoadSampleItems();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in Button_Clicked: {ex}");
        }
    }
}