using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab5.Models
{
    public class ReaderComparer : IComparer<Reader>
    {
        public int Compare(Reader x, Reader y)
        {
            if (String.Compare(x.FullName, y.FullName) == 1)
                return 1;
            else if (String.Compare(x.FullName, y.FullName) == -1)
                return -1;
            else
                return x.DateOfBirth.CompareTo(y.DateOfBirth);
        }
    }
    public class IssuenceComparer : IComparer<Issuence>
    {
        public int Compare(Issuence x, Issuence y)
        {
            if (String.Compare(x.Reader.FullName, y.Reader.FullName) == 1)
                return 1;
            else if (String.Compare(x.Reader.FullName, y.Reader.FullName) == -1)
                return -1;
            else
                return x.DateOfIssuance.CompareTo(y.DateOfIssuance);
        }
    }
    public class BookComparer : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            if (String.Compare(x.Name, y.Name) == 1)
                return 1;
            else if (String.Compare(x.Name, y.Name) == -1)
                return -1;
            else
                return x.YearOfEdition.CompareTo(y.YearOfEdition);
        }
    }
}
