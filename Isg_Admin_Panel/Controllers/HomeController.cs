using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Isg_Admin_Panel.Models;
using Microsoft.AspNetCore.Http;


namespace Isg_Admin_Panel.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BlogContext _context;
        public HomeController(ILogger<HomeController> logger, BlogContext context )
        {
            _logger = logger;
            _context=context;
        }

        //Yazarların girişlerini denetleyen method.
        public IActionResult Login_Controller (string Email, string Password){

                var author=_context.Author.FirstOrDefault(author_info =>author_info.Email==Email && author_info.Password==Password );

                if (author==null)
                {
                    return RedirectToAction(nameof(Index));
                }

             HttpContext.Session.SetInt32("id",author.Id);

                return RedirectToAction(nameof(Author));
        }

        //yazarları listeleyip görüntülememizi sağlayan method.
        public IActionResult Author()
        {
            List<Author> list=_context.Author.ToList();
            return View(list);
        }

        //Yazarların detaylarını çekip json olarak geri döndürmemizi sağlayan method.
        public async Task<IActionResult> AuthorDetails(int Id)
        {
            var author = await _context.Author.FindAsync(Id);
            return Json(author);
        }

        //Sesionu temizleyip yazarların çıkışını sağlayan method.
        public IActionResult LogOut(){

            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));

        }

        ///Yazar eklememizi sağlayan method.
         public async Task<IActionResult> AddAuthor(Author author)
        {
            if (author.Id == 0)
            {
                await _context.AddAsync(author);
            }
            else
            {
                _context.Update(author);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Author));
        }
        

        //Yazar silmemizi sağlayan method.
        public async Task<IActionResult> DeleteAuthor(int Id)
        {
            
            
            var author = await _context.Author.FindAsync(Id);
            _context.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Author));
            
           
        }

        private IActionResult JavaScript(string script)
        {
            throw new NotImplementedException();
        }

        public IActionResult Index()
        {
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
