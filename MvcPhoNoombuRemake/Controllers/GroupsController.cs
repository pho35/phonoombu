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
    public class GroupsController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        //
        // GET: /Groups/

        public ActionResult Index()
        {
            LstTypeGroup lstTypeGroup = new LstTypeGroup();
            lstTypeGroup.groupsAll = db.Groups.ToList();
            lstTypeGroup.groupsPerso = db.Groups.Where(i => i.Owner.UserId == WebSecurity.CurrentUserId).ToList();
            lstTypeGroup.groupsJoin = new List<Groups>();

            // on parcourt l'ensemble du groupe
            foreach (var item in lstTypeGroup.groupsAll)
            {
                // on parcourt un groupe
                foreach (var item2 in item.Members)
                {
                    if (item2.UserId == WebSecurity.CurrentUserId)
                    {
                        lstTypeGroup.groupsJoin.Add(item);
                        break;
                    }
                }
            }

            return View(lstTypeGroup);
            //return View(db.Groups
            //    .Where(i => i.Owner.UserId == WebSecurity.CurrentUserId)
            //    .ToList());
        }

        //public ActionResult Remove()
        //{
        //    return View();
        //}
        //
        // GET: /Groups/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Groups groups = db.Groups.Find(id);

        //    if (groups == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(groups);
        //}

        public ActionResult Details(int id = 0)
        {
            LstMemberGroup lstMemberGroup = new LstMemberGroup();
            lstMemberGroup.groupsConcerned = db.Groups.Find(id);
            //on liste les abonnements suivi par l'utilisateur connecté
            lstMemberGroup.UserSuggest = db.Follows.Where(i => i.userfollower.UserId == WebSecurity.CurrentUserId).ToList();
            if (lstMemberGroup.groupsConcerned == null)
            {
                return HttpNotFound();
            }
            return View(lstMemberGroup);
        }

        public ActionResult LeaveGroup(int id = 0)
        {
            Groups groupsLeave = db.Groups.Find(id);
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            groupsLeave.Members.Remove(userCurrent);
            userCurrent.MyGroups.Remove(groupsLeave);
            db.SaveChanges();
            return RedirectToAction("Index", "Groups");
        }
        // pb à voir !!

        public ActionResult Remove(int id = 0, int pgid = 0)
        {
            User userToKick = db.Users.Find(pgid);
            Groups groupsConcerned = db.Groups.Find(id);
            groupsConcerned.Members.Remove(userToKick);
            userToKick.MyGroups.Remove(groupsConcerned);
            db.SaveChanges();
            return RedirectToAction("Details", "Groups", new { id = groupsConcerned.GroupsId });
        }

        public ActionResult Invite(int id = 0, int pgid = 0)
        {
            User userToJoin = db.Users.Find(pgid);
            Groups groupsConcerned = db.Groups.Find(id);
            groupsConcerned.Members.Add(userToJoin);
            userToJoin.MyGroups.Add(groupsConcerned);
            db.SaveChanges();
            return RedirectToAction("Details", "Groups", new { id = id });
        }

        // 
        // GET: /Groups/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Groups/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(new Groups()
                {
                    Members = new List<User>(),
                    GroupsName = groups.GroupsName,
                    Owner = db.Users.Find(WebSecurity.CurrentUserId)
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groups);
        }

        //
        // GET: /Groups/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Groups/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groups);
        }

        //
        // GET: /Groups/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Groups/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Groups groups = db.Groups.Find(id);

            //on doit supprimer le groupe à tous les membres
            foreach (var item in groups.Members)
            {
                User userMemberRemaining = db.Users.Find(item.UserId);
                userMemberRemaining.MyGroups.Remove(groups);
            }

            // on supprime le groupe dans la liste des groups du créateur
            User userCurrent = db.Users.Find(WebSecurity.CurrentUserId);
            userCurrent.MyGroups.Remove(groups);

            //on supprime le groupe dans la table Groups
            db.Groups.Remove(groups);
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