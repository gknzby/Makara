using SQLite;

namespace Makara.Models;
public enum StatusType
{
    Unspecified = 1,
    Known = 2,
    Unknown = 4,
    Improper = 8,
    Other = 16
}

[Table("WordPick")]
public class WordModel
{
    [PrimaryKey, Unique]
    public string Word { get; set; }

    public StatusType Status { get; set; } = StatusType.Unspecified;
}