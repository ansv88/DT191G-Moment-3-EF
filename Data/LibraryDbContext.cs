using Microsoft.EntityFrameworkCore;
using Moment_3_EF.Models;
namespace Moment_3_EF.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {} //Skickar konfigurationsinställningar

    public DbSet<BooksModel> Books { get; set; } //Skapar en tabell för böcker
    public DbSet<WritersModel> Writers { get; set; } //Skapar en tabell för författare

}