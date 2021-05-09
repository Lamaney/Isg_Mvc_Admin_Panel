using System;
using Microsoft.EntityFrameworkCore;


namespace Isg_Admin_Panel.Models{
    public class PageContext :DbContext{

        public PageContext(DbContextOptions<PageContext> options): base(options){}

        public DbSet<Content>Content{get;set;}

        public DbSet<Author>Author{get;set;}

        

    }


}