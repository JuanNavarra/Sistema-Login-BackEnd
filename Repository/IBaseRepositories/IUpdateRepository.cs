namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUpdateRepository
    {
        /// <summary>
        /// Actualiza los registros de una entidad sincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateEntity"></param>
        /// <returns></returns>
        int Update<T>(T updateEntity) where T : class;
        /// <summary>
        /// Actualiza los registros de una entidad asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(T updateEntity) where T : class;
    }
}
