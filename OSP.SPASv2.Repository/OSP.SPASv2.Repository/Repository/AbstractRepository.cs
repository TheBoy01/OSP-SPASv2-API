using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Reflection;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using SPASv2.Context;

using EFCore.BulkExtensions;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Repository.Context;
using System.ComponentModel.DataAnnotations;

namespace SPASv2.Repository.Repository
{
    public class AbstractRepository<TEntity> where TEntity : class
    {
        #region Private Member Variables
        private SPASv2Context _context;
        private SPASv1Context _V1Context;
        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public AbstractRepository(SPASv2Context context)
        {
            _context = context;
        }
        public AbstractRepository(SPASv1Context V1Context)
        {
            _V1Context = V1Context;
        }
        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public virtual async Task BulkInsert(List<TEntity> entities)
        {
            try
            {

                if (_context == null)
                {
                    await _V1Context.BulkInsertAsync(entities);
                }
                else
                {
                    await _context.BulkInsertAsync(entities);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception($"Inner Exception: {ex.InnerException.Message}");
                }
                else
                {
                    throw new Exception("An error occurred during the bulk insert operation.", ex);
                }

                
               
            }
        }

        public virtual async Task Insert(TEntity entity)
        {

            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(entity, null, null);

                //if (!Validator.TryValidateObject(entity, validationContext, validationResults, true))
                //{
                //    throw new ValidationException("Entity validation failed.");
                //}
                if (!Validator.TryValidateObject(entity, validationContext, validationResults, true))
                {
                    var errors = validationResults.ToDictionary(
                        vr => string.Join(", ", vr.MemberNames),  // Property names
                        vr => vr.ErrorMessage                     // Validation error message
                    );

                    throw new ValidationException("Entity validation failed: " + string.Join("; ", errors.Select(e => $"{e.Key}: {e.Value}")));
                }

                if (_context == null)
                {
                    _V1Context.Set<TEntity>().Add(entity);

                    await _V1Context.SaveChangesAsync();
                }
                else
                {
                    _context.Set<TEntity>().Add(entity);
                    
                    await _context.SaveChangesAsync();
                }
            }
            catch (ValidationException ex)
            {

                throw new ValidationException(ex.Message);
            }
            catch (DbUpdateException ex)
            {

                throw new DbUpdateException(ex.Message);
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

        //   public virtual void Update(TEntity entity)
        public virtual async Task Update(TEntity entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(entity); }


                _context.Entry(entity).State = EntityState.Modified;
               await _context.SaveChangesAsync();



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

        public virtual async Task Delete(TEntity Entity)
        {
            //using (var _context = new SPASv2Context())
            //{
            try
            {
                 _context.Set<TEntity>().Remove(Entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //}
        }

        public virtual async Task DeleteByComposite(object id1, object id2)
        {
            //using (var _context = new SPASv2Context())
            //{
            var Entity = _context.Set<TEntity>().Find(id1, id2);
            if (Entity != null)
            {
                if (_context.Entry(Entity).State == EntityState.Detached)
                { _context.Set<TEntity>().Attach(Entity); }

                _context.Set<TEntity>().Remove(Entity);
               await _context.SaveChangesAsync();
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

        public virtual  async Task<TEntity> GetByID(object id)
        {

            return  await _context.Set<TEntity>().FindAsync(id);


        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
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
