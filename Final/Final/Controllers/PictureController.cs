using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Final.Context;
using System.IO;

namespace Final.Controllers
{
    public class PictureController : Controller
    {
        private WebContext db = new WebContext();

        // GET: /Picture/
        public ActionResult Index()
        {
            var pictures = db.Pictures.Include(p => p.Artist).Include(p => p.Genre).Include(p => p.PictureType);
            return View(pictures.ToList());
        }

        public ActionResult Search(string Name , string SearchGenre, int? Price = 0)
        {
           
            var pictures = from m in db.Pictures
                           select m;

            if (!string.IsNullOrEmpty(SearchGenre))
            {
                pictures = pictures.Where(x => x.Genre.Name.ToLower().Contains(SearchGenre.ToLower()));
            }


            if (!string.IsNullOrEmpty(Name))
            {
                pictures = pictures.Where(x => x.FileName.ToLower().Substring(0,x.FileName.Length - 4).Contains(Name.ToLower()));
            }


            if(Price != 0)
            {
                pictures = pictures.Where(x => x.Price >= Price);
            }

            return View("Index", pictures);
        }

        public ActionResult The3MostPopularGenres()
        {
            var top3 = db.Pictures
                .GroupBy(q => q.GenreId)
                .OrderByDescending(group => group.Count())
                //.Select(g => g.Key);
                .Take(3)
                .Select(x => x.Key);
            //.ToList();
            var groups = from p in db.Pictures
                         where top3.Contains(p.GenreId)
                         select p;


            return View("index", groups);

        }
        // GET: /Picture/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var picture = db.Pictures.Where(x => x.ArtistId == id);

            if (picture == null)
            {
                return HttpNotFound();
            }
            return View("Index", picture);
        }

        public ActionResult PictureDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var picture = db.Pictures.Where(x => x.Id == id);

            if (picture == null)
            {
                return HttpNotFound();
            }
            return View("Index", picture);
        }

        // GET: /Picture/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
            ViewBag.PictureTypeId = new SelectList(db.PictureTypes, "Id", "Name");
            return View();
        }

        // GET: /Picture/AddPicture
        public ActionResult AddPicture(int id)
        {
            var picture = new Picture
            {
                ArtistId = id,
                PictureTypeId = (int)EPictureType.Image
            };

            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
            ViewBag.PictureTypeId = new SelectList(db.PictureTypes, "Id", "Name");

            return View("Create", picture);
        }

        // POST: /Picture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileName,PictureTypeId,ArtistId,Price,GenreId,Description")] Picture picture, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        picture.FileName = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Pictures/"), picture.FileName));
                    }
                }
                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", picture.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", picture.GenreId);
            ViewBag.PictureTypeId = new SelectList(db.PictureTypes, "Id", "Name", picture.PictureTypeId);
            return View(picture);
        }

        // GET: /Picture/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", picture.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", picture.GenreId);
            ViewBag.PictureTypeId = new SelectList(db.PictureTypes, "Id", "Name", picture.PictureTypeId);
            return View(picture);
        }

        // POST: /Picture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName,PictureTypeId,ArtistId,Price,GenreId,Description")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", picture.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", picture.GenreId);
            ViewBag.PictureTypeId = new SelectList(db.PictureTypes, "Id", "Name", picture.PictureTypeId);
            return View(picture);
        }

        // GET: /Picture/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: /Picture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            var comments = (from comment in db.Comments
                            where comment.PictureId == picture.Id
                            select comment).ToList();
            foreach (var c in comments)
            {
                db.Comments.Remove(c);
            }

            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FindDate(string Date)
        {
            DateTime date = Convert.ToDateTime(Date);
            var query = from picture in db.Pictures
                        join comment in db.Comments on picture.Id equals comment.PictureId
                        where DateTime.Compare(comment.Date, date) == 1
                        select picture;
            return View("Index", query.Distinct());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}