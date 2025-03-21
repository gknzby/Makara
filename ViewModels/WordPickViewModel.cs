using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Makara.Data;
using Makara.Models;
using System.Collections.ObjectModel;

namespace Makara.ViewModels;
public partial class WordPickViewModel : ObservableObject
{
    private readonly WordPickDatabase _database;

    public ObservableCollection<WordModel> KnownWords { get; } = new();
    public ObservableCollection<WordModel> OtherWords { get; } = new();

    public WordPickViewModel(WordPickDatabase database)
    {
        _database = database;
        LoadWords();
    }

    void LoadWords()
    {
        KnownWords.Clear();
        OtherWords.Clear();
        var allWords = _database.GetAllWords();

        foreach (var word in allWords)
        {
            if (word.Status == StatusType.Known)
                KnownWords.Add(word);
            else
                OtherWords.Add(word);
        }
    }

    [RelayCommand]
    void MarkAsKnown(WordModel word)
    {
        if (word != null)
        {
            word.Status = StatusType.Known;
            _database.UpdateWord(word);
            LoadWords();
        }
    }

    [RelayCommand]
    void MarkAsOther(WordModel word)
    {
        if (word != null)
        {
            word.Status = StatusType.Other;
            _database.UpdateWord(word);
            LoadWords();
        }
    }
}
