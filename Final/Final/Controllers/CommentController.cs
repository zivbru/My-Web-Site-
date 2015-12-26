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

namespace Final.Controllers
{
    public class CommentController : Controller
    {
        private WebContext db = new WebContext();

        // GET: /Comment/
        public ActionResult Index(Comment comment)
        {

            var comments = db.Comments.Where(s => s.PictureId.Equals(comment.PictureId));
            return View(comments.ToList());
        }
        public ActionResult Search(string Text)
        {

            var comments = from m in db.Comments
                           select m;

            if (!String.IsNullOrEmpty(Text))
            {
                comments = comments.Where(s => s.Text.Contains(Text));
            }
            return View("Index", comments);
        }
        // GET: /Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.Comments.Where(x => x.PictureId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View("Index", comment);
        }

        // GET: /Comment/Create
        public ActionResult Create()
        {
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "FileName");
            return View();
        }
        public ActionResult AddComment(int id)
        {
            var Comment = new Comment
            {
                PictureId = id,
                Date = DateTime.Now
            };

            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "FileName");

            return View("Create", Comment);
        }

        // POST: /Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PictureId,Writer,Text,Date,Pic")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Pic = (from a in db.Artists
                               where a.Name.Equals(User.Identity.Name)
                               select a.UserProfilePic).FirstOrDefault();
                comment.Writer = User.Identity.Name;
                comment.Date = DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("Index", comment);
            }

            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "FileName", comment.PictureId);
            return View("Index", comment);
        }

        // GET: /Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "FileName", comment.PictureId);
            return View(comment);
        }

        // POST: /Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PictureId,Picture,Writer,Text,Pic")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                comment.Writer = User.Identity.Name;
                comment.Pic = (from a in db.Artists
                               where a.Name.Equals(User.Identity.Name)
                               select a.UserProfilePic).FirstOrDefault();
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {id = comment.PictureId });
            }
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "FileName", comment.PictureId);
            return View("Artist");
        }

        // GET: /Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: /Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            var tempId = comment.PictureId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new {id = tempId });
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

