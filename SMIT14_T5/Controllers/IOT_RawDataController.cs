using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMIT14_T5.Models;

namespace SMIT14_T5.Controllers
{
    public class IOT_RawDataController : Controller
    {
        private TEST_weatherEntities db = new TEST_weatherEntities();

        // GET: IOT_RawData
        public ActionResult Index()
        {
            return View(db.IOT_RawData.ToList());
        }

        // GET: IOT_RawData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IOT_RawData iOT_RawData = db.IOT_RawData.Find(id);
            if (iOT_RawData == null)
            {
                return HttpNotFound();
            }
            return View(iOT_RawData);
        }

        // GET: IOT_RawData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IOT_RawData/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IOT_Data_ID,DataTime,Temperature,Humidity,IOT_name")] IOT_RawData iOT_RawData)
        {
            if (ModelState.IsValid)
            {
                db.IOT_RawData.Add(iOT_RawData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iOT_RawData);
        }

        // GET: IOT_RawData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IOT_RawData iOT_RawData = db.IOT_RawData.Find(id);
            if (iOT_RawData == null)
            {
                return HttpNotFound();
            }
            return View(iOT_RawData);
        }

        // POST: IOT_RawData/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IOT_Data_ID,DataTime,Temperature,Humidity,IOT_name")] IOT_RawData iOT_RawData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iOT_RawData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iOT_RawData);
        }

        // GET: IOT_RawData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IOT_RawData iOT_RawData = db.IOT_RawData.Find(id);
            if (iOT_RawData == null)
            {
                return HttpNotFound();
            }
            return View(iOT_RawData);
        }

        // POST: IOT_RawData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IOT_RawData iOT_RawData = db.IOT_RawData.Find(id);
            db.IOT_RawData.Remove(iOT_RawData);
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
