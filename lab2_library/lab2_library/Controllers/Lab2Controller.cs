using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab2_library.Models;
using libr.Models;

namespace lab2_library.Controllers
{
    public class Lab2Controller : Controller
    {
        private readonly lab2_libraryContext _context;

        public Lab2Controller(lab2_libraryContext context)
        {
            _context = context;
        }

        // GET: Books
        [HttpGet]
        public async Task<IActionResult> GetBooks(string id)
        {
            var books = from b in _context.Book.Include(b => b.Jenre)
            select b;

            if (!String.IsNullOrEmpty(id))
            {
                books = books.Where(s => s.Name.Contains(id));
            }

            return View(await books.ToListAsync());
        }

        // GET: Readers
        [HttpGet]
        public async Task<IActionResult> GetReaders(string id)
        {
            var readers = from r in _context.Reader
                        select r;

            if (!String.IsNullOrEmpty(id))
            {
                readers = readers.Where(s => s.FullName.Contains(id));
            }

            return View(await readers.ToListAsync());
        }

        // GET: Jenres
        [HttpGet]
        public async Task<IActionResult> GetJenres(string id)
        {
            var jenres = from j in _context.Jenre
                          select j;

            if (!String.IsNullOrEmpty(id))
            {
                jenres = jenres.Where(s => s.Name.Contains(id));
            }

            return View(await jenres.ToListAsync());
        }


        // GET: Issuences
        [HttpGet]
        public async Task<IActionResult> GetIssuences(string id)
        {
            var issuences = from i in _context.Issuence.Include(i=>i.Reader).Include(i=>i.Book)
                         select i;

            if (!String.IsNullOrEmpty(id))
            {
                issuences = issuences.Where(s => s.Reader.FullName.Contains(id));
            }

            return View(await issuences.ToListAsync());
        }

        // GET: Books/Create
        public IActionResult CreateBook()
        {
            ViewData["JenreID"] = new SelectList(_context.Set<Jenre>(), "ID", "ID");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook([Bind("ID,RegistrationNumber,Name,Author,Edition,YearOfEdition,JenreID,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetBooks));
            }
            ViewData["JenreID"] = new SelectList(_context.Set<Jenre>(), "ID", "ID", book.JenreID);
            return View(book);
        }
    }
}
