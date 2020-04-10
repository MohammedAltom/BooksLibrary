using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BooksLibrary.Data;
using BooksLibrary.Models;
using BooksLibrary.Models.DbModels;
using BooksLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksLibrary.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment HostingEnviroment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext Dbcontext;

        public AdminController(IHostingEnvironment HostingEnviroment , UserManager<ApplicationUser> userManager , ApplicationDbContext DBcontext)
        {
            this.HostingEnviroment = HostingEnviroment;
            this.userManager = userManager;
            this.Dbcontext = DBcontext;
        }
        [HttpGet]
        public IActionResult UploadBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBook(UploadBookVM model)
        {
            if (ModelState.IsValid)
            {
                if(model.Book == null)
                {
                    return View();
                }
                string BookDir = Path.Combine(HostingEnviroment.WebRootPath, "Books");
                string UniqeName =  Guid.NewGuid().ToString() + "_" + model.Book.FileName;
                string BookUrl = Path.Combine(BookDir,UniqeName);
                model.Book.CopyTo(new FileStream(BookUrl, FileMode.Create));
                var user = await userManager.GetUserAsync(User);
                Book book = new Book()
                {
                    BookName = model.BookName,
                    Genre = model.Genre,
                    Author = model.Author,
                    BookURL= UniqeName,
                    Pages = model.Pages,
                    EnterdBy = user
                };
                Dbcontext.Books.Add(book);
                Dbcontext.SaveChanges();
                return View();
            }
            return View();
        }
    }
}