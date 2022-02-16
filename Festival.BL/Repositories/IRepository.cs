using System;
using System.Collections.Generic;

namespace Festival.BL.Repositories
{
    public interface IRepository<List, Detail>
    {
        public IEnumerable<List> GetAll();
        public Detail GetById(Guid id);
        public Detail InsertUpdate(Detail model);
        public void Delete(Guid id);
    }
}
