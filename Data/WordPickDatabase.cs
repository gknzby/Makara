using Makara.Models;

using SQLite;

namespace Makara.Data;

public class WordPickDatabase : IDisposable
{
    private readonly SQLiteConnection _db;

    // Updated to use string keys instead of StatusType enum
    private Dictionary<string, List<WordModel>> _wordCache;

    private bool _isDataCached = false;
    private bool _disposed = false;

    public WordPickDatabase()
    {
        // Use an in-memory DB or proper file path for production
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wordpick.db3");
        _db = new SQLiteConnection(dbPath);
        _db.CreateTable<WordModel>();
        _wordCache = new Dictionary<string, List<WordModel>>();
    }

    public List<WordModel> GetAllWords()
    {
        if(_isDataCached)
        {
            // If we have a cache, combine all entries from cache
            return _wordCache.Values.SelectMany(list => list).ToList();
        }

        // No cache, load from database and build cache
        var allWords = _db.Table<WordModel>().ToList();
        BuildCache(allWords);
        return allWords;
    }

    private void BuildCache(List<WordModel> allWords)
    {
        _wordCache.Clear();

        foreach(var word in allWords)
        {
            if(!_wordCache.ContainsKey(word.Status))
            {
                _wordCache[word.Status] = new List<WordModel>();
            }
            _wordCache[word.Status].Add(word);
        }

        _isDataCached = true;
    }

    // Updated to accept string status instead of StatusType enum
    public List<WordModel> GetWordsByStatus(string status)
    {
        if(_isDataCached && _wordCache.ContainsKey(status))
        {
            return _wordCache[status];
        }

        // Cache miss or no cache, query database
        var words = _db.Table<WordModel>()
                     .Where(w => w.Status == status)
                     .ToList();

        // Update cache for just this status
        if(!_isDataCached)
        {
            _wordCache[status] = words;
        }

        return words;
    }

    public int UpdateWord(WordModel word)
    {
        // Update the database
        var result = _db.Update(word);

        // If successful, update the cache accordingly
        if(result > 0 && _isDataCached)
        {
            // Find the word in the cache by its primary key (Word property)
            foreach(var statusList in _wordCache.Values)
            {
                var index = statusList.FindIndex(w => w.Word == word.Word);
                if(index >= 0)
                {
                    // Remove the word from its old status list
                    statusList.RemoveAt(index);
                    break;
                }
            }

            // Add the word to its new status list
            if(!_wordCache.ContainsKey(word.Status))
            {
                _wordCache[word.Status] = new List<WordModel>();
            }
            _wordCache[word.Status].Add(word);
        }

        return result;
    }

    public int InsertWord(WordModel word)
    {
        // Try to find if the word already exists
        var existingWord = _db.Table<WordModel>().Where(w => w.Word == word.Word).FirstOrDefault();

        if(existingWord == null)
        {
            // Word doesn't exist, insert it
            var result = _db.Insert(word);

            // Update cache if needed
            if(result > 0 && _isDataCached)
            {
                if(!_wordCache.ContainsKey(word.Status))
                {
                    _wordCache[word.Status] = new List<WordModel>();
                }
                _wordCache[word.Status].Add(word);
            }

            return result;
        }

        // Word already exists
        return 0;
    }

    public void InvalidateCache()
    {
        _isDataCached = false;
        _wordCache.Clear();
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                // Dispose managed resources here
                _db.Dispose();
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