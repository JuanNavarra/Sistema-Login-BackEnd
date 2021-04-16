namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGetRepository
    {
        /// <summary>
        /// Retorna un registro obtenido de una o mas pks segun la entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        T Find<T>(params object[] pks) where T : class;
        /// <summary>
        /// Retorna un registro obtenido de una o mas pks segun la entidad asincrono
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(params object[] pks) where T : class;
        /// <summary>
        /// Retorna los datos de cierta entidad mediante una condicion de forma sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<T> GetData<T>(Expression<Func<T, bool>> filter) where T : class;
        /// <summary>
        /// Retorna los datos de cierta entidad mediante una condicion de forma asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetDataAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        /// <summary>
        /// Retorna todos los datos de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> All<T>() where T : class;
        /// <summary>
        /// Retorna todos los datos de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> AllAsync<T>() where T : class;
        /// <summary>
        /// Retorna un objeto de cierta entidad mediante una condicion de forma asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<T> GetOneDataAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        /// <summary>
        /// Retorna un objeto de cierta entidad mediante una condicion de forma sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        T GetOneData<T>(Expression<Func<T, bool>> filter) where T : class;
    }
}
