using System.ComponentModel.DataAnnotations;

namespace Moment_3_EF.ViewModels;

public class BookCreateViewModel
{
    //Properties
    [Required]
    [Display(Name = "Boktitel")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Författare")]
    public int? WriterId { get; set; }

    [Display(Name = "Ny författare")]
    public string? NewWriterName { get; set; }
}