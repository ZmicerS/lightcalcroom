using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using LightCalcRoom.DAL.Concreate;

namespace LightCalcRoom.DAL.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> conditionLambda);
        T GetById(object Id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(Object Id);
        void Save();
    }


    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _db = null;
        private DbSet<T> dbSet = null;

        public Repository(DbContext dbcntxt)
        {
            _db = dbcntxt;
            dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> conditionLambda)
        {
           return dbSet.Where(conditionLambda);                
        }

        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
        }


        public void Delete(Object Id)
        {
            T getObjById = dbSet.Find(Id);
            if (getObjById != null)
            {
                dbSet.Remove(getObjById);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (this._db!=null)
                {
                    this._db.Dispose();
                    this._db = null;
                }
            }
        }

    }


    
    public class SvtlRepository : Repository<Svtl>
    {
        public SvtlRepository(DbContext obj) : base (obj)
        {
        }
    }

    public class SvtlGrphRepository : Repository<SvtlGrph>
    {
        public SvtlGrphRepository(DbContext obj) : base (obj)
        {  }
    }

    public class TblLmpRepository : Repository<TblLmp>
    {
        public TblLmpRepository(DbContext obj)  : base(obj)
        {}
    }


    public class TblKfRepository : Repository<TblKf>
    {
        public TblKfRepository(DbContext obj) : base(obj)
        { }
    }

    public class TblKfClmnRepository : Repository<TblKfClmn>
    {
        public TblKfClmnRepository(DbContext obj)  : base(obj)
        { }
    }


    public class TblKfRowRepository : Repository<TblKfRow>
    {
        public TblKfRowRepository(DbContext obj)  : base(obj)
        {   }
    }

    public class TblZncRepository : Repository<TblZnc>
    {
        public TblZncRepository(DbContext obj)   : base(obj)
        {        }
    }


    public class UnitOfWork : IDisposable
    {

        private bool disposed = false;
        private SvetilContext db = null;
        //
        private IRepository<Svtl> _repoSvtl = null;
        private IRepository<SvtlGrph> _repoSvtlGrph = null;
        private IRepository<TblLmp> _repoTblLmp = null;
        private IRepository<TblKf> _repoTblKf = null;
        private IRepository<TblKfClmn> _repoTblKfClmn = null;
        private IRepository<TblKfRow> _repoTblKfRow = null;
        private IRepository<TblZnc> _repoTblZnc = null;
        public UnitOfWork()
        {
            db = new SvetilContext();
        }

        public void Save()
        {
            db.SaveChanges();
        }


        public IRepository<Svtl> SvtlRepstr
        {
            get
            {
                if (_repoSvtl == null)
                {
                    _repoSvtl = new SvtlRepository(db);
                }
                return _repoSvtl;
            }
        }


        public IRepository<SvtlGrph> SvtlGrphRepstr
        {
            get
            {
                if (_repoSvtlGrph==null)
                {
                    _repoSvtlGrph = new SvtlGrphRepository(db);
                }
                return _repoSvtlGrph;
            }
        }


        public IRepository<TblLmp> TblLmpRepstr
        {
            get
            {
                if (_repoTblLmp==null)
                {
                    _repoTblLmp = new TblLmpRepository(db);
                }
                return _repoTblLmp;
            }
        }



        public IRepository<TblKf> TblKfRepstr
        {
            get
            {
                if (_repoTblKf == null)
                {
                    _repoTblKf = new TblKfRepository(db);
                }
                return _repoTblKf;
            }
        }


        public IRepository<TblKfClmn> TblKfClmnRepstr
        {
            get
            {
                if (_repoTblKfClmn == null)
                {
                    _repoTblKfClmn = new TblKfClmnRepository(db);
                }
                return _repoTblKfClmn;
            }
        }


        public IRepository<TblKfRow> TblKfRowRepstr
        {
            get
            {
                if (_repoTblKfRow == null)
                {
                    _repoTblKfRow = new TblKfRowRepository(db);
                }
                return _repoTblKfRow;
            }
        }



        public IRepository<TblZnc> TblZncRepstr
        {
            get
            {
                if (_repoTblZnc == null)
                {
                    _repoTblZnc = new TblZncRepository(db);
                }
                return _repoTblZnc;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}
