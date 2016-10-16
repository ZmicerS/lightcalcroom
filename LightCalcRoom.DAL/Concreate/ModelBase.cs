using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace LightCalcRoom.DAL.Concreate
{
    public class Svtl
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TblKfId { get; set; }//  // будет  ON DELETE CASCADE
        public virtual TblKf TblKf { get; set; }
        public virtual ICollection<TblLmp> TblLmps { get; set; }
        public Svtl()
        {
            TblLmps = new List<TblLmp>();
        }
    }

    public class SvtlGrph
    {
        public int Id { get; set; }
        public decimal Dln { get; set; }
        public decimal Shir { get; set; }
        public decimal Vst { get; set; }
        //New Property to Store Image
        public byte[] Image { get; set; }
        public int SvtlId { get; set; }
        public virtual Svtl Svtl { get; set; }
    }


    public class TblLmp //уже иногда в светильниках могут встречаться разные лампы
    {        
        public int Id { get; set; }
        public int? NmrStrk { get; set; }
        public string Name { get; set; }
        public int Wt { get; set; }
        public int Lumen { get; set; }

        public int KlLmp { get; set; }        
        public int? SvtlId { get; set; }
        public virtual Svtl Svtl { get; set; }
    }

    public class TblKf
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public virtual ICollection<Svtl> Svtls { get; set; }
        
        public virtual ICollection<TblKfClmn> TblKfClmns { get; set; }
        public virtual ICollection<TblKfRow> TblKfRows { get; set; }
        public virtual ICollection<TblZnc> TblZncs { get; set; }
        public TblKf()
        {
            Svtls = new List<Svtl>();
            TblKfClmns = new List<TblKfClmn>();
            TblKfRows = new List<TblKfRow>();
            TblZncs = new List<TblZnc>();
        }
    }


    public class TblKfClmn
    {
        public int Id { get; set; }

        public int NmrCl { get; set; }
        public int Ptlk { get; set; }
        public int Steny { get; set; }

        public int Pol { get; set; }

        public virtual ICollection<TblZnc> TblZncs { get; set; }
        public int TblKfId { get; set; }//  // будет  ON DELETE CASCADE
        public virtual TblKf TblKf { get; set; }

        public TblKfClmn()
        {
            TblZncs = new List<TblZnc>();
        }
    }

    public class TblKfRow
    {
        public int Id { get; set; }
        public int NmrRw { get; set; }
        public decimal IndxPm { get; set; }//modelBuilder.Entity<Phone>().Property(p => p.Price).HasPrecision(15,2);
        public virtual ICollection<TblZnc> TblZncs { get; set; }
        public int TblKfId { get; set; }//  // будет  ON DELETE CASCADE
        public virtual TblKf TblKf { get; set; }
        public TblKfRow()
        {
            TblZncs = new List<TblZnc>();
        }
    }

    public class TblZnc
    {
        public int Id { get; set; }
        public int Znac { get; set; }
        public int NmrCl { get; set; }
        public int NmrRw { get; set; }
        public int? TblKfRowId { get; set; }  // будет создаваться внешний ключ
        public virtual TblKfRow TblKfRow { get; set; }//для foreign key без каскадного удаления

        public int? TblKfClmnId { get; set; }  // будет  ON DELETE CASCADE
        public virtual TblKfClmn TblKfClmn { get; set; }//для foreign key без каскадного удаления

        public int TblKfId { get; set; }//  // будет  ON DELETE CASCADE
        public virtual TblKf TblKf { get; set; }
    }


    public class SvetilContext : DbContext
    {
        public SvetilContext() : base("name=LightCalcRoom2") { }
        public DbSet<Svtl> Svtls { get; set; }
        public DbSet<SvtlGrph> SvtlGrphs { get; set; }
        public DbSet<TblKf> TblKfs { get; set; }
        public DbSet<TblKfClmn> TblKfClmns { get; set; }
        public DbSet<TblKfRow> TblKfRows { get; set; }
        public DbSet<TblZnc> TblZncs { get; set; }
        public DbSet<TblLmp> TblLmps { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }



   public class SvetilContextInitializer : CreateDatabaseIfNotExists<SvetilContext>
   {
       protected override void Seed(SvetilContext context)
       {
           

       }
   }

}
