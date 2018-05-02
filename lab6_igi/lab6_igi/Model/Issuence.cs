using System;
using System.ComponentModel.DataAnnotations;

namespace libr.Models
{
    public class Issuence
    {
        [Display(Name = "id")]
        public int ID { get; set; }
        [Display(Name = "readerID")]
        [Required(ErrorMessage = " Укажите читателя.")]
        public int? ReaderID { get; set; }
        [Display(Name = "bookID")]
        [Required(ErrorMessage = " Укажите книгу.")]
        public int? BookID { get; set; }
        [Display(Name = "dateOfIssuance")]
        [Required(ErrorMessage = " Укажите дату выдачи книги.")]
        public DateTime DateOfIssuance { get; set; }
        [Display(Name = "dateOfReturn")]
        public DateTime DateOfReturn { get; set; }
        [Display(Name = "return")]
        [Required(ErrorMessage = "Укажите была ли книга возвращена(true/false)")]
        public bool Return { get; set; }
        [Display(Name = "Readers")]
        public virtual Reader Reader { get; set; }
        [Display(Name = "Books")]
        public virtual Book Book { get; set; }
    }
}
