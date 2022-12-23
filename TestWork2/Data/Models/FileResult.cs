using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestWork2.Data.Models;

public class FileResult
{
    [Key]
    [DisplayName("Индификатор")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    [DisplayName("Время исполнения")]
    [DataType(DataType.Time)]
    public TimeSpan ElapsedTime { get; set; }
    [DisplayName("Минимальное время")]
    public DateTime MinDateTime { get; set; }
    [DisplayName("Среднее значение секунд")]
    public double AverageSeconds { get; set; }
    [DisplayName("Медиана секунд")]
    public double MedianSeconds { get; set; }
    [DisplayName("Среднее значение индикатора")]
    public double AverageIndicator { get; set; }
    [DisplayName("Самое большое значение индикатора")]
    public double MaxIndicator { get; set; }
    [DisplayName("Самое маленькое значение индикатора")]
    public double MinIndicator { get; set; }
    [DisplayName("Количество строк")]
    public int RowCount { get; set; }

    [Display(AutoGenerateField = false)]
    [DisplayName("Файл")]
    [JsonIgnore]
    public string FileName { get; set; }
    [ForeignKey("FileName")]
    public File File { get; set; }
    
    public override string ToString() => Convert.ToString(Id);
}