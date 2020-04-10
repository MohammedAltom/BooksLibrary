using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLibrary.Models.DbModels
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string BookURL { get; set; }
        public ApplicationUser EnterdBy { get; set; }
        
    }
}
