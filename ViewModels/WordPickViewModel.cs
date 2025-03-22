using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Makara.Data;
using Makara.Models;

namespace Makara.ViewModels;

public partial class WordPickViewModel : ObservableObject, IDisposable
{
    private readonly WordPickDatabase _database;
    private bool _disposed = false;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private int totalWordsCount;

    public ObservableCollection<WordModel> AllWords { get; } = new();

    public WordPickViewModel(WordPickDatabase database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task LoadWords()
    {
        if(IsLoading) return;

        try
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                var allWords = _database.GetAllWords();

                // Process in batches to prevent UI freezing with large datasets
                const int batchSize = 50;
                var wordsBatch = new List<WordModel>(batchSize);

                // Clear collections on UI thread to avoid cross-thread operations
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AllWords.Clear();
                });

                foreach(var word in allWords)
                {
                    wordsBatch.Add(word);

                    // Add batch to observable collection
                    if(wordsBatch.Count >= batchSize)
                    {
                        var tempBatch = new List<WordModel>(wordsBatch);
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            foreach(var w in tempBatch)
                                AllWords.Add(w);
                        });
                        wordsBatch.Clear();
                    }
                }

                // Add remaining items
                if(wordsBatch.Count > 0)
                {
                    var tempBatch = new List<WordModel>(wordsBatch);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach(var w in tempBatch)
                            AllWords.Add(w);
                    });
                }

                // Update the count after loading all words
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TotalWordsCount = AllWords.Count;
                });
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void MarkAsKnown(WordModel word)
    {
        UpdateWordStatus(word, WordModel.StatusValues.Known);
    }

    [RelayCommand]
    private void MarkAsUnknown(WordModel word)
    {
        UpdateWordStatus(word, WordModel.StatusValues.Unknown);
    }

    [RelayCommand]
    private void MarkAsImproper(WordModel word)
    {
        UpdateWordStatus(word, WordModel.StatusValues.Improper);
    }

    [RelayCommand]
    private void MarkAsOther(WordModel word)
    {
        UpdateWordStatus(word, WordModel.StatusValues.Other);
    }

    // Simplified helper method to update word status
    private void UpdateWordStatus(WordModel word, string newStatus)
    {
        if(word == null) return;

        // No need to update if status is already the same
        if(word.Status == newStatus) return;

        // Update the status - this will trigger UI update via INotifyPropertyChanged
        word.Status = newStatus;

        // Update the database
        _database.UpdateWord(word);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                _database.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}