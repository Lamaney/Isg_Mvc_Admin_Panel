using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Isg_Admin_Panel.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Isg_Admin_Panel.Filter;

namespace Isg_Admin_Panel.Controllers
{
    //Çıkış yapıldığında tekrardan izinsiz erişimlerin oluşmaması için oluşturulan classın bağlantısı.
    [UserFilter]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;

        private readonly BlogContext _context;
        public BlogController(ILogger<BlogController> logger, BlogContext context )
        {
            _logger = logger;
            _context=context;
        }

        public async Task<IActionResult>Save(Blog model){

            if (model!=null)
            {
               
                model.AuthorId=(int)HttpContext.Session.GetInt32("id");

                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);

            }

            return Json(false);
        }

        public async Task<IActionResult> DeleteBlog(int Id)
        {
            
            
            var blog = await _context.Blog.FindAsync(Id);
            _context.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(blog));
            
           
        }


        public IActionResult Index()
        {
            var list=_context.Blog.ToList();
            return View(list);
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Publish(int Id)
        {
            var blog=_context.Blog.Find(Id);
            blog.Is_Publish=true;
            _context.Update(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
       
    public IActionResult UnPublish(int Id)
        {
            var blog=_context.Blog.Find(Id);
            blog.Is_Publish=false;
            _context.Update(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
