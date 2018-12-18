using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToolsForEver.Models;

namespace ToolsForEver.Controllers
{
    [Authorize(Roles = "Employee")]
    public class SuppliesController : Controller
    {
        private teunstah_toolsforeverEntities db = new teunstah_toolsforeverEntities();

        // GET: Supplies
        public ActionResult Index(int? id, string searchLocation, int? searchProduct)
        {
            var supplies = db.Supplies.Include(s => s.Location).Include(s => s.Product);

            if (id != null)
            {
                supplies = db.Supplies.Include(s => s.Location).Include(s => s.Product).Where(s => s.ProductId == id);
            }

            if (!string.IsNullOrEmpty(searchLocation)){
                supplies = db.Supplies.Include(s => s.Location).Include(s => s.Product).Where(f => f.Location.Name.Contains(searchLocation));
            }

            //Ticket 104
            if (searchProduct != null)
            {
                supplies = db.Supplies.Where(x => x.Amount < searchProduct);
            }

            return View(supplies.ToList());
        }

        // GET: Supplies/Details/5
        public ActionResult Details(int? locationId, int? productId)
        {
            if (locationId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(locationId, productId);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // GET: Supplies/Create
        public ActionResult Create()
        {
            ViewBag.LocatieId = new SelectList(db.Locations, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocatieId,ProductId,Amount")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Supplies.Add(supply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocatieId = new SelectList(db.Locations, "Id", "Name", supply.LocatieId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", supply.ProductId);
            return View(supply);
        }

        // GET: Supplies/Edit/5
        public ActionResult Edit(int? locationId, int? productId)
        {
            if (locationId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(locationId, productId);
            if (supply == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocatieId = new SelectList(db.Locations, "Id", "Name", supply.LocatieId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", supply.ProductId);
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocatieId,ProductId,Amount")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocatieId = new SelectList(db.Locations, "Id", "Name", supply.LocatieId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", supply.ProductId);
            return View(supply);
        }

        // GET: Supplies/Delete/5
        public ActionResult Delete(int? locationId, int? productId)
        {
            if (locationId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(locationId, productId);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int locationId, int productId)
        {
            Supply supply = db.Supplies.Find(locationId, productId);
            db.Supplies.Remove(supply);
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
