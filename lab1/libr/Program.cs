using System;
using libr.Models;
using System.Collections.Generic;
using System.Linq;

namespace libr
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new LibraryContext();
            libr.DbInitializer.Initialize(db);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose task: ");
                Console.WriteLine("1: Out table Books");
                Console.WriteLine("2: Out table Books with year of edition after 2000");
                Console.WriteLine("3: Out table Books grouped by year");
                Console.WriteLine("4: Out tables of Books with genres");
                Console.WriteLine("5: Out table Books with genres where year of edition after 2000 ");
                Console.WriteLine("6: Add Book");
                Console.WriteLine("7: Add Reader");
                Console.WriteLine("8: Dell Book");
                Console.WriteLine("9: Dell Reader");
                Console.WriteLine("0: Edit Book");
                char action = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (action)
                {
                    case '1':
                        OutBooks(GetBookList(db));
                        break;
                    case '2':
                        FilterByYear(GetBookList(db));
                        break;
                    case '3':
                        GroupBooksByYear(GetBookList(db));
                        break;
                    case '4':
                        JoinTwoTables(GetBookList(db), GetJenreList(db));
                        break;
                    case '5':
                        JoinTwoTablesAndFilterIt(GetBookList(db), GetJenreList(db));
                        break;
                    case '6':
                        Console.WriteLine("Input name of book");
                        string name = Console.ReadLine();
                        Console.WriteLine("Input registration number of book");
                        string regNumber = Console.ReadLine();

                        Console.WriteLine("Input genre ID");
                        OutJenres(GetJenreList(db));
                        string  genreID= Console.ReadLine();

                        Console.WriteLine("Input author");
                        string author = Console.ReadLine();

                        Console.WriteLine("Input edition");
                        string edition = Console.ReadLine();

                        Console.WriteLine("Input price");
                        string price = Console.ReadLine();

                        Console.WriteLine("Input year of edition");
                        string yearOfEdition = Console.ReadLine();

                        AddBook(db, new Book
                        {
                            Name = name,
                            RegistrationNumber = Int32.Parse(regNumber),
                            Author = author,
                            Edition = edition,
                            Price = Int32.Parse(price),
                            YearOfEdition = Int32.Parse(yearOfEdition),
                            JenreID = Int32.Parse(genreID)                            
                        });
                        break;
                    case '7':
                        Console.WriteLine("Input full name");
                        string fullName = Console.ReadLine();
                        Console.WriteLine("Input date of birth");
                        string dateOfBirth = Console.ReadLine();
                        Console.WriteLine("Input sex");
                        string sex = Console.ReadLine();
                        Console.WriteLine("Input adress");
                        string adress = Console.ReadLine();
                        Console.WriteLine("Input passportData");
                        string passportData = Console.ReadLine();
                        Console.WriteLine("Input phoneNumber");
                        string phoneNumber = Console.ReadLine();

                        AddReader(db, new Reader {
                            FullName = fullName,
                            DateOfBirth=DateTime.Parse(dateOfBirth),
                            Sex=sex,
                            Adres=adress,
                            PassportData=passportData,
                            PhoneNumber=phoneNumber

                        });
                        break;
                    case '8':
                        OutBooks(GetBookList(db));
                        Console.WriteLine("Input index of deleted element");
                        int id = Int32.Parse(Console.ReadLine());
                        DellBook(db, id);
                        break;
                    case '9':
                        OutReaders(GetReaderList(db));
                        Console.WriteLine("Input index of deleted element");
                        id = Int32.Parse(Console.ReadLine());
                        DellReader(db, id);
                        break;
                    case '0':
                        OutBooks(GetBookList(db));
                        Console.WriteLine("Input index of edited element");
                        id = Int32.Parse(Console.ReadLine());

                        Console.WriteLine("Input name of book");
                        name = Console.ReadLine();
                        Console.WriteLine("Input registration number of book");
                        regNumber = Console.ReadLine();

                        Console.WriteLine("Input genre ID");
                        OutJenres(GetJenreList(db));
                        genreID = Console.ReadLine();

                        Console.WriteLine("Input author");
                        author = Console.ReadLine();

                        Console.WriteLine("Input edition");
                        edition = Console.ReadLine();

                        Console.WriteLine("Input price");
                        price = Console.ReadLine();

                        Console.WriteLine("Input year of edition");
                        yearOfEdition = Console.ReadLine();
                    
                        EditBook(db, id, new Book
                        {
                            Name = name,
                            RegistrationNumber = Int32.Parse(regNumber),
                            Author = author,
                            Edition = edition,
                            Price = Int32.Parse(price),
                            YearOfEdition = Int32.Parse(yearOfEdition),
                            JenreID = Int32.Parse(genreID)
                        });
                        break;
                }
                Console.ReadKey();
            }
        }
        static void OutIssuence(List<Issuence> issuences)
        {
            Console.WriteLine("._____.__________.__________.");
            foreach (Issuence issuence in issuences)
            {
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|", issuence.ID, issuence.Reader, issuence.Book));
                Console.WriteLine("|_____|__________|__________|");
            }
        }
        static void OutBooks(List<Book> books)
        {
            Console.WriteLine("._____._____.__________._____.__________.__________._____.");
            Console.WriteLine(String.Format("|   ID|  Num|      Name| Year|    Author|   Edition|Price|"));
            Console.WriteLine("|_____|_____|__________|_____|__________|__________|_____|");
            foreach (Book book in books)
            {
                Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,10}|{3,5}|{4,10}|{5,10}|{6,5}|", book.ID, book.RegistrationNumber, book.Name, book.YearOfEdition,book.Author,book.Edition, book.Price));
                Console.WriteLine("|_____|_____|__________|_____|__________|__________|_____|");
            }
        }
        static void OutReaders(List<Reader> readers)
        {
            Console.WriteLine("._____.__________._________________________.");
            foreach (Reader reader in readers)
            {
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,25}|", reader.ID, reader.FullName, reader.DateOfBirth));
                Console.WriteLine("|_____|__________|_________________________|");
            }
        }
        static void OutJenres(List<Jenre> jenres)
        {
            Console.WriteLine("._____._______________.");
            foreach (Jenre jenre in jenres)
                Console.WriteLine(String.Format("|{0,5}|{1,15}|", jenre.ID, jenre.Name));
            Console.WriteLine("|_____|_______________|");

        }

        static List<Issuence> GetIssuenceList(LibraryContext db)
        {
            List<Issuence> issuences = null;
            issuences = db.Issuances.ToList();
            return issuences;
        }
        static List<Book> GetBookList(LibraryContext db)
        {
            List<Book> books = null;
            books = db.Books.ToList();
            return books;
        }
        static List<Reader> GetReaderList(LibraryContext db)
        {
            List<Reader> readers = null;
            readers = db.Readers.ToList();
            return readers;
        }
        static List<Jenre> GetJenreList(LibraryContext db)
        {
            List<Jenre> jenres = null;
            jenres = db.Jenres.ToList();
            return jenres;
        }

        static void FilterByYear(List<Book> list)
        {
            var result =
                from item in list
                where item.YearOfEdition >2000
                select item;
            OutBooks(result.ToList());
        }
        static void GroupBooksByYear(List<Book> list)
        {
            var result =
                from item in list
                group item by item.YearOfEdition;
            foreach (var group in result)
            {
                Console.WriteLine(group.Key);
                OutBooks(group.ToList());
            }
        }
        static void JoinTwoTables(List<Book> books, List<Jenre> genres)
        {
            var result =
                books.Join(
                    genres,
                    p => p.JenreID,
                    q => q.ID,
                    (p, q) => new
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Jenre = q.Name
                    });
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,15}|", "ID", "Name", "Jenre"));
            Console.WriteLine("|_____|__________|_______________|");
            foreach (var item in result)
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,15}|", item.ID, item.Name, item.Jenre));
            Console.WriteLine("|_____|__________|_______________|");

        }
        static void JoinTwoTablesAndFilterIt(List<Book> books, List<Jenre> genres)
        {
            var result =
                books.Join(
                    genres,
                    p => p.JenreID,
                    q => q.ID,
                    (p, q) => new
                    {
                        ID = p.ID,
                        Name = p.Name,
                        YearOfEdition = p.YearOfEdition,
                        Jenre = q.Name
                    });
            result =
                from res in result
                where res.YearOfEdition > 2000
                select res;
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,5}|{3,15}|", "ID", "Name", "Year", "Jenre"));
            Console.WriteLine("|_____|__________|_____|_______________|");

            foreach (var item in result)
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,5}|{3,15}|", item.ID, item.Name, item.YearOfEdition, item.Jenre));
            Console.WriteLine("|_____|__________|_____|_______________|");

        }

        static void AddBook(LibraryContext db, Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
        }
        static void AddReader(LibraryContext db, Reader reader)
        {
            db.Readers.Add(reader);
            db.SaveChanges();
        }
   
        static void EditBook(LibraryContext db, int id, Book book)
        {
            Book oldValue = db.Books.Where(a => a.ID == id).First();

            oldValue.Name = book.Name;
            oldValue.RegistrationNumber = book.RegistrationNumber;
            oldValue.Author = book.Author;
            oldValue.Edition = book.Edition;
            oldValue.YearOfEdition = book.YearOfEdition;
            oldValue.Price = book.Price;
            oldValue.JenreID = book.JenreID;
            
            db.SaveChanges();
        }

        static void DellReader(LibraryContext db, int id)
        {
            db.Readers.RemoveRange(db.Readers.Where(a => a.ID == id));
            db.SaveChanges();
        }
        static void DellBook(LibraryContext db, int id)
        {
            db.Books.RemoveRange(db.Books.Where(a => a.ID == id));
            db.SaveChanges();
        }
    }
}
