using System;
namespace ATS.Data
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>        
        void Save();
    }
}
