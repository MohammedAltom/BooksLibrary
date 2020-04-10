using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BooksLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using BooksLibrary.Data;
using BooksLibrary.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BooksLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext Dbcontext;
        private readonly IHostingEnvironment HostngEnviroment;

        public HomeController(ApplicationDbContext DBcontext , IHostingEnvironment HostngEnviroment)
        {
            this.Dbcontext = DBcontext;
            this.HostngEnviroment = HostngEnviroment;
        }

        [HttpGet]
        public IActionResult Index(string Genre)
        {
            IEnumerable<Book> Books;
            if (!string.IsNullOrEmpty(Genre))
            {
                Books = Dbcontext.Books.AsEnumerable().Where(b => b.Genre == Genre);
                return View(Books);
            }
            Books = Dbcontext.Books.AsEnumerable();
            return View(Books);
        }
        [HttpPost]
        public IActionResult Index(IFormCollection formFields)
        {
            IEnumerable<Book> Books;
            String BookName =  formFields["BookName"];
            if (!string.IsNullOrEmpty(BookName))
            {
                Books = Dbcontext.Books.AsEnumerable().Where(b => b.BookName == BookName);
                return View(Books);
            }
            else
            {
                Books = Dbcontext.Books.AsEnumerable();
                return View(Books);
            }
        }

            public IActionResult ShowBook(string url)
        {
            Book book = Dbcontext.Books.FirstOrDefault(b => b.BookURL == url);
            string BookUrl = "/Books/" + url;
            book.BookURL = BookUrl;
            return View(book);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
