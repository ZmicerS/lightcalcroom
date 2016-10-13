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
    public class IsprStrcMtrKfIspController : Controller
    {
        UnitOfWork unitOfWork;
        public IsprStrcMtrKfIspController()
        {
            unitOfWork = new UnitOfWork();
        }

        //
        // GET: /IsprStrcMtrKfIsp/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IsprStrctKfOtrz(int? id)
        {
            int tbkid = id ?? 0;
            var vvkof = unitOfWork.TblKfRepstr.GetById(tbkid);
            if (vvkof == null)
            {
                return HttpNotFound();
            }
            var vv = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == tbkid).AsQueryable(); ;
            if (vv == null)
            {
                return HttpNotFound();
            }
            string snmntb = vvkof.Name;
            List<TblKfClmnUI> tkcllst = new List<TblKfClmnUI>();
          /*  if (vv != null)
            {
                tkcllst = vv.Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).OrderBy(o => o.NmrCl).ToList(); ;
            }
            */
            ViewBag.NameTbl = snmntb;
            ViewBag.TblKfId = tbkid;
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

        [HttpPost]
        public ActionResult PlcDanIndxPm(int? TblKfId)
        //public JsonResult PlcDanIndxPm(int? TblKfId)
        {
            int tbkid = TblKfId ?? 0;
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
            // var data = new { nzvtbl = snmntb, spspom = vvrl };

            var data = vvrl;
            //     return Json(tkcllst, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);

            // ViewBag.NameTbl = snmntb;
            // ViewBag.TblKfId = tbkid;
            // return View();
            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<TblKfRowUI> GetDataIP(int tid)
        {
            var vvkof = unitOfWork.TblKfRepstr.GetById(tid);
            var vv = unitOfWork.TblKfRowRepstr.Get(t => t.TblKfId == tid).AsQueryable();
            List<TblKfRowUI> vvrl = vv.Select(s => new TblKfRowUI { Id = s.Id, NmrRw = s.NmrRw, IndxPm = s.IndxPm, TblKfId = s.TblKfId }).OrderBy(r => r.NmrRw).ToList();
            return vvrl;
        }

        [HttpPost]
        public ActionResult DbvlIndxPm(string Id, string TblKfId, string IndPm, string NmrRw)
        {
            HttpCookie cultureCookie = HttpContext.Request.Cookies["lang"];
            int pid = Int32.Parse(Id);
            int tid = Int32.Parse(TblKfId);
            int nomrow = Int32.Parse(NmrRw);
            nomrow++;
            IndPm = IndPm.Replace(',', '.');
            decimal dIndPm= 0.0m; 
            bool uipm;
            decimal res;
           
            if (Decimal.TryParse(IndPm, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dIndPm = res;

            // TblKfRow tkrw = unitOfWork.TblKfRowRepstr.GetById(pid);
           TblKfRow tkrw = new TblKfRow { IndxPm = dIndPm, TblKfId = tid, NmrRw = nomrow };
            tkrw.IndxPm = dIndPm;
            //  tkrui.NmrRw = tkrui.NmrRw;
            unitOfWork.TblKfRowRepstr.Insert(tkrw);
            unitOfWork.Save();
            //
            //
            var vclnlst = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == tid).AsQueryable().OrderBy(t => t.NmrCl).ToList();
            if (vclnlst != null && vclnlst.Count > 0)
            {
                foreach (TblKfClmn tcl in vclnlst)
                {
                    TblZnc tz = new TblZnc { NmrCl = tcl.NmrCl, NmrRw = nomrow, TblKfId = tkrw.TblKfId, Znac = 0 };
                    unitOfWork.TblZncRepstr.Insert(tz);
                }
                unitOfWork.Save();
            }
            //
            var data = GetDataIP(tid);
            //
            //     return Json(tkcllst, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
            //
            
        }

        [HttpPost]
        public JsonResult GetIndxPm(int? Id)
        {
            int icid = Id ?? 0;
            var vv = unitOfWork.TblKfRowRepstr.GetById(icid);
            if (vv == null)
            {
                // return HttpNotFound();
                var dat = new { };
                return Json(dat, JsonRequestBehavior.AllowGet);
            }
            //
            var data = new { Id = vv.Id, NmrRw = vv.NmrRw, IndxPm = vv.IndxPm, TblKfId = vv.TblKfId };
            return Json(data, JsonRequestBehavior.AllowGet);
            //
            // return null;
        }

        [HttpPost]
        public ActionResult EditIndxPm(string Id, string TblKfId, string IndPm)
        {
            //    return RedirectToAction("GetIndxPm"); //в ajax RedirectToAction не работает
            // Получаем куки из контекста, которые могут содержать установленную культуру
            ////америкнская с точкой берет и умножает на 10/?????
            HttpCookie cultureCookie = HttpContext.Request.Cookies["lang"];
            int pid = Int32.Parse(Id);
            int tid = Int32.Parse(TblKfId);
            decimal dIndPm=0.0m; ;
            bool uipm;
            decimal res;

            if (Decimal.TryParse(IndPm, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dIndPm = res;

            TblKfRow tkrw = unitOfWork.TblKfRowRepstr.GetById(pid);
            tkrw.IndxPm = dIndPm;
            //  tkrui.NmrRw = tkrui.NmrRw;
            unitOfWork.TblKfRowRepstr.Update(tkrw);
            unitOfWork.Save();
            var data = GetDataIP(tid);
            return Json(data, JsonRequestBehavior.AllowGet);
            //
         //   return null;
        }

        public JsonResult PlcDanKfOtrz(int? TblKfId)
        {
            int tbkid = TblKfId ?? 0;
            var vv = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == tbkid);//.FirstOrDefault();//   .GetById(tbkid);
            List<TblKfClmnUI> tkcllst = new List<TblKfClmnUI>();
            if (vv != null)
            {
                tkcllst = vv.Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).OrderBy(o => o.NmrCl).ToList(); ;
            }
            var data = tkcllst;
            //     return Json(tkcllst, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
          //  return null;
        }



        [HttpPost]
        public ActionResult DbvlColKofOtr(int TblKfId, int Ptlk, int Steny, int Pol, int NmrCl)
        {
            TblKfClmn tkcl = new TblKfClmn { TblKfId = TblKfId, Ptlk = Ptlk, Steny = Steny, Pol = Pol, NmrCl = ++NmrCl };
            unitOfWork.TblKfClmnRepstr.Insert(tkcl);
            unitOfWork.Save();
            var vrwlst = unitOfWork.TblKfRowRepstr.Get(t => t.TblKfId == tkcl.TblKfId).AsQueryable().OrderBy(t => t.NmrRw).ToList();
            foreach (TblKfRow tr in vrwlst)
            {
                TblZnc tz = new TblZnc { NmrCl = tkcl.NmrCl, NmrRw = tr.NmrRw, TblKfId = tkcl.TblKfId, Znac = 0 };
                unitOfWork.TblZncRepstr.Insert(tz);
            }
            unitOfWork.Save();
            var data = GetDataKO(TblKfId);
            return Json(data, JsonRequestBehavior.AllowGet);
          //  return RedirectToAction("KfOtrzAjxZgrzDan", new { TblKfId = tkcl.TblKfId });
            //      return null;
        }

        private IEnumerable<TblKfClmnUI> GetDataKO(int tid)
        {
            int tbkid = tid;// TblKfId ?? 0;
            var vv = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == tbkid);//.FirstOrDefault();//   .GetById(tbkid);
            List<TblKfClmnUI> tkcllst = new List<TblKfClmnUI>();
            if (vv != null)
            {
                tkcllst = vv.Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).OrderBy(o => o.NmrCl).ToList(); ;
            }
            return tkcllst;
        }


        public JsonResult GetColKofOtr(int? Id)
        {
            int icid = Id ?? 0;
            var vv = unitOfWork.TblKfClmnRepstr.GetById(icid);
            var data = new { Id = vv.Id, NmrCl = vv.NmrCl, Pol = vv.Pol, Ptlk = vv.Ptlk, Steny = vv.Steny, TblKfId = vv.TblKfId };
            return Json(data, JsonRequestBehavior.AllowGet);
            return null;
        }

        [HttpPost]
        public ActionResult EditColKofOtr(int Id, int TblKfId, int Ptlk, int Steny, int Pol)
        {
            TblKfClmn tkcl = unitOfWork.TblKfClmnRepstr.GetById(Id);
            tkcl.Ptlk = Ptlk;
            tkcl.Steny = Steny;
            tkcl.Pol = Pol;
            unitOfWork.TblKfClmnRepstr.Update(tkcl);
            unitOfWork.Save();
            // var vnn = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == TblKfId).OrderBy(t => t.NmrCl).ToList();
            // return RedirectToAction("KfOtrzAjxZgrzDan", new { TblKfId = tkcl.TblKfId });
            var data = GetDataKO(TblKfId);
            return Json(data, JsonRequestBehavior.AllowGet);
            // return null;
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}