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
    public class ReadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Readers
        public async Task<IActionResult> Index()
        {
            var journal = JsonConvert.DeserializeObject<List<string>>(HttpContext.Session.GetString("Journal"));
            ViewBag.UserActions = journal;
            return View(await _context.Reader.ToListAsync());
        }

        public ActionResult SortedList(bool name, bool date, int? pageStart = 0)
        {
            var sortedList = _context.Reader.ToList();
            if (name & !date)
            {
                sortedList = sortedList.OrderBy(p => p.DateOfBirth).ToList();
            }
            else if (!name & date)
            {
                sortedList = sortedList.OrderBy(p => p.DateOfBirth).ToList();
            }
            else if (name & date)
            {
                sortedList.Sort(new ReaderComparer());
            }
            else
            {
                sortedList = _context.Reader.ToList();
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
            HttpContext.Session.SetString("Reader.Finding", findingTextJSON);
            var filterFirstJSON = JsonConvert.SerializeObject(first.ToString());
            HttpContext.Session.SetString("Reader.Filter.First", filterFirstJSON);
            var filterSecondJSON = JsonConvert.SerializeObject(second.ToString());
            HttpContext.Session.SetString("Reader.Filter.Second", filterSecondJSON);
        }

        // GET: Readers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader
                .SingleOrDefaultAsync(m => m.ID == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // GET: Readers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Readers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName,DateOfBirth,Sex,Adres,PhoneNumber,PassportData")] Reader reader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // GET: Readers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader.SingleOrDefaultAsync(m => m.ID == id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: Readers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,DateOfBirth,Sex,Adres,PhoneNumber,PassportData")] Reader reader)
        {
            if (id != reader.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaderExists(reader.ID))
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
            return View(reader);
        }

        // GET: Readers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader
                .SingleOrDefaultAsync(m => m.ID == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reader = await _context.Reader.SingleOrDefaultAsync(m => m.ID == id);
            _context.Reader.Remove(reader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaderExists(int id)
        {
            return _context.Reader.Any(e => e.ID == id);
        }
    }
}
