using Makara.ViewModels;

namespace Makara.Views;

public partial class DataMigratePage : ContentPage
{
    // Using constructor injection via DI, keeping code-behind minimal.
    public DataMigratePage(DataMigrateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}