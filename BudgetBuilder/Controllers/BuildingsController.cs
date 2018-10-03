using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetBuilder.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace BudgetBuilder.Controllers
{
    public class BuildingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult List(BuildingRequestModel request)
        {
            // Store current User Id
            string userId = request.UserID;
     
            // Select Buildings where foreign key is equal to current User Id
            var buildings = db.Buildings.Where(fk => fk.ApplicationUserID == userId);
    
            return Json(new { Buildings = buildings });
        }

        [HttpPost]
        public ActionResult Details(BuildingRequestModel request)
        {
            if (request.BuildingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Building building = db.Buildings.Find(request.BuildingID);
            if (building != null)
            {
                return Json(new { Building = building });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Create(Building request)
        {
            bool success = false;

            if (ModelState.IsValid)
            {
                DateTime timestamp = DateTime.Now;
                request.DateAdded = timestamp;
                request.DateModified = timestamp;

                db.Buildings.Add(request);
                db.SaveChanges();
                success = true;

                Building building = db.Buildings.Find(request.BuildingID);

                return Json(new { Success = success, Building = building });
            }

            return Json(new { Success = success });
        }

        [HttpPost]
        public ActionResult Update(Building request)
        {
            if (ModelState.IsValid)
            {

                request.DateModified = DateTime.Now;

                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();

                Building building = db.Buildings.Find(request.BuildingID);

                return Json(new { Success = true, Building = building });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(BuildingRequestModel request)
        {
            Building building = db.Buildings.Find(request.BuildingID);
            db.Buildings.Remove(building);
            db.SaveChanges();
            return Json(new { Success = true } );
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
