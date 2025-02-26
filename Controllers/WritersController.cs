using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moment_3_EF.Data;
using Moment_3_EF.Models;

namespace Moment_3_EF.Controllers;

public class WritersController : Controller
{
    private readonly LibraryDbContext _context;

    public WritersController(LibraryDbContext context)
    {
        _context = context;
    }

    // GET: Writers
    public async Task<IActionResult> Index()
    {
        var writers = await _context.Writers.ToListAsync();
        return View(writers);
    }

    // GET: Writers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //Hämta alla böcker av en författare
        var writer = await _context.Writers
            .Include(writer => writer.Books)
            .FirstOrDefaultAsync(writer => writer.Id == id);
        if (writer == null)
        {
            return NotFound();
        }

        return View(writer);
    }

    // GET: Writers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Writers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] WritersModel writer)
    {
        if (ModelState.IsValid)
        {
            _context.Add(writer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(writer);
    }

    // GET: Writers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var writer = await _context.Writers.FindAsync(id);
        if (writer == null)
        {
            return NotFound();
        }
        return View(writer);
    }

    // POST: Writers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] WritersModel writer)
    {
        if (id != writer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(writer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WriterExists(writer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(writer);
    }

    // GET: Writers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var writer = await _context.Writers
            .FirstOrDefaultAsync(writer => writer.Id == id);
        if (writer == null)
        {
            return NotFound();
        }

        return View(writer);
    }

    // POST: Writers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var writer = await _context.Writers.FindAsync(id);
        if (writer != null)
        {
            _context.Writers.Remove(writer);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool WriterExists(int id)
    {
        return _context.Writers.Any(writer => writer.Id == id);
    }
}
