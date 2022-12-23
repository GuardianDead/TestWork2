using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestWork2.Data.Models
{
    public class File
    {
        [Key]
        [Required]
        [DisplayName("Имя")]
        public string Name { get; init; }
        [NotMapped]
        [JsonIgnore]
        public int RowCount { get; init; }

        public override string ToString() => Name;
    }
}