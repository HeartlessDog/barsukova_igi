using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
namespace Lab3IGI.Controllers
{
    public class HomeController : Controller
    {
        LibraryContext db = new LibraryContext();
        IMemoryCache _memoryCache;
        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [ResponseCache(CacheProfileName = "NoCaching")]
        public IActionResult Index()
        {
            DbInitializer.Initialize(db);
            return View();
        }
        [ResponseCache(CacheProfileName = "Caching")]
        public async Task<IActionResult> First()
        {
            string path = Request.Path.Value.ToLower();
            Book BookFromMemory = (Book)_memoryCache.Get(path);
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, Jenre> elems = null;
            HttpContext.Session.Get("First");
            if (HttpContext.Session.Get("first") != null)
            {
                string str = HttpContext.Session.GetString("first");
                string[] strM = str.Split(";");
                Book book = new Book();

                book.RegistrationNumber = Convert.ToInt32(strM[0]);
                book.Name = strM[1];
                book.Author = strM[2];
                book.Edition = strM[3];
                book.JenreID = Convert.ToInt32(strM[4]);
                book.YearOfEdition = Convert.ToInt32(strM[5]);
                book.Price = (float)Convert.ToDouble(strM[6]);
                ViewData["first"] = book;
            }
            else
            {
                using (LibraryContext db = new LibraryContext())
                {
                    ViewData["Jenres"] = db.Jenres.ToList();
                    string str = JsonConvert.SerializeObject(db.Jenres.ToList());

                    HttpContext.Session.SetString("First", str);
                }
            }
            using (LibraryContext db = new LibraryContext())
            {
                elems = db.Books.Include(x => x.Jenre);
                ViewData["Jenres"] = db.Jenres.ToList();
                ViewData["BookFromMemory"] = BookFromMemory;
                return View(await elems.ToListAsync());
            }
        }
    
        [ResponseCache(CacheProfileName = "Caching")]
        public PartialViewResult TbodyInsert()
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddMinutes(1);
            List<Reader> elems = null;
            string path = "/htmlpage.html";
            Reader ReaderFromMemory = (Reader)_memoryCache.Get(path);

            if (Request.Cookies["Fourth"] != null)
            {
                elems = JsonConvert.DeserializeObject<List<Reader>>(Request.Cookies["Fourth"].ToString());

            }
            else
            {
                using (LibraryContext db = new LibraryContext())
                {
                    elems = db.Readers.ToList();
                    string str = JsonConvert.SerializeObject(elems.Take(2));
                    Response.Cookies.Append("Fourth", str, cookie);
                }

            }
            ViewData["ReaderFromMemory"] = ReaderFromMemory;
            using (LibraryContext db = new LibraryContext())
            {
                return PartialView("PartialViewForStatic", elems);
            }
        }
        public PartialViewResult FormInsert()
        {
            if (HttpContext.Session.Get("Fourth") != null)
            {
                string str = HttpContext.Session.GetString("Fourth");
                string[] strM = str.Split(";");
                Reader reader = new Reader();
                reader.FullName = strM[0];
                reader.DateOfBirth = Convert.ToDateTime(strM[1]);
                reader.Adres = strM[2];
                reader.PassportData = strM[3];
                reader.PhoneNumber = strM[4];
                ViewData["Fourth"] = reader;
            }

            using (LibraryContext db = new LibraryContext())
            {
                return PartialView("PartialViewForForm");
            }
        }
        [HttpPost]
        public IActionResult AddNewItemReader(Reader reader)
        {
            using (LibraryContext db = new LibraryContext())
            {
                db.Readers.Add(reader);
                db.SaveChanges();
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddMinutes(1);
                var elems = db.Readers.ToList();
                string str = reader.FullName + ";" + reader.DateOfBirth + ";" + reader.Adres + ";" + reader.PassportData + ";" + reader.PhoneNumber;
                HttpContext.Session.SetString("Fourth", str);
            }

            return Redirect("~/htmlpage.html");
        }

        
        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Third()
        {
            if (HttpContext.Session.Get("third") != null)
            {
                string str = HttpContext.Session.GetString("third");
                string[] strM = str.Split(";");
                Issuence issuence = new Issuence();

                issuence.BookID = Convert.ToInt32(strM[0]);
                issuence.ReaderID = Convert.ToInt32(strM[1]);
                issuence.DateOfIssuance = Convert.ToDateTime(strM[2]);
                issuence.Return = Convert.ToBoolean(strM[3]);
                ViewData["third"] = issuence;
            }
            string path = Request.Path.Value.ToLower();
            Issuence IssuenceFromMemory = (Issuence)_memoryCache.Get(path);
            using (LibraryContext db = new LibraryContext())
            {
                var elems = db.Issuances.Include(x => x.Reader)
                              .Include(bx => bx.Book);
                ViewData["Readers"] = db.Readers.ToList();
                ViewData["Books"] = db.Books.ToList();
                ViewData["IssuenceFromMemory"] = IssuenceFromMemory;
                return View(elems.ToList());
            }
        }
        [HttpPost]
        public IActionResult AddNewItemIssuence(Issuence issuence, int ReaderID, int BookID)
        {
            using (LibraryContext db = new LibraryContext())
            {
                issuence.ReaderID = ReaderID;
                issuence.BookID = BookID;
                db.Issuances.Add(issuence);
                db.SaveChanges();
                string str = issuence.BookID + ";" + issuence.ReaderID + ";" + issuence.DateOfIssuance + ";" + issuence.Return;
                HttpContext.Session.SetString("third", str);
            }
            return Redirect("~/Home/Third");
        }
        [HttpPost]
        public IActionResult AddNewItemBook(Book book, int JenreID)
        {
            using (LibraryContext db = new LibraryContext())
            {
                book.JenreID = JenreID;
                db.Books.Add(book);
                db.SaveChanges();
                string str = book.Name + ";" + book.Author + ";" + book.Edition + ";" + book.JenreID + ";" + book.YearOfEdition + ";" + book.Price;
                HttpContext.Session.SetString("First", str);
            }
            return Redirect("~/Home/First");
        }


    }
}
