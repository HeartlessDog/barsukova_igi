using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using lab5.Data;
using lab5.Models;

namespace lab5.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
            public async Task<IActionResult> Index()
        {
            var journal = JsonConvert.DeserializeObject<List<string>>(HttpContext.Session.GetString("Journal"));
            ViewBag.UserActions = journal;
            ViewBag.StartIndex = 0;
            return View(await _context.Book.ToListAsync());
        }

        public ActionResult SortedList(bool name, bool year, int? pageStart = 0)
        {
            var sortedList = _context.Book.Include(b => b.Jenre).ToList();
            if (name & !year)
            {
                sortedList = sortedList.OrderBy(p => p.YearOfEdition).ToList();
            }
            else if (!name & year)
            {
                sortedList = sortedList.OrderBy(p => p.YearOfEdition).ToList();
            }
            else if (name & year)
            {
                sortedList.Sort(new BookComparer());
            }
            else
            {
                sortedList = _context.Book.Include(b => b.Jenre).ToList();
            }
            if (pageStart != null)
            {
                ViewBag.StartIndex = pageStart * 5;
            }
            return PartialView("SortedList", sortedList);
        }
        public void SaveFiltration(string find, bool first, bool second)
        {
            var findingTextJSON = JsonConvert.SerializeObject(find);
            HttpContext.Session.SetString("Book.Finding", findingTextJSON);
            var filterFirstJSON = JsonConvert.SerializeObject(first.ToString());
            HttpContext.Session.SetString("Book.Filter.First", filterFirstJSON);
            var filterSecondJSON = JsonConvert.SerializeObject(second.ToString());
            HttpContext.Session.SetString("Book.Filter.Second", filterSecondJSON);
        }
        // GET: Books/Details/5
        [Authorize]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "ID", book.JenreID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
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
            ViewData["JenreID"] = new SelectList(_context.Jenre, "ID", "ID", book.JenreID);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        protected IEnumerable<Book> Divider(IEnumerable<Book> query, int page)
        {
            List<Book> onePage = new List<Book>();
            for (int i = (query.Count() / 10) * (page - 1); i < ((page != 10) ? (query.Count() / 10) * page : query.Count()); i++)
            {
                onePage.Add(query.ElementAt(i));
            }
            return onePage;
        }


        public IActionResult GetPage(int page)
        {
            return PartialView("TablePartial", Divider(Container.LastQuery, page));
        }
    }

    public static class Container
    {
        private static IEnumerable<Book> lastQuery;

        public static IEnumerable<Book> LastQuery { get => lastQuery; set => lastQuery = value; }
    }
}
