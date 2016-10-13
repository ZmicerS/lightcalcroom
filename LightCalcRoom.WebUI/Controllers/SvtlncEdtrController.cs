using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

using LightCalcRoom.DAL.Infrastructure;
using LightCalcRoom.DAL.Concreate;
using LightCalcRoom.WebUI.Models;

namespace LightCalcRoom.WebUI.Controllers
{
    public class SvtlncEdtrController : Controller
    {
        UnitOfWork unitOfWork;

        public SvtlncEdtrController()
        {
            unitOfWork = new UnitOfWork();
        }

        //
        // GET: /SvtlncEdtr/
         public ActionResult Index()
        {
            return RedirectToAction("ZplSvtlnc");
        }
        [HttpGet]
         public ActionResult ZplSvtlnc()
        {
            List<SelectListItem> lstslit;
            var vrtkf = unitOfWork.TblKfRepstr.GetAll().OrderBy(s => s.Name);
            if (vrtkf != null)
            {
                IEnumerable<SelectListItem> tkf = vrtkf.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
                //List<SelectListItem> lstslit = tkf.ToList();
                lstslit = tkf.ToList();
                lstslit.Insert(0, new SelectListItem { Value = "-1", Text = "-выберите таблицу-", Selected = true });
                SelectList sllsttb = new SelectList(lstslit, "Value", "Text");
                ViewBag.sllsttb = sllsttb;
            }
            else
            {
                lstslit = new List<SelectListItem>();
                lstslit.Add(new SelectListItem { Value = "-1", Text = "-выберите таблицу-", Selected = true });
                SelectList sllsttb = new SelectList(lstslit, "Value", "Text");
                ViewBag.sllsttb = sllsttb;
            }
            //
            List<SelectListItem> lstselsvtl;
            SelectList slctsvtl;
            var vvsv=unitOfWork.SvtlRepstr.GetAll().OrderBy(s => s.Name);
            if (vvsv!=null)
            {
                IEnumerable<SelectListItem> svs = vvsv.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
                lstselsvtl = svs.ToList();
                lstselsvtl.Insert(0, new SelectListItem { Value = "-1", Text = "-выберите светильник-", Selected = true });
                slctsvtl = new SelectList(lstselsvtl, "Value", "Text");
                ViewBag.slctsvtl = slctsvtl;
              
            }
            else
            {
                 lstselsvtl=new List<SelectListItem>();
                lstselsvtl.Add(new SelectListItem { Value = "-1", Text = "-выберите таблицу-", Selected = true });
                 slctsvtl = new SelectList(lstselsvtl, "Value", "Text");
                ViewBag.slctsvtl = slctsvtl;
            }
            //
            return View();
        }

        [HttpPost]
        public JsonResult PlcTblKfAjx(int TblId=0)
        {
            //var data = new {};
            TbKfExOtbrUI tabkof = new TbKfExOtbrUI();
            string Nazva;
            var vres = unitOfWork.TblKfRepstr.GetById(TblId);
            if (vres != null)
            {
                //   var vv=vres.TblKfClmns.OrderBy
                Nazva = vres.Name;
                IEnumerable<TblKfRowUI> tbrw = vres.TblKfRows.OrderBy(o => o.NmrRw).Select(t => new TblKfRowUI { Id = t.Id, NmrRw = t.NmrRw, IndxPm = t.IndxPm, TblKfId = t.TblKfId }).ToList();
                IEnumerable<TblKfClmnUI> tbcl = vres.TblKfClmns.OrderBy(o => o.NmrCl).Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).ToList();
                int klrw = tbrw.Count();
                int klcl = tbcl.Count();
                string[,] mszgl = new string[3, klcl];
                int k = 0;
                foreach (TblKfClmnUI cl in tbcl)
                {
                    mszgl[0, k] = String.Format("{0}", cl.Ptlk);
                    mszgl[1, k] = String.Format("{0}", cl.Steny);
                    mszgl[2, k] = String.Format("{0}", cl.Pol);
                    k++;
                }
                string[] msstrk = new string[klrw];
                int m = 0;
                foreach (TblKfRowUI rw in tbrw)
                {
                    msstrk[m] = String.Format("{0:0.##}", rw.IndxPm);
                    m++;
                }
                //  int[,] mskfisp = new int[klrw, klcl];
                string[,] mskfisp = new string[klrw, klcl];
                IEnumerable<TbKZncUI> tbznc = vres.TblZncs.Select(s => new TbKZncUI { Id = s.Id, NmrCl = s.NmrCl, NmrRw = s.NmrRw, Znac = s.Znac }).OrderBy(o => o.NmrRw).ThenBy(o => o.NmrCl).Cast<TbKZncUI>().ToList();
                foreach (TbKZncUI tz in tbznc)
                {
                    int id = tz.Id;
                    string strw = String.Format("{0:d2}", tz.NmrRw);
                    string stcl = String.Format("{0:d2}", tz.NmrCl);
                    if (tz.NmrRw <= klrw && tz.NmrCl <= klcl)
                    {
                        //   mskfisp[tz.NmrRw-1,tz.NmrCl-1] = tz.Znac;
                        mskfisp[tz.NmrRw - 1, tz.NmrCl - 1] = String.Format("{0}", tz.Znac);
                    }
                    else
                    {
                        int ui = 0;
                    }
                }
                // var data = new { Kolstr = klrw, Kolcln = klcl, Nazva = "ARSR/R", Shapka = mszgl, Levo = msstrk, Znac = mskfisp };
                tabkof = new TbKfExOtbrUI { Id = TblId, Nazva = Nazva, Kolcln=klcl,Kolstr=klrw,MsKfOtrz = mszgl,MsIndPm=msstrk,MsKf=mskfisp };
            }
        //    else
          //      data = new { Kolstr = 1 };
            return Json(tabkof);
        }

        [HttpPost]
        public ActionResult ZplSvtlnc(SvtlExUI svt)
        {
            if (svt.TblKfId<1)
            {
              //  ModelState.AddModelError("TblKfId", "НЕ выбрана таблица");
            }
            //надо проверук вставить в форме или пусть пока проходит табл -1
            if (ModelState.IsValid)
            {
                TblLmp tblmp = new TblLmp { KlLmp = svt.Kol, Lumen = svt.Potok, Name = svt.Lampa, Wt = svt.Pwr, NmrStrk = 0 };
                List<TblLmp> lstlmp = new List<TblLmp> { tblmp };
               // lstlmp.Add(tblmp);
                Svtl svtl = new Svtl { Name = svt.Svtlnk, TblKfId = svt.TblKfId,TblLmps= lstlmp };
                unitOfWork.SvtlRepstr.Insert(svtl);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult PlcSvtlAjx(int SvtlId)
        {
            string NzvSvtl = "";
            string NazvaLmp="";
            int KlLmp = 0;
            int Wt = 0;
            int Lumen = 0;
            int Lmpest=0;
            int IdLmp = 0;
            var vkdtb = unitOfWork.SvtlRepstr.GetById(SvtlId);
            if (vkdtb != null)
            {
                NzvSvtl = vkdtb.Name;
                var vlmp = vkdtb.TblLmps.FirstOrDefault();
                if (vlmp != null)
                {
                    Lmpest = 1;
                    NazvaLmp = vlmp.Name;
                    KlLmp = vlmp.KlLmp;
                    Wt = vlmp.Wt;
                    Lumen = vlmp.Lumen;
                    IdLmp = vlmp.Id;
                }
                else
                {
                    Lmpest = 0;
                    NazvaLmp = "";
                    KlLmp = 0;
                    Wt = 0;
                    Lumen = 0;
                    IdLmp = -1;
                }
            }//
            var data = new { Lmpest = Lmpest, NzvSvtl = NzvSvtl, NazvaLmp = NazvaLmp, KlLmp = KlLmp, Wt = Wt, Lumen = Lumen, IdLmp = IdLmp };
                return Json(data);
           // return null;
        }


        [HttpPost]
        public ActionResult IzmSvtlnc(int IdLmp, int SvtlId , string Svtlnk, string Lampa, int? Kol, int? Pwr, int? Potok)
        {
            var vvs = unitOfWork.SvtlRepstr.GetById(SvtlId);
            int ikol = Kol ?? 0;
            int ipwr = Pwr ?? 0;
            int iptk = Potok ?? 0;
            if(vvs!=null)
            {
                vvs.Name = Svtlnk;
                
            }
            var vvl = unitOfWork.TblLmpRepstr.GetById(SvtlId);
            if (vvl!=null)
            {
                vvl.Name = Lampa;
                vvl.KlLmp = ikol;
                vvl.Wt = ipwr;
                vvl.Lumen = iptk;
            }
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

	}
}