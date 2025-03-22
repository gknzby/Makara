using Makara.ViewModels;

namespace Makara.Views;

public partial class WordPickPage : ContentPage
{
    public WordPickPage(WordPickViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Ensure view model data is loaded when the page appears
        if(BindingContext is WordPickViewModel vm)
        {
            vm.LoadWordsCommand.Execute(null);
        }
    }
}