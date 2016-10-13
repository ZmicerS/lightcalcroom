using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace LightCalcRoom.WebUI.Models
{
 /*   public class ViewModel
    {
    }
  */

      public class SvtlUI
    {
        public int Id { get; set; }
       [Required]
        public string Name { get; set; }
    }


    public class TblLmpUI
    {
        public int Id { get; set; }
        public int NmrStrk { get; set; }
        public string Name { get; set; }
        public int Wt { get; set; }
        public int Lumen { get; set; }
        public int SvtlId { get; set; }
     //   public virtual Svtl Svtl { get; set; }// новигационное свойство обязательно для создания FOREIGN KEY
    }




    public class TblKfUI
    {
        // public int TblKfId { get; set; }
        public int Id { get; set; }
        //  public int SvtlId { get; set; }
        public int NmrStrk { get; set; }

        public int F883 { get; set; }
        public int F853 { get; set; }
        public int F831 { get; set; }
        public int F752 { get; set; }
        public int F551 { get; set; }
        public int F531 { get; set; }
        public int F331 { get; set; }
        public int F000 { get; set; }
        public decimal IndxPm { get; set; }//modelBuilder.Entity<Phone>().Property(p => p.Price).HasPrecision(15,2);

        public int SvtlId { get; set; }
 //       public virtual Svtl Svtl { get; set; }
        public TblKfUI()
        {
            F883 = 0;
            F853 = 0;
            F831 = 0;
            F752 = 0;
            F551 = 0;
            F531 = 0;
            F331 = 0;
            F000 = 0;
            IndxPm = 0.0M;
        }
    }


    public class SvtlExUI
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Pls Select")]
        [Range(1,int.MaxValue)]
        public int TblKfId { get; set; }
        public string Svtlnk { get; set; }
        public string Lampa { get; set; }
        public int Kol { get; set; }
        public int Pwr { get; set; }
        public int Potok { get; set; }
       // [Required]
    //    public string Name { get; set; }
     //   public virtual ICollection<SelectListItem> TblKfSl { get; set; }
     //   public IEnumerable<SelectListItem> Genres { get; set; }
    }


    public class TbKfUI
    {
        public int Id { get; set; }

        public int [,] MsKf{set; get; }

        public decimal[] KfIsp { set; get; }
            
    }

    public class TbKZncUI
    {
        public int Id { get; set; }
        public int Znac { get; set; }
        
        public int NmrRw { get; set; }
        public int NmrCl { get; set; }
    }



     public class TblKfRowUI
     {
         public int Id { get; set; }
         public int NmrRw { get; set; }
         public decimal IndxPm { get; set; }
         public int TblKfId { get; set; }
     }


     public class TblKfClmnUI
     {
         public int Id { get; set; }

         public int NmrCl { get; set; }
         public int Ptlk { get; set; }
         public int Steny { get; set; }

         public int Pol { get; set; }
         public int TblKfId { get; set; }
     }


     public class TbKfExOtbrUI
     {
         public int Id { get; set; }
         public string Nazva { set; get; }

         public int  Kolstr {set;get;}
         public int Kolcln { set; get; }

         public string[,] MsKfOtrz { set; get; }
         public string[] MsIndPm { set; get; }
         public string[,] MsKf { set; get; }

        

     }

}

