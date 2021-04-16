namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        public CrearCurriculumnsContext Context { get; set; }
        #endregion
        #region Constructors
        public UnitOfWork(CrearCurriculumnsContext context)
        {
            this.Context = context;
        }
        #endregion
        #region Methods
        public void Commit()
        {
            Context.SaveChanges();
        }
        public void Dispose()
        {
            Context.Dispose();
        }
        #endregion
    }
}
