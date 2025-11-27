using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Reflection;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;



using OSP.Common.Repository.Utility;
using OSP.Common.Repository.Context;

namespace Common.Repository.Repository
{
    public class AbstractRepository<TEntity> where TEntity : class
    {
        #region Private Member Variables
        private OSPContext _context;
       
        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public AbstractRepository(OSPContext context)
        {
            _context = context;
        }

       
        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public virtual async Task Insert(TEntity entity)
        {

            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


            //}
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //             eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}

        }

        public virtual void Update(TEntity entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(entity); }


                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public virtual void Update(TEntity entityOldValue, TEntity entityToUpdate)
        {
            //using (var _context = new SPASv2Context())
            //{
            try
            {
                _context.Entry(entityOldValue).CurrentValues.SetValues(entityToUpdate);
                _context.SaveChanges();
                //string errormessage = Utility.GetEntityException(_context);
                //if (errormessage != string.Empty)
                //{
                //    _context.Entry(entityOldValue).State = EntityState.Unchanged;
                //    throw new Exception(errormessage);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //}

        }

        public virtual void Delete(object id)
        {
            //using (var _context = new SPASv2Context())
            //{
            try
            {
                var Entity = _context.Set<TEntity>().Find(id);
                if (Entity != null)
                {
                    if (_context.Entry(Entity).State == EntityState.Detached)
                    { _context.Set<TEntity>().Attach(Entity); }

                    _context.Set<TEntity>().Remove(Entity);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //}
        }

        public virtual void Delete(TEntity Entity)
        {
            //using (var _context = new SPASv2Context())
            //{
            try
            {
                _context.Set<TEntity>().Remove(Entity);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //}
        }

        public virtual void DeleteByComposite(object id1, object id2)
        {
            //using (var _context = new SPASv2Context())
            //{
            var Entity = _context.Set<TEntity>().Find(id1, id2);
            if (Entity != null)
            {
                if (_context.Entry(Entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(Entity); }

                _context.Set<TEntity>().Remove(Entity);
                _context.SaveChanges();
            }
            //}
        }

        public virtual void DeleteByComposite(object id, object id2, object id3)
        {
            //using (var _context = new SPASv2Context())
            //{
            var Entity = _context.Set<TEntity>().Find(id, id2, id3);
            if (Entity != null)
            {
                if (_context.Entry(Entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(Entity); }

                _context.Set<TEntity>().Remove(Entity);
                _context.SaveChanges();
            }

            //}

        }

        public virtual void DeleteByComposite(object id, object id2, object id3, object id4)
        {
            //using (var _context = new SPASv2Context())
            //{
            var Entity = _context.Set<TEntity>().Find(id, id2, id3, id4);
            if (Entity != null)
            {
                if (_context.Entry(Entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(Entity); }

                _context.Set<TEntity>().Remove(Entity);
                _context.SaveChanges();
            }
            //}
        }

        #endregion

        #region Public Function

        public virtual TEntity GetByID(object id)
        {

            return _context.Set<TEntity>().Find(id);


        }

        public virtual TEntity GetByCompositeID(object id1, object id2)
        {

            return _context.Set<TEntity>().Find(id1, id2);

        }

        public virtual TEntity GetByCompositeID(object id1, object id2, object id3)
        {

            return _context.Set<TEntity>().Find(id1, id2, id3);

        }

        public virtual TEntity GetByCompositeID(object id1, object id2, object id3, object id4)
        {

            return _context.Set<TEntity>().Find(id1, id2, id3, id4);

        }

        public virtual IList<TEntity> GetTEntity()
        {
            return _context.Set<TEntity>().ToList();
        }

        #endregion


    }
}
