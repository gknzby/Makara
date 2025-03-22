using System.ComponentModel;
using System.Runtime.CompilerServices;

using SQLite;

namespace Makara.Models;
// Implement INotifyPropertyChanged to notify UI of property changes

[Table("WordPick")]
public class WordModel : INotifyPropertyChanged
{
    [PrimaryKey, Unique]
    public string Word { get; set; }

    // Backing field for Status property
    private string _status = StatusValues.Unspecified;

    // Changed from StatusType enum to string with property change notification
    public string Status
    {
        get => _status;
        set
        {
            if(_status == value) return;
            _status = value;
            OnPropertyChanged();
        }
    }

    // Common status values as constants for consistent usage
    public static class StatusValues
    {
        public const string Unspecified = "Unspecified";
        public const string Known = "Known";
        public const string Unknown = "Unknown";
        public const string Improper = "Improper";
        public const string Other = "Other";
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}