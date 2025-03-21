using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input; 

namespace Makara.ViewModels 
{
    public partial class BasePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? entryText;
        [ObservableProperty]
        private string? labelText;

        public BasePageViewModel()
        {
            entryText = "Enter text here";
            labelText = "Text will appear here";
        }

        partial void OnEntryTextChanged(string? value)
        {
            LabelText = "Text changed: " + value;
        }
    }
}

