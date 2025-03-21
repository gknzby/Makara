using SQLite;

namespace Makara.Models
{
    [Table("diko")]
    public class Beriki
    {
        [PrimaryKey]
        public string Ber { get; set; }
        public string Def { get; set; }
        public StatusType Status { get; set; } = StatusType.Unspecified;
    }
}
