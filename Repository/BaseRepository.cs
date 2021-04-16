namespace Repository
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseRepository : IBaseRepository
    {
        #region Propiedades
        private readonly IUnitOfWork unitOfWork;
        #endregion
        #region Constructores
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion
        #region Metodos

        /// <summary>
        /// Retorna un registro obtenido de una o mas pks segun la entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        public T Find<T>(params object[] pks) where T : class
        {
            try
            {
                if (pks == null)
                    throw new ArgumentNullException(nameof(pks), $"The parameter pks can not be null");
                T result = null;
                result = unitOfWork.Context.Set<T>().Find(pks);
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna un registro obtenido de una o mas pks segun la entidad asincrono
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        public Task<T> FindAsync<T>(params object[] pks) where T : class
        {
            return Task.Run(() =>
            {
                return Find<T>(pks);
            });
        }

        /// <summary>
        /// Retorna un objeto de cierta entidad mediante una condicion de forma sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T GetOneData<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter), $"The parameter filter can not be null");

                T result = null;
                result = unitOfWork.Context.Set<T>().Where(filter).FirstOrDefault();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna un objeto de cierta entidad mediante una condicion de forma asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<T> GetOneDataAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return Task.Run(() =>
            {
                return GetOneData(filter);
            });
        }

        /// <summary>
        /// Retorna los datos de cierta entidad mediante una condicion de forma sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> GetData<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter), $"The parameter filter can not be null");

                var result = Enumerable.Empty<T>();
                result = unitOfWork.Context.Set<T>().Where(filter).ToList();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna los datos de cierta entidad mediante una condicion de forma asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetDataAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return Task.Run(() =>
            {
                return GetData(filter);
            });
        }

        /// <summary>
        /// Retorna todos los datos de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> All<T>() where T : class
        {
            IEnumerable<T> result = unitOfWork.Context.Set<T>().ToList();
            return result;
        }

        /// <summary>
        /// Retorna todos los datos de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<IEnumerable<T>> AllAsync<T>() where T : class
        {
            return Task.Run(() =>
            {
                return All<T>();
            });
        }

        /// <summary>
        /// Agrega datos de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public int Add<T>(T newEntity) where T : class
        {
            try
            {
                if (newEntity == null)
                    throw new ArgumentNullException(
                        nameof(newEntity), $"The parameter newEntity can not be null");
                var result = 0;
                unitOfWork.Context.Add(newEntity);
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega datos de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public Task<int> AddAsync<T>(T newEntity) where T : class
        {
            return Task.Run(() =>
            {
                return Add(newEntity);
            });
        }

        /// <summary>
        /// Agrega una lista de objetos de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntities"></param>
        /// <returns></returns>
        public int Add<T>(IEnumerable<T> newEntities) where T : class
        {
            try
            {
                if (newEntities == null)
                    throw new ArgumentNullException(
                        nameof(newEntities), $"The parameter newEntities can not be null");
                var result = 0;
                unitOfWork.Context.Set<T>().AddRange(newEntities);
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega una lista de objetos de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntities"></param>
        /// <returns></returns>
        public Task<int> AddAsync<T>(IEnumerable<T> newEntities) where T : class
        {
            return Task.Run(() =>
            {
                return Add(newEntities);
            });
        }

        /// <summary>
        /// Remueve un resgistro de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntity"></param>
        /// <returns></returns>
        public int Remove<T>(T removeEntity) where T : class
        {
            try
            {
                if (removeEntity == null)
                    throw new ArgumentNullException(
                        nameof(removeEntity), $"The parameter removeEntity can not be null");
                var result = 0;
                var dbSet = unitOfWork.Context.Set<T>();
                dbSet.Attach(removeEntity);
                unitOfWork.Context.Entry(removeEntity).State = EntityState.Deleted;
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Remueve un resgistro de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntity"></param>
        /// <returns></returns>
        public Task<int> RemoveAsync<T>(T removeEntity) where T : class
        {
            return Task.Run(() =>
            {
                return Remove<T>(removeEntity);
            });
        }

        /// <summary>
        /// Remueve varios resgistros de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntities"></param>
        /// <returns></returns>
        public int Remove<T>(IEnumerable<T> removeEntities) where T : class
        {
            try
            {
                if (removeEntities == null)
                    throw new ArgumentNullException(
                        nameof(removeEntities), $"The parameter removeEntities can not be null");
                var result = 0;
                var dbSet = unitOfWork.Context.Set<T>();
                foreach (var removeEntity in removeEntities)
                {
                    dbSet.Attach(removeEntity);
                    unitOfWork.Context.Entry(removeEntity).State = EntityState.Deleted;
                }
                dbSet.RemoveRange(removeEntities);
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Remueve varios resgistros de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntities"></param>
        /// <returns></returns>
        public Task<int> RemoveAsync<T>(IEnumerable<T> removeEntities) where T : class
        {
            return Task.Run(() =>
            {
                return Remove<T>(removeEntities);
            });
        }

        /// <summary>
        /// Remueve un registro de cierta entidad con la pk como condicion sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        public int Remove<T>(params object[] pks) where T : class
        {
            try
            {
                if (pks == null)
                    throw new ArgumentNullException(nameof(pks), $"The parameter removeEntity can not be null");
                var result = 0;
                var dbSet = unitOfWork.Context.Set<T>();
                var entity = Find<T>(pks);
                dbSet.Attach(entity);
                unitOfWork.Context.Entry(entity).State = EntityState.Deleted;
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Remueve un registro de cierta entidad con la pk como condicion aincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        public Task<int> RemoveAsync<T>(params object[] pks) where T : class
        {
            return Task.Run(() =>
            {
                return Remove<T>(pks);
            });
        }

        /// <summary>
        /// Actualiza los registros de una entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateEntity"></param>
        /// <returns></returns>
        public int Update<T>(T updateEntity) where T : class
        {
            try
            {
                if (updateEntity == null)
                    throw new ArgumentNullException(
                        nameof(updateEntity), $"The parameter updateEntity can not be null");
                var result = 0;
                var dbSet = unitOfWork.Context.Set<T>();
                dbSet.Attach(updateEntity);
                unitOfWork.Context.Entry(updateEntity).State = EntityState.Modified;
                result = unitOfWork.Context.SaveChanges();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza los registros de una entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateEntity"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync<T>(T updateEntity) where T : class
        {
            return Task.Run(() =>
            {
                return Update(updateEntity);
            });
        }
        #endregion
    }
}
