﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Budget_Portal.Extensions;
using Budget_Portal.Helpers;
using Budget_Portal.Models;
using Microsoft.AspNet.Identity;

namespace Budget_Portal.Controllers
{
    public class HouseHoldsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: HouseHolds
        public ActionResult Index()
        {
            return View(db.HouseHolds.ToList());
        }

        // GET: HouseHolds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HouseHolds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdName,Greeting")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                houseHold.Created = DateTime.Now;
                db.HouseHolds.Add(houseHold);
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseholdId = houseHold.Id;
                roleHelper.UpdateUserRole(user.Id, "Head");
                db.SaveChanges();

                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);

                return RedirectToAction("Index");
            }

            return View(houseHold);
        }

        // GET: HouseHolds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdName,Greeting,Created,IsDeleted")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(houseHold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseHold houseHold = db.HouseHolds.Find(id);
            db.HouseHolds.Remove(houseHold);
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
    }
}
