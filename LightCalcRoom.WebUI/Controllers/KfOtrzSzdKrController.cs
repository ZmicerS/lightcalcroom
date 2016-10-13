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
    public class KfOtrzSzdKrController : Controller
    {
        UnitOfWork unitOfWork;

        public KfOtrzSzdKrController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: KfOtrzSzdKr
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IsprStrctKfOtrz(int? id)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}