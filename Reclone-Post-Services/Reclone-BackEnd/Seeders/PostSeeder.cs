using Reclone_BackEnd.Models;

namespace Reclone_BackEnd.Seeders
{
    public class PostSeeder
    {
        private readonly PostDbContext _context;


        public PostSeeder(PostDbContext context)
        {
            _context = context;
        }

        public void SeedPostDB()
        {
            if (_context.Image.Any())
            {
                return; // Database has already been seeded
            }


            _context.Database.EnsureCreated();



            var images = new List<Image>
            {
                new Image
                {
                    
                    PublicId = "PublicID1",
                    UserId = "UserID1",
                    Caption = "My first post",
                    Tag = "#selfie",
                    likes = 33

                },
                new Image
                {
                   
                    PublicId = "PublicID2",
                    UserId = "UserID1",
                    Caption = "My Second post",
                    Tag = "#selfie",
                    likes = 23

                },
                new Image
                {

                    PublicId = "PublicID3",
                    UserId = "UserID2",
                    Caption = "New account first video post",
                    Tag = "#selfie",
                    likes = 53,
                    URL = "https://www.youtube.com/watch?v=USLl2YFdxjM&ab_channel=SereBoxing"

                }

            };
            _context.AddRange(images);
            _context.SaveChanges();


         }
           
       }

    }

