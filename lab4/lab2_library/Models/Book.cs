using System;
using System.Collections.Generic;
using System.Text;

namespace libr.Models
{
    public class Book
    {
        public int ID { get; set; }
        public int RegistrationNumber { get; set; }
        public String Name { get; set; }
        public String Author { get; set; }
        public String Edition { get; set; }
        public int YearOfEdition { get; set; }
        public int? JenreID { get; set; }
        public float Price { get; set; }

        public virtual Jenre Jenre { get; set; }
    }
}
