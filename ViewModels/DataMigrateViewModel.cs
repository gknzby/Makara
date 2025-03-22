// No changes needed in DataMigrateViewModel.cs
// It interacts with WordModel only through the WordPickDatabase abstraction
// which handles the struct-specific logic internally
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Makara.Data;
using Makara.Helpers;

namespace Makara.ViewModels
{
    public partial class DataMigrateViewModel : ObservableObject
    {
        private readonly WordPickDatabase _database;

        public DataMigrateViewModel(WordPickDatabase database)
        {
            _database = database;
        }

        // Holds the folder (file) path entered by the user.
        [ObservableProperty]
        private string folderPath;

        // Entry counts before migration.
        [ObservableProperty]
        private int beforeCount;

        // Entry counts after migration.
        [ObservableProperty]
        private int afterCount;

        // Boolean to show processing state.
        [ObservableProperty]
        private bool isProcessing;

        // Computed property to enable the button when not processing.
        public bool IsNotProcessing => !isProcessing;

        // Migrates the file content into the target database.
        [RelayCommand]
        public async Task MigrateAsync()
        {
            IsProcessing = true;
            OnPropertyChanged(nameof(IsNotProcessing));
            try
            {
                // Get count before migration.
                BeforeCount = _database.GetAllWords().Count;

                // Verify file existence.
                if(!File.Exists(FolderPath))
                {
                    // Optionally show an error or simply exit.
                    return;
                }

                // Read file content.
                string text = await File.ReadAllTextAsync(FolderPath);
                if(string.IsNullOrWhiteSpace(text))
                {
                    // Fill empty file with default content.
                    text = "default content";
                    await File.WriteAllTextAsync(FolderPath, text);
                }
                // Parse and add words using the helper.
                WordParserHelper.ParseAndAddWords(text, _database);

                // Get count after migration.
                AfterCount = _database.GetAllWords().Count;
            }
            finally
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(IsNotProcessing));
            }
        }
    }
}