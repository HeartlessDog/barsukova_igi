using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using lab5.Data;
using lab5.Models;

namespace lab5.Controllers
{
    public class IssuencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult SortedList(bool name, bool year, int? pageStart=0)
        {
            var sortedList = _context.Issuence.Include(b => b.Reader).Include(c=>c.Book).ToList();
            if (name & !year)
            {
                sortedList = sortedList.OrderBy(p => p.DateOfIssuance).ToList();
            }
            else if (!name & year)
            {
                sortedList = sortedList.OrderBy(p => p.DateOfIssuance).ToList();
            }
            else if (name & year)
            {
                sortedList.Sort(new IssuenceComparer());
            }
            else
            {
                sortedList = _context.Issuence.Include(b => b.Reader).Include(c => c.Book).ToList();
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
            HttpContext.Session.SetString("Issuence.Finding", findingTextJSON);
            var filterFirstJSON = JsonConvert.SerializeObject(first.ToString());
            HttpContext.Session.SetString("Issuence.Filter.First", filterFirstJSON);
            var filterSecondJSON = JsonConvert.SerializeObject(second.ToString());
            HttpContext.Session.SetString("Issuence.Filter.Second", filterSecondJSON);
        }

        // GET: Issuences
        public async Task<IActionResult> Index()
        {
            var journal = JsonConvert.DeserializeObject<List<string>>(HttpContext.Session.GetString("Journal"));
            ViewBag.UserActions = journal;
            var lab2_libraryContext = _context.Issuence.Include(i => i.Book).Include(i => i.Reader);
            return View(await lab2_libraryContext.ToListAsync());
        }

        // GET: Issuences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuence = await _context.Issuence
                .Include(i => i.Book)
                .Include(i => i.Reader)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (issuence == null)
            {
                return NotFound();
            }

            return View(issuence);
        }

        // GET: Issuences/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Book, "ID", "Name");
            ViewData["ReaderID"] = new SelectList(_context.Reader, "ID", "FullName");
            return View();
        }

        // POST: Issuences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ReaderID,BookID,DateOfIssuance,DateOfReturn,Return")] Issuence issuence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issuence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Book, "ID", "Name", issuence.BookID);
            ViewData["ReaderID"] = new SelectList(_context.Reader, "ID", "FullName", issuence.ReaderID);
            return View(issuence);
        }

        // GET: Issuences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuence = await _context.Issuence.SingleOrDefaultAsync(m => m.ID == id);
            if (issuence == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Book, "ID", "Name", issuence.BookID);
            ViewData["ReaderID"] = new SelectList(_context.Reader, "ID", "FullName", issuence.ReaderID);
            return View(issuence);
        }

        // POST: Issuences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ReaderID,BookID,DateOfIssuance,DateOfReturn,Return")] Issuence issuence)
        {
            if (id != issuence.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issuence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssuenceExists(issuence.ID))
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
            ViewData["BookID"] = new SelectList(_context.Book, "ID", "Name", issuence.BookID);
            ViewData["ReaderID"] = new SelectList(_context.Reader, "ID", "FullName", issuence.ReaderID);
            return View(issuence);
        }

        // GET: Issuences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuence = await _context.Issuence
                .Include(i => i.Book)
                .Include(i => i.Reader)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (issuence == null)
            {
                return NotFound();
            }

            return View(issuence);
        }

        // POST: Issuences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issuence = await _context.Issuence.SingleOrDefaultAsync(m => m.ID == id);
            _context.Issuence.Remove(issuence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssuenceExists(int id)
        {
            return _context.Issuence.Any(e => e.ID == id);
        }
    }
}
