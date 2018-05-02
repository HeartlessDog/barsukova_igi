using System;
using System.Collections.Generic;
using System.Linq;
using libr;
using libr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab6_igi.Controllers
{
    [Route("api/Library")]
    public class LibraryController : Controller
    {
        LibraryContext db;
        public LibraryController(LibraryContext context)
        {
            this.db = context;
            DbInitializer.Initialize(context);
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Issuence> Get()
        {
            var elems = db.Issuances
            .Include(x => x.Book)
            .Include(x => x.Reader)
            .ToList();
            return elems;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var elem = db.Issuances
            .Include(x => x.Book)
            .Include(x => x.Reader)
            .FirstOrDefault(x => x.ID == id);
            if (elem == null)
                return NotFound();
            return new ObjectResult(elem);

        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]Issuence issuence)
        {
            if (issuence == null)
            {
                ModelState.AddModelError("", "Не указаны данные!");
                return BadRequest(ModelState);
            }

            if (issuence.DateOfIssuance < issuence.DateOfReturn)
                ModelState.AddModelError("dateOfReturn", "Дата возврата предшествует дате выдачи!");

            if (issuence.DateOfIssuance > DateTime.Today || issuence.DateOfIssuance < DateTime.Parse("2000-01-01"))
                ModelState.AddModelError("dateOfIssuance", "Невозможная дата выдачи!");

            if (issuence.Return != true && issuence.Return != false)
                ModelState.AddModelError("return", "Укажите была ли книга возвращена(true/false)");
            
            
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                db.Issuances.Add(issuence);
                db.SaveChanges();
                var elem = db.Issuances
           .Include(x => x.Book)
           .Include(x => x.Reader)
           .FirstOrDefault(x => x.ID == issuence.ID);
                return Ok(elem);
            
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IActionResult Put([FromBody]Issuence issuence)
        {
            if (!db.Issuances.Any(x => x.ID == issuence.ID))
            {
                ModelState.AddModelError("", "Не существует такой записи");
                return BadRequest(ModelState);
            }
        
            if (issuence.DateOfIssuance < issuence.DateOfReturn)
                ModelState.AddModelError("dateOfReturn", "Дата возврата предшествует дате выдачи!");

            if (issuence.DateOfIssuance > DateTime.Today || issuence.DateOfIssuance < DateTime.Parse("2000-01-01"))
                ModelState.AddModelError("dateOfIssuance", "Невозможная дата выдачи!");

            if (issuence.Return != true && issuence.Return != false)
                ModelState.AddModelError("return", "Укажите была ли книга возвращена(true/false)");

            if (issuence == null)
            {
                ModelState.AddModelError("", "Не определено");
                return BadRequest(ModelState);
            }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                db.Update(issuence);
                db.SaveChanges();
                var elem = db.Issuances
                .Include(x => x.Book)
                .Include(x => x.Reader)
                .FirstOrDefault(x => x.ID == issuence.ID);
                return Ok(elem);
            
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Issuence issuence = db.Issuances.FirstOrDefault(x => x.ID == id);
            if (issuence == null)
            {
                return NotFound();
            }
            db.Issuances.Remove(issuence);
            db.SaveChanges();
            return Ok(issuence);

        }
    }
}