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


namespace Isg_Admin_Panel.Controllers
{
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
                var file=Request.Form.Files.First();
                //CC:\Users\Omer\github repos\Isg_Mvc_Admin_Panel\Isg_Admin_Panel\wwwroot
                string savePath=Path.Combine("C:","Users","Omer","github repos","Isg_Mvc_Admin_Panel","Isg_Admin_Panel","wwwroot","img");
                var fileName=$"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
                var fileUrl=Path.Combine(savePath,fileName);
                
                using(var filestream=new FileStream(fileUrl,FileMode.Create)){
                    await file.CopyToAsync(filestream);
                }

                model.Image_Path=fileName;
               
                model.AuthorId=(int)HttpContext.Session.GetInt32("id");

                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);

            }

            return Json(false);
        }



        public IActionResult Index()
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