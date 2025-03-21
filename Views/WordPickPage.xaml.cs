using Makara.ViewModels;

namespace Makara.Views;
public partial class WordPickPage : ContentPage
{
    public WordPickPage(WordPickViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
