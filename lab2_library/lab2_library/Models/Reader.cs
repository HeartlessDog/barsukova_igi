using System;
using System.Collections.Generic;
using System.Text;

namespace libr.Models
{
    public class Reader
    {
        public int ID { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Sex { get; set; }
        public String Adres { get; set; }
        public String PhoneNumber { get; set; }
        public String PassportData { get; set; }
    }
}
