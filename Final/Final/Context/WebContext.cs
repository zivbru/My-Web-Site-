using Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Context
{
    public class WebContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PictureType> PictureTypes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Genre> Genres { get; set; }

        static WebContext()
        {
            Database.SetInitializer(new WebContextInitiailizer());
        }











        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Artist_Configuration

            modelBuilder.Entity<Artist>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Artist>()
               .Property(a => a.Name)
               .HasMaxLength(25);

            modelBuilder.Entity<Artist>()
               .HasRequired(a => a.Gender)
               .WithMany(g => g.Artists)
               .HasForeignKey(a => a.GenderID)
               .WillCascadeOnDelete(false);


            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Pictures);


            #endregion

            #region Picture_Configuration

            modelBuilder.Entity<Picture>()
               .HasKey(p => p.Id);

            modelBuilder.Entity<Picture>()
               .HasRequired(p => p.PictureType)
               .WithMany(pt => pt.Pictures)
               .HasForeignKey(p => p.PictureTypeId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Picture>()
               .HasRequired(p => p.Artist)
               .WithMany(a => a.Pictures)
               .HasForeignKey(p => p.ArtistId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Picture>()
               .HasRequired(p => p.Genre)
               .WithMany(g => g.Pictures)
               .HasForeignKey(p => p.GenreId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Picture>()
                .HasMany(p => p.Comments);

            #endregion

            #region Comment_Configuration

            modelBuilder.Entity<Comment>()
              .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
              .HasRequired(c => c.Picture)
              .WithMany(p => p.Comments)
              .HasForeignKey(c => c.PictureId)
              .WillCascadeOnDelete(false);

            #endregion

            #region PictureType_Configuration

            modelBuilder.Entity<PictureType>()
              .HasKey(pt => pt.Id);

            modelBuilder.Entity<PictureType>()
              .HasMany(pt => pt.Pictures);

            #endregion

            #region Gender_Configuration

            modelBuilder.Entity<Gender>()
             .HasKey(g => g.Id);

            modelBuilder.Entity<Gender>()
              .HasMany(g => g.Artists);

            #endregion

            #region Genre_Configuration

            modelBuilder.Entity<Genre>()
             .HasKey(g => g.Id);

            modelBuilder.Entity<Genre>()
              .HasMany(g => g.Pictures);

            #endregion
        }
    }
}