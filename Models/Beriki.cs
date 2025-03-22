using SQLite;

namespace Makara.Models
{
    [Table("diko")]
    public class Beriki
    {
        [PrimaryKey]
        public string Ber { get; set; }

        public string Def { get; set; }
        public string Status { get; set; } = StatusValues.Unspecified;

        // Common status values as constants for consistent usage
        public static class StatusValues
        {
            public const string Unspecified = "Unspecified";
            public const string Known = "Known";
            public const string Unknown = "Unknown";
            public const string Improper = "Improper";
            public const string Other = "Other";
        }
    }
}