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
    public class ArtistController : Controller
    {
        private WebContext db = new WebContext();

        // GET: /Artist/
        public ActionResult Index()
        {
            var artists = db.Artists.Include(a => a.Gender);
            return View(artists.ToList());
        }

        // GET: /Artist/
        public ActionResult Login()
        {
            return View(new UserAccount());
        }

       

        public ActionResult Search(string SearchCity, string searchString, int? age)
        {

            var artists = from m in db.Artists
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(s => s.Name.Contains(searchString));

            }
            if (!string.IsNullOrEmpty(SearchCity))
            {
                artists = artists.Where(x => x.City.Contains(SearchCity));
            }
            if (age >= 1)
            {
                artists = artists.Where(x => x.Age >= age);
            }

            return View("Index", artists);
        }
        
        

        public ActionResult FindPrice(int ? price)
        {
            var artists = from m in db.Artists select m;
            if (price >= 1)
            {

                var query = from artist in db.Artists
                            join picture in db.Pictures on artist.Id equals picture.ArtistId
                            where picture.Price > price
                            select artist;

                return View("Index", query.Distinct());
            }
            return View("index", artists);
        }

        // GET: /Artist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: /Artist/Create
        public ActionResult Create()
        {
            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name");
            return View();
        }

        // POST: /Artist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,AboutMe,GenderID,Age,Address,City,Mail,UserProfilePic")] Artist artist, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        artist.UserProfilePic = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Pictures/"), artist.UserProfilePic));
                    }
                }
                if(artist.UserProfilePic==null)
                {
                    artist.UserProfilePic = "avatar_2x.png";
                }
                artist.Name = User.Identity.Name;
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name", artist.GenderID);
            return View(artist);
        }


        // GET: /Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name", artist.GenderID);
            return View(artist);
        }

        // POST: /Artist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,AboutMe,GenderID,Age,Address,City,Mail")] Artist artist, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        artist.UserProfilePic = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Pictures/"), artist.UserProfilePic));
                    }
                }
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name", artist.GenderID);
            return View(artist);
        }

        // GET: /Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: /Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            var pictures = (from picture in db.Pictures
                            where picture.ArtistId == artist.Id
                            select picture).ToList();
            foreach (var p in pictures)
            {
                var comments = (from comment in db.Comments
                                where comment.PictureId == p.Id
                                select comment).ToList();
                foreach (var c in comments)
                {
                    db.Comments.Remove(c);
                }

                db.Pictures.Remove(p);
            }
            db.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult getAddress()
        {
            List<String> address = new List<string>();
            string fullAddr;
            var artists = from s in db.Artists select s;
            foreach (var artist in artists)
            {
                if (artist.Address != null)
                {
                    fullAddr = artist.City + " " + artist.Address;
                    address.Add(fullAddr);
                }
            }

            return Json(address, JsonRequestBehavior.AllowGet);
        }

       


        public JsonResult getArtistAndPic()
        {
            
            List<PieChart> ArtistAndPic = new List<PieChart>();
            
            var artists = (from s in db.Artists select s );
            var random = new Random();
            foreach (var artist in artists)
            {
                
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                var highlight = String.Format("#{0:X6}", random.Next(0x1000000));
                
                if (artist.Pictures.Count() > 0)
                {
                    PieChart pie = new PieChart();

                    pie.label = artist.Name;
                    pie.value = artist.Pictures.Count();
                    pie.color = color;
                    pie.highlight = highlight;
                    
                    ArtistAndPic.Add(pie);

                   
                }
            }
            

            return Json(ArtistAndPic.ToList(), JsonRequestBehavior.AllowGet);
        }

         public JsonResult getArtistAndGender()
        {
            RadarChart radar = new RadarChart();
            RAdarDataSet dataSet = new RAdarDataSet();
            List<RadarChart> radars = new List<RadarChart>();
             List<String> labels = new List<string>();
            List<int> intset = new List<int>();

             var pictures = (from pic in db.Pictures select pic );

             foreach (var picture in pictures){
                 if (picture.Genre.Name != null)
                 {
                      
                     if (!labels.Contains(picture.Genre.Name))
                     {
                         labels.Add(picture.Genre.Name);
                     }
                 }
             }
             dataSet.labels = labels;

             radar.label = "My First dataset";
             radar.fillColor = "rgba(151,187,205,0.2)";
             radar.strokeColor = "rgba(151,187,205,1)";
             radar.pointColor = "rgba(151,187,205,1)";
             radar.pointHighlightFill = "#fff";
             radar.pointStrokeColor = "#fff";
             radar.pointHighlightStroke = "rgba(151,187,205,1)";

             foreach (var genre in dataSet.labels)
             {
                 var count = (from gen in db.Pictures 
                              where gen.Genre.Name.Equals(genre)
                                  select gen).Count();
                 intset.Add(count);
             }

             radar.data = intset;
             radars.Add(radar);
             dataSet.datasets = radars;

            return Json(dataSet, JsonRequestBehavior.AllowGet);
        }
    }
}
