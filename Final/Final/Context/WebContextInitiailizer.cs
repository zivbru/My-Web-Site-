using Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Context
{
    public class WebContextInitiailizer : DropCreateDatabaseIfModelChanges<WebContext>//DropCreateDatabaseAlways<WebContext>
    {
        protected override void Seed(WebContext context)
        {
            var genders = new List<Gender>()
            {
                new Gender
                {
                    Name = "Female"
                },
                new Gender
                {
                    Name = "Male"
                }
            };

            genders.ForEach(g => context.Genders.Add(g));


            var geners = new List<Genre>()
            {
                new Genre
                {
                    Name = "Action"
                },
                new Genre
                {
                    Name = "Drama"
                },
                new Genre
                {
                    Name = "Fashion"
                },
                new Genre
                {
                    Name = "Art"
                },
                new Genre
                {
                    Name = "Sport"
                }
            };

            geners.ForEach(g => context.Genres.Add(g));

            var pictureType = new List<PictureType>()
            {
                new PictureType
                {
                    Name = "image"
                },
                new PictureType
                {
                    Name = "Pmg"
                }
            };
            pictureType.ForEach(g => context.PictureTypes.Add(g));
            context.SaveChanges();
        }
    }
}