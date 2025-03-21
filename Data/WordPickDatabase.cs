using SQLite;
using Makara.Models;

namespace Makara.Data;
public class WordPickDatabase
{
    readonly SQLiteConnection _db;
    public WordPickDatabase()
    {
        // Use an in-memory DB or proper file path for production
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wordpick.db3");
        _db = new SQLiteConnection(dbPath);
        _db.CreateTable<WordModel>();
    }

    public List<WordModel> GetAllWords() =>
        _db.Table<WordModel>().ToList();

    public int UpdateWord(WordModel word) =>
        _db.Update(word);

    // New helper method to insert a word into the database.
    public int InsertWord(WordModel word) =>
        _db.Insert(word);
}
