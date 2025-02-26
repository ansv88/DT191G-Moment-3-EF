using System.ComponentModel.DataAnnotations;

namespace Moment_3_EF.Models;

public class BooksModel
{
    //Properties
    public int Id { get; set; }

    [Required]
    [Display(Name = "Titel")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "FörfattarID")]
    public int WriterId { get; set; }  //Foreign Key

    [Display(Name = "Författare")]
    public WritersModel? Writer { get; set; } //Navigation property

}