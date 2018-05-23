using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab2_library.Models;
using libr.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace lab2_library.wwwroot.Controllers
{
    public class BooksController : Controller
    {
        private readonly lab2_libraryContext _context;

        public BooksController(lab2_libraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var journal = JsonConvert.DeserializeObject<List<string>>(HttpContext.Session.GetString("Journal"));
            ViewBag.UserActions = journal;
            var lab2_libraryContext = _context.Book.Include(b => b.Jenre);
            return View(await lab2_libraryContext.ToListAsync());
        }
        public ActionResult SortedList(bool name)
        {
            var sortedList = _context.Book.Include(b => b.Jenre).ToList();
            if (name)
            {
                sortedList = sortedList.OrderBy(p => p.YearOfEdition).ToList();
            }
            else
            {
                sortedList = _context.Book.ToList();
            }
            return PartialView("SortedList", sortedList);
        }
        public void SaveFiltration(string find, bool first)
        {
            var findingTextJSON = JsonConvert.SerializeObject(find);
            HttpContext.Session.SetString("Book.Finding", findingTextJSON);
            var filterFirstJSON = JsonConvert.SerializeObject(first.ToString());
            HttpContext.Session.SetString("Book.Filter.First", filterFirstJSON);
         
        }
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Jenre)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RegistrationNumber,Name,Author,Edition,YearOfEdition,JenreID,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "Name", book.JenreID);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "Name", book.JenreID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RegistrationNumber,Name,Author,Edition,YearOfEdition,JenreID,Price")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
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
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "Name", book.JenreID);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Jenre)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.SingleOrDefaultAsync(m => m.ID == id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
