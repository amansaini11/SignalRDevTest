using SignalRDevTest.Dal.Context;
using SignalRDevTest.Dal.Entity;
using SignalRDevTest.Dal.Model;
using SignalRDevTest.Dal.Repository;
using SignalRDevTest.Dal.SqlServerNotifier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace SignalRDevTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private DevTestContext db = new DevTestContext();
        public ActionResult Index()
        {
           var collection = unitOfWork.DevTestRepository.GetDevTestQuerable();// db.DevTests;       
           ViewBag.NotifierEntity = unitOfWork.context.GetNotifierEntity<DevTest>(collection).ToJson();
            return View(collection.ToList());
        }


        public ActionResult IndexPartial()
        {
            var collection = unitOfWork.DevTestRepository.GetDevTestQuerable();// db.DevTests;       
            ViewBag.NotifierEntity = unitOfWork.context.GetNotifierEntity<DevTest>(collection).ToJson();
            return PartialView("_IndexPartial", collection.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DevTest devTest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.DevTestRepository.Insert(devTest);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dx)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(devTest);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevTest devTest = unitOfWork.DevTestRepository.GetByID(id);
            if (devTest == null)
            {
                return HttpNotFound();
            }
            return View(devTest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DevTest devTest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.DevTestRepository.Update(devTest);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dx)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(devTest);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unitOfWork.DevTestRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}