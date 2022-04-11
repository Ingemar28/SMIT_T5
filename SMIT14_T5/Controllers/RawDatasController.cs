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
    public class RawDatasController : Controller
    {
        private TEST_weatherEntities db = new TEST_weatherEntities();

        // GET: RawDatas
        public ActionResult Index()
        {
            return View(db.RawData.ToList());
        }

        // GET: RawDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawData rawData = db.RawData.Find(id);
            if (rawData == null)
            {
                return HttpNotFound();
            }
            return View(rawData);
        }

        // GET: RawDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RawDatas/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Data_ID,DataTime,Temperature,Humidity,Pressure,H24R,DateTempHigh,DateTempLow,StationID")] RawData rawData)
        {
            if (ModelState.IsValid)
            {
                db.RawData.Add(rawData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rawData);
        }

        // GET: RawDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawData rawData = db.RawData.Find(id);
            if (rawData == null)
            {
                return HttpNotFound();
            }
            return View(rawData);
        }

        // POST: RawDatas/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Data_ID,DataTime,Temperature,Humidity,Pressure,H24R,DateTempHigh,DateTempLow,StationID")] RawData rawData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rawData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rawData);
        }

        // GET: RawDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawData rawData = db.RawData.Find(id);
            if (rawData == null)
            {
                return HttpNotFound();
            }
            return View(rawData);
        }

        // POST: RawDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RawData rawData = db.RawData.Find(id);
            db.RawData.Remove(rawData);
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
