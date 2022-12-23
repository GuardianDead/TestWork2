using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestWork2.Data.Models
{
    public class FileValue
    {
        [Key]
        [DisplayName("Индификатор")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [DisplayName("Время")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
        [DisplayName("Секунды")]
        public int Seconds { get; set; }
        [DisplayName("Индикатор")]
        public double Indicator { get; set; }
        
        [Display(AutoGenerateField = false)]
        [DisplayName("Файл")]
        [JsonIgnore]
        public string FileName { get; set; }
        [ForeignKey("FileName")]
        public File File { get; set; }

        public override string ToString() => Convert.ToString(Id);
    }
}
