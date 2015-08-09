using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPhoNoombuRemake.Filters;
using MvcPhoNoombuRemake.Models;
using WebMatrix.WebData;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Controllers
{
    public class HomeController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        [AuthorizeEx]
        public ActionResult Index()
        {
            LstMyNewsModel viewLstMyNew = new LstMyNewsModel();
            viewLstMyNew.MyNews = new List<Post>();
            List<Follow> MyFollow = new List<Follow>();
            List<Post> PostsUserFollowed = new List<Post>();

            //on récupère la liste de likes mis par l'utilisateur connecté 
            viewLstMyNew.Likes = db.Likes.Where(i => i.LikeFromUser.UserId == WebSecurity.CurrentUserId).ToList();

            // on récupère la liste des utilisateur suivi de l'utilisateur connecté 
            MyFollow = db.Follows
                       .Where(i => i.userfollower.UserId == WebSecurity.CurrentUserId)
                       .ToList();
            foreach (Follow unUserFollowed in MyFollow)
            {
                PostsUserFollowed = db.Posts
                                        .Where(i => i.Author.UserId == unUserFollowed.userfollowed.UserId)
                                        .ToList();
                foreach (Post unPost in PostsUserFollowed)
                {
                    if (unPost != null)
                    {
                        viewLstMyNew.MyNews.Add(unPost);
                    }
                    else
                    {
                        break;
                    }

                }
                PostsUserFollowed.Clear();
            }

            // on liste nos news personnel
            List<Post> PersonalPost = new List<Post>();
            PersonalPost = db.Posts
                            .Where(i => i.Author.UserId == WebSecurity.CurrentUserId)
                            .ToList();
            foreach(Post unPostPerso in PersonalPost)
            {
                if (unPostPerso != null)
                {
                    viewLstMyNew.MyNews.Add(unPostPerso);
                }
                else
                {
                    break;
                }
                
            }
            PersonalPost.Clear();
            viewLstMyNew.MyNews.OrderByDescending(i => i.DatePublication);
            return View(viewLstMyNew);

        }

        public ActionResult DetailPostComments(int id = 0)
        {
            LstCommentsByPost myLstCommentsByPost = new LstCommentsByPost();
            myLstCommentsByPost.postConcerned = db.Posts.Find(id);
            if (myLstCommentsByPost.postConcerned == null)
            {
                return HttpNotFound();
            }
            return View(myLstCommentsByPost);
        }

        [HttpPost]
        public ActionResult DetailPostComments(LstCommentsByPost lstCommentsByPost, int id)
        {
            if (ModelState.IsValid)
            {
                User userAuthor = db.Users.Find(WebSecurity.CurrentUserId);
                Post postConcerned = db.Posts.Find(id);
                Comment newComs = new Comment()
                {
                    CommentAuthor = userAuthor,
                    CommentContent = lstCommentsByPost.unComment.CommentContent,
                    PostConcerned = postConcerned,
                    DatePublication = DateTime.Today
                };
                db.Comments.Add(newComs);
                userAuthor.MyLstComment.Add(newComs);
                postConcerned.Comments.Add(newComs);
                db.SaveChanges();
                return RedirectToAction("DetailPostComments", "Home", new { id = postConcerned.PostId });
            }
            else
            {
                return View(lstCommentsByPost);
            }
        }

        [HttpPost]
        public ActionResult Index(LstMyNewsModel post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(new Post()
                {
                    ContentPost = post.ContentPost,
                    DatePublication = DateTime.Today,
                    Author = db.Users.Find(WebSecurity.CurrentUserId),
                    Likes = new List<Like>(),
                    Comments = new List<Comment>()
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult LstUser(string critere)
        {
            LstUserModel viewLstUser = new LstUserModel();
            viewLstUser.Users = db.Users.Where(i => i.UserName.StartsWith(critere)).ToList();
            viewLstUser.Follows = db.Follows.Where(i => i.userfollower.UserId == WebSecurity.CurrentUserId).ToList();
            return View(viewLstUser);
        }

        public ActionResult Follow(int id = 0)
        {
            User userAdd = db.Users.Find(id);
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            Follow newfollow = new Follow()
            {
                userfollower = userCurrent,
                userfollowed = userAdd
            };
            userCurrent.MyFollow.Add(newfollow);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyFollow()
        {
            LstFollowModel myLstFollowModel = new LstFollowModel();

            //Liste des utilisateurs qui vous suivent 
            myLstFollowModel.LstUserFollowed = new List<Follow>();
            myLstFollowModel.LstUserFollowed = db.Follows
                                                 .Where(i => i.userfollowed.UserId == WebSecurity.CurrentUserId)
                                                 .ToList();

            //Liste des utilisateurs que vous suiviez
            myLstFollowModel.LstUserFollower = new List<Follow>();
            myLstFollowModel.LstUserFollower = db.Follows
                                                .Where(i => i.userfollower.UserId == WebSecurity.CurrentUserId)
                                                .ToList();

            return View(myLstFollowModel);
        }

        public ActionResult UnFollow(int id = 0)
        {
            Follow follow = db.Follows.Find(id);
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            userCurrent.MyFollow.Remove(follow);
            db.Follows.Remove(follow);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CommentDelete(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            Post postconcerned = comment.PostConcerned;
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            userCurrent.MyLstComment.Remove(comment);
            postconcerned.Comments.Remove(comment);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("DetailPostComments", "Home", new { id = postconcerned.PostId });

        }
        public ActionResult Rechercher()
        {
            return View();
        }

        public ActionResult Liked(int id = 0)
        {
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            Post postcurrent = db.Posts.Find(id);
            Like newlike = new Like()
            {
                LikeFromUser = userCurrent,
                LikeForPost = postcurrent
            };
            db.Likes.Add(newlike);
            postcurrent.Likes.Add(newlike);
            userCurrent.MyLstLike.Add(newlike);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DisLike(int id = 0)
        {
            //trouver id like
            List<Like> AllLstLike = db.Likes.Where(i => i.LikeForPost.PostId == id).ToList();
            int IdLike = 0;
            bool researchIdFound = false;
            foreach (var item in AllLstLike)
            {
                if (item.LikeForPost.PostId == id)
                {
                    researchIdFound = true;
                    IdLike = item.LikeId;
                    break;
                }
            }
            if (researchIdFound == true)
            {
                Like like = db.Likes.Find(IdLike);
                User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
                userCurrent.MyLstLike.Remove(like);
                db.Likes.Remove(like);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

        }

    }
}
