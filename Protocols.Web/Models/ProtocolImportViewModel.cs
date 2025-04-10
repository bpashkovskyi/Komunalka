using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Protocols.Web.Models;

public class ProtocolImportViewModel
{
    [Required]
    [DisplayName("Номер протоколу від 1 до 100")]
    [Range(1, 100, ErrorMessage = "Номер протоколу повинен бути від 1 до 100.")]
    public string Number { get; set; }

    [Required]
    [DisplayName("Дата засідання у форматі dd.mm.yyyy. Наприклад 30.06.2025")]
    [RegularExpression(@"^\d{2}\.\d{2}\.\d{4}$", ErrorMessage = "Дата повинна бути у форматі dd.mm.yyyy. Наприклад 30.06.2025")]
    public string Date { get; set; }

    [Required]
    [DisplayName("Файл протоколу. Тільки у pdf форматі")]
    [DataType(DataType.Upload)]
    public IFormFile File { get; set; }
}