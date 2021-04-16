namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBaseRepository : IAddRepository, IRemoveRepository, 
        IUpdateRepository, IGetRepository
    {
    }
}
