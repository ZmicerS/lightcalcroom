using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Globalization;

using LightCalcRoom.DAL.Infrastructure;
using LightCalcRoom.DAL.Concreate;
using LightCalcRoom.WebUI.Models;

namespace LightCalcRoom.WebUI.Controllers
{
    public class IndxPmSzdKrController : Controller
    {

        UnitOfWork unitOfWork;
        public IndxPmSzdKrController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: IndxPmSzdKrController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IsprStrctIndPom(int? id)
        {
            int tbkid = id ?? 0;
            var vvkof = unitOfWork.TblKfRepstr.GetById(tbkid);
            if (vvkof == null)
            {
                return HttpNotFound();
            }
            var vv = unitOfWork.TblKfRowRepstr.Get(t => t.TblKfId == tbkid).AsQueryable();//.OrderBy(r => r.NmrRw);
            if (vv == null)
            {
                return HttpNotFound();
            }
            List<TblKfRowUI> vvrl = vv.Select(s => new TblKfRowUI { Id = s.Id, NmrRw = s.NmrRw, IndxPm = s.IndxPm, TblKfId = s.TblKfId }).OrderBy(r => r.NmrRw).ToList();
            string snmntb = vvkof.Name;
            var data = new { nzvtbl = snmntb, spspom = vvrl };
            /*
             *      var data = tkcllst;
       //     return Json(tkcllst, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
             */
            ViewBag.NameTbl = snmntb;
            ViewBag.TblKfId = tbkid;
            return View();
            // return Json(data, JsonRequestBehavior.AllowGet);
            //  return null;
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}