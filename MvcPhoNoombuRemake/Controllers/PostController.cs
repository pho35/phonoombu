using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPhoNoombuRemake.DAL;
using MvcPhoNoombuRemake.Models;
using WebMatrix.WebData;

namespace MvcPhoNoombuRemake.Controllers
{
    public class PostController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        //
        // GET: /Post/

        public ActionResult Index()
        {
            LstPostModel viewLstPost = new LstPostModel();
            viewLstPost.Posts = db.Posts
                .Where(i => i.Author.UserId == WebSecurity.CurrentUserId)
                .OrderByDescending(i => i.DatePublication)
                .ToList();
            viewLstPost.Likes = db.Likes.Where(i => i.LikeFromUser.UserId == WebSecurity.CurrentUserId).ToList();
            return View(viewLstPost);
        }

        //
        // GET: /Post/Details/5

        public ActionResult Details(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Post/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //
        // GET: /Post/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            //on effacera tous les comment publié sur le post à supprimer dans la liste des coms des utilisateurs
            foreach (var item in post.Comments)
            {
                User userComment = db.Users.Find(item.CommentAuthor.UserId);
                userComment.MyLstComment.Remove(item);
            }

            //idem pour les likes
            foreach (var itemlike in post.Likes)
            {
                User userLike = db.Users.Find(itemlike.LikeFromUser.UserId);
                userLike.MyLstLike.Remove(itemlike);
            }

            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}