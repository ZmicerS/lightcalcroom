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
    public class SmnSvtlUI
    {
      public int Lmpest { set; get; }
      public string  NazvaLmp { set; get; }
      public int KlLmp { set; get; }
      public int Wt { set; get; }
      public int Lumen { set; get; }
        public int TblKfId { set; get; }
        public int Kolstr { set; get; }
        public int Kolcln { set; get; }
        public string NzvSvtl { set; get; }
        public string NzvTbl { set; get; }
        public string[,] Shapka { set; get; }
        public string[] Levo { set; get; }
        public string[,] Znac   { set; get; }
    }

    public class HomeController : Controller
    {
        UnitOfWork unitOfWork;

        public HomeController()
        {
            unitOfWork = new UnitOfWork();
        }


        public ActionResult Index()
        {
            var vvu = unitOfWork.SvtlRepstr.GetAll().OrderBy(s => s.Name);
            List<SelectListItem> lstsv;
            if (vvu != null)
            {
                IEnumerable<SelectListItem> tks = vvu.Select(s => new SelectListItem {Value=s.Id.ToString(),Text=s.Name });
                lstsv = tks.ToList();
                lstsv.Insert(0, new SelectListItem { Value = "-1", Text = "-выберите светильник-", Selected = true });
                SelectList sllstsv = new SelectList(lstsv, "Value", "Text");
                ViewBag.Slst = sllstsv;
            }
            else
            {

            }
                return View();
        }

        [HttpPost]
        public ActionResult SmnSvtlnc(int svtlid)
        {
            string NzvSvtl, NzvTbl;
            int tblid;
            tblid = -1;
            SmnSvtlUI smnsvtl = new SmnSvtlUI();
            smnsvtl.Lmpest = 0;
            smnsvtl.TblKfId = -1;
            try {
                var vkdtb = unitOfWork.SvtlRepstr.GetById(svtlid);
                if (vkdtb != null)
                {
                    tblid = (int)vkdtb.TblKfId;
                    NzvSvtl = vkdtb.Name;
                    NzvTbl = vkdtb.TblKf.Name;
                   var vlmp = vkdtb.TblLmps.FirstOrDefault();
                    var vrow = vkdtb.TblKf.TblKfRows;
                    int klrw = 0;
                    int klcl = 0;
                    IEnumerable<TblKfRowUI> tbrw = null;
                    IEnumerable<TblKfClmnUI> tbcl = null;
                    if (vrow!=null&&vrow.Count()>0)
                    {
                         tbrw =vrow.OrderBy(o => o.NmrRw).Select(t => new TblKfRowUI { Id = t.Id, NmrRw = t.NmrRw, IndxPm = t.IndxPm, TblKfId = t.TblKfId }).ToList();
                        klrw = tbrw.Count();
                    }
                    var vclmn = vkdtb.TblKf.TblKfClmns;
                    if (vclmn != null && vclmn.Count > 0)
                    {
                         tbcl = vclmn.OrderBy(o => o.NmrCl).Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).ToList();
                         klcl = tbcl.Count();
                    }
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
                    string[,] mskfisp = new string[klrw, klcl];
                    var vznc = vkdtb.TblKf.TblZncs;
                    if(vznc!=null&&vznc.Count()>0)
                    {
                        IEnumerable<TbKZncUI> tbznc = vznc.Select(s => new TbKZncUI { Id = s.Id, NmrCl = s.NmrCl, NmrRw = s.NmrRw, Znac = s.Znac }).OrderBy(o => o.NmrRw).ThenBy(o => o.NmrCl).Cast<TbKZncUI>().ToList();
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
                    }
                    if (vlmp != null)
                    {
                        smnsvtl.Lmpest = 1;
                        smnsvtl.NazvaLmp = vlmp.Name;
                        smnsvtl.KlLmp = vlmp.KlLmp;
                        smnsvtl.Wt = vlmp.Wt;
                        smnsvtl.Lumen = vlmp.Lumen;
                    }
                    else
                    {
                        smnsvtl.Lmpest = 0;
                        smnsvtl.NazvaLmp = "";
                        smnsvtl.KlLmp = 0;
                        smnsvtl.Wt = 0;
                        smnsvtl.Lumen = 0;
                    }
                    smnsvtl.TblKfId = tblid;
                    smnsvtl.NzvSvtl = NzvSvtl;
                    smnsvtl.NzvTbl=NzvTbl;
                    smnsvtl.Kolcln = klcl;
                    smnsvtl.Kolstr = klrw;
                    smnsvtl.Shapka = mszgl;
                    smnsvtl.Levo = msstrk;
                    smnsvtl.Znac = mskfisp;

                }// if (vkdtb != null)
            }
            catch (InvalidOperationException ioe)
            {

            }
           var data = smnsvtl;
            return Json(smnsvtl);
        }

        [HttpPost]
        public ActionResult RschtOsvet()
        {/* var elms = ['dlnpom', 'shrpom', 'vyspom', 'nrmosv', 'urrbpov', 'vstsvs', 'kfzps'];
    var elms2 = ['kdtb', 'svtptk', 'nzvlmp', 'pwr', 'kl'];*/
            string sdlnpom = HttpContext.Request["dlnpom"];
            string sshrpom = HttpContext.Request["shrpom"];
            string svyspom = HttpContext.Request["vyspom"];
            string snrmosv = HttpContext.Request["nrmosv"];
            string surrbpov = HttpContext.Request["urrbpov"];
            string svstsvs = HttpContext.Request["vstsvs"];
            string skfzps = HttpContext.Request["kfzps"];
            string sKdSvtl = HttpContext.Request["KdSvtl"];
            string sKdOtrz = HttpContext.Request["KdOtrz"];
            string skdtb = HttpContext.Request["kdtb"];
            string ssvtptk = HttpContext.Request["svtptk"];
            string snzvlmp = HttpContext.Request["nzvlmp"];
            string spwr = HttpContext.Request["pwr"];
            string skl = HttpContext.Request["kl"];
            //
            sdlnpom = sdlnpom.Replace(',', '.');
            sshrpom = sshrpom.Replace(',', '.');
            svyspom = svyspom.Replace(',', '.');
            snrmosv = snrmosv.Replace(',', '.');
            surrbpov =surrbpov.Replace(',', '.');
            skfzps = skfzps.Replace(',', '.');
            svstsvs = svstsvs.Replace(',', '.');
            //
            double res;
            double ddlnpom=0;
            if (Double.TryParse(sdlnpom, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                ddlnpom = res;
            double dshrpom=0;
            if (Double.TryParse(sshrpom, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dshrpom = res;
            double dvyspom=0;
            if (Double.TryParse(svyspom, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dvyspom = res;
            double dnrmosv=0;
            if (Double.TryParse(snrmosv, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dnrmosv = res;
            double durrbpov=0;
            if (Double.TryParse(surrbpov, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                durrbpov = res;
            double dvstsvs=0;
            if (Double.TryParse(svstsvs, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dvstsvs = res;
            double dkfzps=0;
            if (Double.TryParse(skfzps, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dkfzps = res;
            int ires;
            int KdSvtl=-1;
            if (Int32.TryParse(sKdSvtl, NumberStyles.Integer, CultureInfo.GetCultureInfo("en-US"), out ires))
                KdSvtl = ires;
            int KdOtrz=-1;
            if (Int32.TryParse(sKdOtrz, NumberStyles.Integer, CultureInfo.GetCultureInfo("en-US"), out ires))
                KdOtrz = ires;
            int kdtb;
            if (Int32.TryParse(skdtb, NumberStyles.Integer, CultureInfo.GetCultureInfo("en-US"), out ires))
                kdtb = ires;
            double dsvtptk=0;
            if (Double.TryParse(ssvtptk, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dsvtptk = res;
            double dpwr=0;
            if (Double.TryParse(spwr, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"), out res))
                dpwr = res;
            int kl=0;
            if (Int32.TryParse(skl, NumberStyles.Integer, CultureInfo.GetCultureInfo("en-US"), out ires))
                kl = ires;
            //
            double drschvst = dvyspom - dvstsvs - durrbpov;
            if (drschvst <0.00001) goto oshb;
            double dindpm = (ddlnpom * dshrpom) / (drschvst * (ddlnpom + dshrpom));
            decimal dsmindpm = Convert.ToDecimal(dindpm, CultureInfo.GetCultureInfo("en-US"));   // CultureInfo.ge  ("en-US"));
            //
            var vkdsvt = unitOfWork.SvtlRepstr.GetById(KdSvtl);
            if (vkdsvt == null) goto oshb;
            if (vkdsvt!=null)
            {
                int? ikodtab = vkdsvt.TblKfId ?? 0;
                IEnumerable<TblKfRowUI> tbrw =vkdsvt.TblKf.TblKfRows.OrderBy(o => o.NmrRw).Select(t => new TblKfRowUI { Id = t.Id, NmrRw = t.NmrRw, IndxPm = t.IndxPm, TblKfId = t.TblKfId }).ToList();
                IEnumerable<TblKfClmnUI> tbcl = vkdsvt.TblKf.TblKfClmns.OrderBy(o => o.NmrCl).Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).ToList();
                IEnumerable<TbKZncUI> tbznc = vkdsvt.TblKf.TblZncs.Select(s => new TbKZncUI { Id = s.Id, NmrCl = s.NmrCl, NmrRw = s.NmrRw, Znac = s.Znac }).OrderBy(o => o.NmrRw).ThenBy(o => o.NmrCl).Cast<TbKZncUI>().ToList();
                List<int> stlbznc = new List<int>();
                foreach (TbKZncUI tz in tbznc)
                {
                    if (tz.NmrCl == KdOtrz)
                    {
                        stlbznc.Add(tz.Znac);
                    }
                }
                int inmrw = 0;
                int iKolIndPom = tbrw.Count();
                for (int i = 0; i < iKolIndPom - 1; i++) // && iFlgPom == 0
                {
                    decimal dcmind1 = tbrw.ElementAt(i).IndxPm;
                    decimal dcmind2 = tbrw.ElementAt(i + 1).IndxPm;
                    if ((dcmind1 - 0.0001M) < dsmindpm && dsmindpm < (dcmind2 + 0.0001M))
                    {
                        if ((dsmindpm - dcmind1) < (dcmind2 - dsmindpm))
                            inmrw = i + 1;
                        else
                            inmrw = i + 1 + 1;
                        break;
                    }//if ((dip1-0.0001)<dIndPom&&dIndPom<(dip2+0.0001))
                     //
                }//for
               var vvzn= vkdsvt.TblKf.TblZncs.Where(t => t.TblKfId == ikodtab && t.NmrCl == KdOtrz && t.NmrRw == inmrw).FirstOrDefault();
                
                int ikofisp = -1;
                double dKlSvt = 0;
                if (vvzn != null)
                {
                    ikofisp = vvzn.Znac;
                }
                else
                {
                     goto oshb;
                }
                
                double dchsl = (dnrmosv * (ddlnpom * dshrpom) * 100);
                int ikllmp = kl;
               
                double dznm = (ikofisp * ikllmp * dsvtptk * dkfzps);
                if (dznm > 0.0001)
                    dKlSvt = dchsl / dznm;
                //
                
                string sklsvt = String.Format("Количество светильников ≈ {0:0.##};    коэффициент использования={1:0.##};  индекс помещения={2:0.##}", dKlSvt, ikofisp, dsmindpm);
                var data = new Object();
                data = new {Rez=1, KlSvt = dKlSvt, Sitg = sklsvt };

                return Json(data);
            }//if (vkdsvt!=null)
            oshb:
            var dataplh = new { Rez = 0, KlSvt = 0, Sitg = "Неверно введены данные!" };
            return Json(dataplh);
           
        }


       

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
