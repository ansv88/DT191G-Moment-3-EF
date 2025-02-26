using System.ComponentModel.DataAnnotations;

namespace Moment_3_EF.ViewModels;

public class BookEditViewModel
{
    public int Id { get; set; }  //Bok-ID

    [Required]
    [Display(Name = "Boktitel")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "FörfattarID")]
    public int WriterId { get; set; }  //Foreign Key

    [Required]
    [Display(Name = "Författare")]
    public string WriterName { get; set; } = string.Empty;

}
