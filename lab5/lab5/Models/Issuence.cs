using System;
using System.Collections.Generic;
using System.Text;

namespace lab5.Models
{
    public class Issuence
    {
        public int ID { get; set; }
        public int? ReaderID { get; set; }
        public int? BookID { get; set; }
        public DateTime DateOfIssuance { get; set; }
        public DateTime DateOfReturn { get; set; }
        public bool Return { get; set; }

        public virtual Reader Reader { get; set; }
        public virtual Book Book { get; set; }
    }
}
