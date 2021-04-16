namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAddRepository
    {
        /// <summary>
        /// Agrega datos de cierta entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        int Add<T>(T newEntity) where T : class;
        /// <summary>
        /// Agrega datos de cierta entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        Task<int> AddAsync<T>(T newEntity) where T : class;
    }
}
