namespace Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        AboutMe = c.String(),
                        GenderID = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        UserProfilePic = c.String(),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genders", t => t.GenderID)
                .Index(t => t.GenderID);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        PictureTypeId = c.Int(nullable: false),
                        ArtistId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                        Description = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId)
                .ForeignKey("dbo.Genres", t => t.GenreId)
                .ForeignKey("dbo.PictureTypes", t => t.PictureTypeId)
                .Index(t => t.PictureTypeId)
                .Index(t => t.ArtistId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureId = c.Int(nullable: false),
                        Writer = c.String(),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Pic = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PictureTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "PictureTypeId", "dbo.PictureTypes");
            DropForeignKey("dbo.Pictures", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Comments", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Pictures", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.Artists", "GenderID", "dbo.Genders");
            DropIndex("dbo.Comments", new[] { "PictureId" });
            DropIndex("dbo.Pictures", new[] { "GenreId" });
            DropIndex("dbo.Pictures", new[] { "ArtistId" });
            DropIndex("dbo.Pictures", new[] { "PictureTypeId" });
            DropIndex("dbo.Artists", new[] { "GenderID" });
            DropTable("dbo.PictureTypes");
            DropTable("dbo.Genres");
            DropTable("dbo.Comments");
            DropTable("dbo.Pictures");
            DropTable("dbo.Genders");
            DropTable("dbo.Artists");
        }
    }
}
