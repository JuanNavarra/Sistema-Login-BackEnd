namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRemoveRepository
    {
        /// <summary>
        /// Remueve un resgistro de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntity"></param>
        /// <returns></returns>
        int Remove<T>(T removeEntity) where T : class;
        /// <summary>
        /// Remueve un resgistro de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntity"></param>
        /// <returns></returns>
        Task<int> RemoveAsync<T>(T removeEntity) where T : class;
        /// <summary>
        /// Remueve varios resgistros de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntities"></param>
        /// <returns></returns>
        int Remove<T>(IEnumerable<T> removeEntities) where T : class;
        /// <summary>
        /// Remueve varios resgistros de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="removeEntities"></param>
        /// <returns></returns>
        Task<int> RemoveAsync<T>(IEnumerable<T> removeEntities) where T : class;
        /// <summary>
        /// Remueve un registro de cierta entidad con la pk como condicion sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        int Remove<T>(params object[] pks) where T : class;
        /// <summary>
        /// Remueve un registro de cierta entidad con la pk como condicion aincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pks"></param>
        /// <returns></returns>
        Task<int> RemoveAsync<T>(params object[] pks) where T : class;
    }
}
