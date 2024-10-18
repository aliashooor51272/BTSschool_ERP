using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.IRepository;
using ByteTechSchoolERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ByteTechSchoolERP.DataAccess.Repository.GenericRepository
{ 
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ByteTechSchoolERPContext _db;
        internal DbSet<T> DbSet;
       
        public GenericRepository(ByteTechSchoolERPContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }
        public async Task<int> Count(Expression<Func<T, bool>> filter)
        {
            return filter != null ? await DbSet.CountAsync(filter) : await DbSet.CountAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await DbSet.AnyAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetBy(Expression<Func<T, bool>> filter)
        {
            return await DbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<T> GetById(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetByList(Expression<Func<T, bool>> filter)
        {
            return await DbSet.Where(filter).ToListAsync();
        }

        public async Task<ResponseModel> Remove(T entity)
        {
            try
            {
                DbSet.Remove(entity);
                await _db.SaveChangesAsync();
                return new ResponseModel()
                {
                    isSuccess = true,
                    Message = "Record Successfully Removed"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> RemoveRange(IEnumerable<T> entities)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    DbSet.RemoveRange(entities);
                    await transaction.CommitAsync();
                    await _db.SaveChangesAsync();
                    return new ResponseModel
                    {
                        isSuccess = true,
                        Message = "Records Successfully Removed"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new ResponseModel
                    {
                        isSuccess = false,
                        Message = ex.Message
                    };
                }
            }
        }


        public async Task<ResponseModel> UpdateRange(IEnumerable<T> entities)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                DbSet.UpdateRange(entities);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseModel
                {
                    isSuccess = true,
                    Message = "Records Successfully Updated"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseModel
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> Update(T entity)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                DbSet.Update(entity);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseModel
                {
                    isSuccess = true,
                    Message = "Record Successfully Updated"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseModel
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }



        public async Task<ResponseModel> Add(T entity)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                DbSet.Add(entity);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseModel
                {
                    isSuccess = true,
                    Message = "Record Successfully Added"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseModel
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> AddRange(IEnumerable<T> entities)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                DbSet.AddRange(entities);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseModel
                {
                    isSuccess = true,
                    Message = "Records Successfully Added"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseModel
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> Delete(object Id)
        {
            try
            {
                var model = await DbSet.FindAsync(Id);
                if (model != null)
                {
                    DbSet.Remove(model);
                    await _db.SaveChangesAsync();
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        Message = "Record Successfully Removed"
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        isSuccess = false,
                        Message = "Record Not Found"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    isSuccess = false,
                    Message = ex.Message
                };
            }
        }

    }
}
