﻿using Microsoft.EntityFrameworkCore;

namespace Reclone_BackEnd.Models
{
    public class PostDbContext : DbContext
    {
        
        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
           
        }

        //Enteties
      public  DbSet<Image> Image { get; set; }
      public  DbSet<Comment> Comment { get; set; }


    }
}
