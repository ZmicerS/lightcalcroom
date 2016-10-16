using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//
using System.Net;
using System.IO;
using System.Text;
//
using LightCalcRoom.WebUI.Models;
using LightCalcRoom.DAL.Infrastructure;
using LightCalcRoom.DAL.Concreate;
namespace LightCalcRoom.WebUI.Controllers
{
    public class TabKofIspController : Controller
    {
        UnitOfWork unitOfWork;
        public TabKofIspController()
        {
            unitOfWork = new UnitOfWork();
        }
        //
        // GET: /TabKofIsp/
        public ActionResult Index()
        {
            List<SelectListItem> lstslit;
            var vrtkf = unitOfWork.TblKfRepstr.GetAll().OrderBy(s => s.Name);
            if (vrtkf!=null)
            {
                IEnumerable<SelectListItem> tkf = vrtkf.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
                
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
            return View();
        }


        [HttpPost]   
        public ActionResult  AddTblKof(string TblName,string MetaTbl )
        {
          if (MetaTbl=="stnd"&& TblName.Length>0)
          {
              List<TblKfClmn> lsttbkc = new List<TblKfClmn>();
              lsttbkc.Add(new TblKfClmn { NmrCl = 1, Ptlk = 80, Steny = 80, Pol = 30 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 2, Ptlk = 80, Steny = 50, Pol = 30 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 3, Ptlk = 80, Steny = 30, Pol = 10 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 4, Ptlk = 70, Steny = 50, Pol = 20 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 5, Ptlk = 50, Steny = 50, Pol = 10 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 6, Ptlk = 50, Steny = 30, Pol = 10 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 7, Ptlk = 30, Steny = 30, Pol = 10 });
              lsttbkc.Add(new TblKfClmn { NmrCl = 8, Ptlk = 0, Steny = 0, Pol = 0 });
              List<TblKfRow> lsttbrw = new List<TblKfRow>();
              lsttbrw.Add(new TblKfRow { NmrRw = 1, IndxPm = 0.6m });
              lsttbrw.Add(new TblKfRow { NmrRw = 2, IndxPm = 0.8m });
              lsttbrw.Add(new TblKfRow { NmrRw = 3, IndxPm = 1.0m });
              lsttbrw.Add(new TblKfRow { NmrRw = 4, IndxPm = 1.25m });
              lsttbrw.Add(new TblKfRow { NmrRw = 5, IndxPm = 1.5m });
              lsttbrw.Add(new TblKfRow { NmrRw = 6, IndxPm = 2m });
              lsttbrw.Add(new TblKfRow { NmrRw = 7, IndxPm = 2.5m });
              lsttbrw.Add(new TblKfRow { NmrRw = 8, IndxPm = 3m });
              lsttbrw.Add(new TblKfRow { NmrRw = 9, IndxPm = 4m });
              lsttbrw.Add(new TblKfRow { NmrRw = 10, IndxPm = 5m });
              List<TblZnc> lsttbznc = new List<TblZnc>();
              foreach (TblKfRow rw in lsttbrw)
              {
                  foreach (TblKfClmn cl in lsttbkc)
                  {
                      TblZnc tznc = new TblZnc { NmrCl = cl.NmrCl, NmrRw = rw.NmrRw, Znac = 0 };
                      lsttbznc.Add(tznc);

                  }
              }
              TblKf tbkf = new TblKf { Name = TblName, TblKfClmns = lsttbkc, TblKfRows = lsttbrw, TblZncs = lsttbznc };
              unitOfWork.TblKfRepstr.Insert(tbkf);
              unitOfWork.Save();
              int? ik = tbkf.Id;
              int id = ik ?? 0;
             
              return RedirectToAction("EdtrTblStnd", new { TblKfId = id });
          }
            if (MetaTbl == "nonstnd")
            {
                if (TblName == "dajvsetably")
                {
                    return RedirectToAction("DownloadFile", new { fileName = TblName });
                    return   RedirectToAction("Index");
                }
                TblKf tbkf = new TblKf { Name = TblName };
                unitOfWork.TblKfRepstr.Insert(tbkf);
                  unitOfWork.Save();
                  var idtb = tbkf.Id;
                return RedirectToAction("IsprStrctKfOtrz","IsprStrcMtrKfIsp", new { id = idtb });                
            }

            return View();
        }

       // EdtrTblStnd
        [HttpGet]
        public ActionResult EdtrTblStnd(int? TblKfId)
        {
            
            int tkid = TblKfId ?? 0;
            
            var vk = unitOfWork.TblKfRepstr.GetById(tkid);
            IEnumerable<TblKfRowUI> tbrw;
            IEnumerable<TblKfClmnUI> tbcl;
            int klrw = 0;
            int klcl = 0;
            string[,] mszgl = new string[0, 0];
            IEnumerable<TbKZncUI> tbznc;
            string[] msstrk = new string[0];
            string[,] mskfisp = new string[0, 0];
            string snmtbl = "";
                
            //
            int vkid = 0;
            if (vk != null)
            {
                vkid = vk.Id;
                snmtbl = vk.Name;

            }
            //
            if (vkid > 0)
            {
                tbrw = unitOfWork.TblKfRowRepstr.Get(t => t.TblKfId == vkid).OrderBy(o => o.NmrRw).Select(t => new TblKfRowUI { Id = t.Id, NmrRw = t.NmrRw, IndxPm = t.IndxPm, TblKfId = t.TblKfId }).ToList();
                tbcl = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == vkid).OrderBy(o => o.NmrCl).Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).ToList();
                klrw = tbrw.Count();
                klcl = tbcl.Count();
                mszgl = new string[3, klcl];
                //
                int k = 0;
                foreach (TblKfClmnUI cl in tbcl)
                {
                    mszgl[0, k] = String.Format("{0}", cl.Ptlk);
                    mszgl[1, k] = String.Format("{0}", cl.Steny);
                    mszgl[2, k] = String.Format("{0}", cl.Pol);
                    k++;
                }
                msstrk = new string[klrw];
                int m = 0;
                foreach (TblKfRowUI rw in tbrw)
                {
                    msstrk[m] = String.Format("{0:0.##}", rw.IndxPm);
                    m++;
                }
                //
                mskfisp = new string[klrw, klcl];
                tbznc = unitOfWork.TblZncRepstr.Get(t => t.TblKfId == vkid).Select(s => new TbKZncUI { Id = s.Id, NmrCl = s.NmrCl, NmrRw = s.NmrRw, Znac = s.Znac }).OrderBy(o => o.NmrRw).ThenBy(o => o.NmrCl).Cast<TbKZncUI>().ToList();
                //
                foreach (TbKZncUI tz in tbznc)
                {
                    int id = tz.Id;
                    string strw = String.Format("{0:d2}", tz.NmrRw);
                    string stcl = String.Format("{0:d2}", tz.NmrCl);
                    if (tz.NmrRw <= klrw && tz.NmrCl <= klcl)
                    {
                        
                        mskfisp[tz.NmrRw - 1, tz.NmrCl - 1] = String.Format("{0}", tz.Znac);
                    }
                    else
                    {
                        int ui = 0;
                    }
                }
            }//if(vkid>0)
            //
            ViewBag.Mskfisp = mskfisp;
            ViewBag.Msrw = msstrk;//tbrw;
            ViewBag.Mscl = mszgl;//tbcl;
            ViewBag.KdTb = vkid;
            ViewBag.NmTbl = snmtbl;
            ViewBag.Klrw = klrw;
            ViewBag.Klcl = klcl;
            //
            return View();
        }

        [HttpPost]
        public ActionResult EdtrTblStnd()
        {
            var vkdtb = HttpContext.Request["kdtb"];
            //          
            
            int ikdtb=0;
            int number;
            bool result = Int32.TryParse((string)vkdtb, out number);
            if (result) ikdtb = number;
            IEnumerable<TblZnc> tzc = unitOfWork.TblZncRepstr.Get(t => t.TblKfId == ikdtb).OrderBy(o => o.NmrRw).ThenBy(t => t.NmrCl).ToList();
            int kol = tzc.Count();
            var vklrw = HttpContext.Request["klrw"];
            int iklrw =0;// Int32.Parse((string)vklrw);
            result = Int32.TryParse((string)vklrw, out number);
            if (result) iklrw = number;
            var vklcl = HttpContext.Request["klcl"];
            int iklcl = 0;// Int32.Parse((string)vklcl);
            result = Int32.TryParse((string)vklcl, out number);
            if (result) iklcl = number;
            var v1 = HttpContext.Request["mas0000"];
            //
            for (int m = 0; m < kol; m++)
            {
                int irw = tzc.ElementAt(m).NmrRw;
                int icln = tzc.ElementAt(m).NmrCl;
                if (irw <= iklrw && icln <= iklcl)
                {
                    string strw = String.Format("{0:d2}", irw - 1);
                    string stcl = String.Format("{0:d2}", icln - 1);
                    string mas = "mas" + strw + stcl;
                    var vms = HttpContext.Request[mas];
                    int iznc = Int32.Parse((string)vms);
                    tzc.ElementAt(m).Znac = iznc;
                    unitOfWork.TblZncRepstr.Update(tzc.ElementAt(m));
                }
            }
            unitOfWork.Save();
            //
            return RedirectToAction("Index");
        
        }

        public ActionResult EdtrTblNonStnd(int? TblKfId)
        {
            
            int tkid = TblKfId ?? 0;
            
            var vk = unitOfWork.TblKfRepstr.GetById(tkid);
            IEnumerable<TblKfRowUI> tbrw;
            IEnumerable<TblKfClmnUI> tbcl;
            int klrw = 0;
            int klcl = 0;
            string[,] mszgl = new string[0, 0];
            IEnumerable<TbKZncUI> tbznc;
            string[] msstrk = new string[0];
            string[,] mskfisp = new string[0, 0];
            string snmtbl = "";

            //
            //
            int vkid = 0;
            if (vk != null)
            {
                vkid = vk.Id;
                snmtbl = vk.Name;

            }
            if (vkid > 0)
            {
                tbrw = unitOfWork.TblKfRowRepstr.Get(t => t.TblKfId == vkid).OrderBy(o => o.NmrRw).Select(t => new TblKfRowUI { Id = t.Id, NmrRw = t.NmrRw, IndxPm = t.IndxPm, TblKfId = t.TblKfId }).ToList();
                tbcl = unitOfWork.TblKfClmnRepstr.Get(t => t.TblKfId == vkid).OrderBy(o => o.NmrCl).Select(t => new TblKfClmnUI { Id = t.Id, NmrCl = t.NmrCl, Pol = t.Pol, Steny = t.Steny, Ptlk = t.Ptlk, TblKfId = t.TblKfId }).ToList();
                klrw = tbrw.Count();
                klcl = tbcl.Count();
                mszgl = new string[3, klcl];
                //
                int k = 0;
                foreach (TblKfClmnUI cl in tbcl)
                {
                    mszgl[0, k] = String.Format("{0}", cl.Ptlk);
                    mszgl[1, k] = String.Format("{0}", cl.Steny);
                    mszgl[2, k] = String.Format("{0}", cl.Pol);
                    k++;
                }
                msstrk = new string[klrw];
                int m = 0;
                foreach (TblKfRowUI rw in tbrw)
                {
                    msstrk[m] = String.Format("{0:0.##}", rw.IndxPm);
                    m++;
                }
                //
                mskfisp = new string[klrw, klcl];
                tbznc = unitOfWork.TblZncRepstr.Get(t => t.TblKfId == vkid).Select(s => new TbKZncUI { Id = s.Id, NmrCl = s.NmrCl, NmrRw = s.NmrRw, Znac = s.Znac }).OrderBy(o => o.NmrRw).ThenBy(o => o.NmrCl).Cast<TbKZncUI>().ToList();
                //
                foreach (TbKZncUI tz in tbznc)
                {
                    int id = tz.Id;
                    string strw = String.Format("{0:d2}", tz.NmrRw);
                    string stcl = String.Format("{0:d2}", tz.NmrCl);
                    if (tz.NmrRw <= klrw && tz.NmrCl <= klcl)
                    {
                        
                        mskfisp[tz.NmrRw - 1, tz.NmrCl - 1] = String.Format("{0}", tz.Znac);
                    }
                    else
                    {
                        int ui = 0;
                    }
                }
            }//if(vkid>0)
             //
             //
            ViewBag.Mskfisp = mskfisp;
            ViewBag.Msrw = msstrk;//tbrw;
            ViewBag.Mscl = mszgl;//tbcl;
            ViewBag.KdTb = vkid;
            ViewBag.NmTbl = snmtbl;
            ViewBag.Klrw = klrw;
            ViewBag.Klcl = klcl;
            //
            //
            return View();
        }


        public FileResult DownloadFile(string fileName)
        {
            IEnumerable<string> lststr = GetTabls();
            MemoryStream memstrm = new MemoryStream();
            foreach (string st in lststr)
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(st);
                memstrm.Write(array, 0, array.Length);
            }//
            return File(memstrm.GetBuffer(), "text/plain", "file.txt");

        }

        private IEnumerable<string> GetTabls()
        {
            List<string> lstdn = new List<string>();
            //
            var vv = unitOfWork.TblKfRepstr;
            // IEnumerable<TblKf> lstkf = unitOfWork.TblKfRepstr.GetAll();
            var vvall = vv.GetAll();
            IEnumerable<TblKf> lstkf = vvall.ToList();
             // IEnumerable<TblKf> lstkf = vv.GetAll();
             //
            string scrlf = "\r\n";
            //
            String strk1 = "";
            String strk2 = "";
            String strk3 = "";
            string snmtbl;
            string skntbl;
            int kodtab;
            //
            foreach (TblKf tk in lstkf)
            {
                kodtab = tk.Id;
                snmtbl = @"<---- " + tk.Name + " ---->" + scrlf;
                lstdn.Add(snmtbl);
                var vcol = tk.TblKfClmns.OrderBy(o => o.NmrCl).ToList();
                var vrw = tk.TblKfRows.OrderBy(o => o.NmrRw).ToList();
                StringBuilder st1 = new StringBuilder("[*******");
                StringBuilder st2 = new StringBuilder("[*******");
                StringBuilder st3 = new StringBuilder("[*******");
                foreach (TblKfClmn tkc in vcol)
                {
                    int inmrcl = tkc.NmrCl;
                    int ptlc = tkc.Ptlk;
                    int stn = tkc.Steny;
                    int pol = tkc.Pol;
                    string sptlc = String.Format("{0:d2}", ptlc);
                    string sstn = String.Format("{0:d2}", stn);
                    string spol = String.Format("{0:d2}", pol);
                    st1.Append("#");
                    st2.Append("#");
                    st3.Append("#");
                    st1.Append(sptlc);
                    st2.Append(sstn);
                    st3.Append(spol);
                }//foreach (TblKfClmn tkc in vcol)
                st1.Append("]");
                st2.Append("]");
                st3.Append("]");
                st1.Append(scrlf);
                st2.Append(scrlf);
                st3.Append(scrlf);
                strk1 = st1.ToString();
                strk2 = st2.ToString();
                strk3 = st3.ToString();
                lstdn.Add(strk1);
                lstdn.Add(strk2);
                lstdn.Add(strk3);
                foreach (TblKfRow tkr in vrw)
                {
                    decimal ipm = tkr.IndxPm;
                    int irw = tkr.NmrRw;
                    String srw = String.Format("{0:d2}", irw);
                    //  String sipm=String.Format("{0:0.##}", ipm);
                    String sipm = String.Format("{0:0.00}", ipm);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    sb.Append(srw);
                    sb.Append("#");
                    sb.Append(sipm);
                    foreach (TblKfClmn tkc in vcol)
                    {
                        int iclmn = tkc.NmrCl;
                        
                        var vzz = tk.TblZncs.Where(t => t.NmrCl == iclmn && t.NmrRw == irw && kodtab == t.TblKfId).FirstOrDefault();

                        int znc = (vzz == null) ? 0 : vzz.Znac;
                        sb.Append("#");
                        string stz = String.Format("{0:d2}", znc);
                        sb.Append(stz);
                    }
                    sb.Append("]");
                    sb.Append(scrlf);
                    string stvyv = sb.ToString();
                    //      
                    lstdn.Add(stvyv);
                }//foreach (TblKfRow tkr in vrw)
                skntbl = "<----#KONTABL#---->" + scrlf;
                lstdn.Add(skntbl);
            }//foreach (TblKf tk in lstkf)
             //
            return lstdn;
        }



        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

	}
}