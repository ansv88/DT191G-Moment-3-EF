using System.ComponentModel.DataAnnotations;

namespace Moment_3_EF.Models;

public class WritersModel
{
    //Properties
    public int Id { get; set; }

    [Required]
    [Display(Name = "Författare")]
    public string Name { get; set; } = string.Empty;

    public List<BooksModel> Books { get; set; } = new(); //En författare kan ha flera böcker
}