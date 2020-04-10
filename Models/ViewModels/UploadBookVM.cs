using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLibrary.Models.ViewModels
{
    public class UploadBookVM
    {
        [Required]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }
        [Required]
        [Display(Name = "Genre")]
        public string Genre { get; set; }
        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Pages")]
        public int Pages { get; set; }
        [Display(Name = "Book")]
        public IFormFile Book { get; set; }
    }
}
