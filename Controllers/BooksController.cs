using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moment_3_EF.Data;
using Moment_3_EF.Models;
using Moment_3_EF.ViewModels;

namespace Moment_3_EF.Controllers;

public class BooksController : Controller
{
    private readonly LibraryDbContext _context;

    public BooksController(LibraryDbContext context)
    {
        _context = context;
    }

    // GET: Books
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.Include(book => book.Writer).ToListAsync(); //Hämtar alla böcker inklusive deras författare
        return View(books);
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //Hämtar boken med angivet ID inklusive författaren
        var book = await _context.Books
            .Include(b => b.Writer)
            .FirstOrDefaultAsync(book => book.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name"); //Skickar befintliga författare till vyn i dropdown-lista
        return View();
    }

    // POST: Books/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookCreateViewModel model)
    {
        //Kontrollera om formuläret är giltigt
        if (!ModelState.IsValid)
        {
            ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name", model.WriterId);
            return View(model);
        }

        //Hämta eller skapa författaren (hjälpfunktion längre ned i koden)
        int? writerId = await GetOrCreateWriterIdAsync(model.WriterId, model.NewWriterName);

        //Om ingen författare har valts eller lagts till, visa felmeddelande
        if (writerId == null)
        {
            ModelState.AddModelError("WriterId", "Du måste välja en författare eller ange en ny.");
            ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name");
            return View(model);
        }

        //Skapa en ny bok med den valda författaren
        var book = new BooksModel { Title = model.Title, WriterId = writerId.Value };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index)); //Redirecta till boklistan
    }

    // GET: BooksModels/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //Hämta boken inklusive författaren
        var book = await _context.Books
            .Include(book => book.Writer)
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        //Skapa ViewModel för att redigera boken och författaren
        var viewModel = new BookEditViewModel
        {
            Id = book.Id,
            Title = book.Title,
            WriterId = book.WriterId,
            WriterName = book.Writer?.Name ?? "Okänd författare"
        };

        ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name", book.WriterId);
        return View(viewModel);
    }

    // POST: BooksModels/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name", model.WriterId);
            return View(model);
        }

        var book = await _context.Books.Include(book => book.Writer).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return NotFound();

        book.Title = model.Title;

        //Hämta eller skapa författare (hjälpfunktion längre ned)
        int? writerId = await GetOrCreateWriterIdAsync(model.WriterId, model.WriterName);

        if (writerId == null)
        {
            ModelState.AddModelError("WriterId", "Du måste välja en författare eller ange en ny.");
            ViewBag.Writers = new SelectList(_context.Writers, "Id", "Name");
            return View(model);
        }

        book.WriterId = writerId.Value;

        _context.Update(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: BooksModels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var booksModel = await _context.Books
            .Include(book => book.Writer)
            .FirstOrDefaultAsync(book => book.Id == id);
        if (booksModel == null)
        {
            return NotFound();
        }

        return View(booksModel);
    }

    // POST: BooksModels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> BooksModelExistsAsync(int id)
    {
        return await _context.Books.FindAsync(id) != null;
    }

    //Hjälpmetod för att hitta eller skapa en författare
    private async Task<int?> GetOrCreateWriterIdAsync(int? writerId, string? newWriterName)
    {
        if (!string.IsNullOrWhiteSpace(newWriterName))
        {
            var existingWriter = await _context.Writers.FirstOrDefaultAsync(writer => writer.Name == newWriterName);

            if (existingWriter != null) return existingWriter.Id;

            var newWriter = new WritersModel { Name = newWriterName };
            _context.Writers.Add(newWriter);
            await _context.SaveChangesAsync();
            return newWriter.Id;
        }

        return writerId.HasValue ? writerId : null;
    }
}