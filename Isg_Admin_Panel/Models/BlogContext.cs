using System;
using Microsoft.EntityFrameworkCore;


namespace Isg_Admin_Panel.Models{
    public class BlogContext :DbContext{

        public BlogContext(DbContextOptions<BlogContext> options): base(options){}

        public DbSet<Blog>Blog{get;set;}

        public DbSet<Author>Author{get;set;}

        

    }


}