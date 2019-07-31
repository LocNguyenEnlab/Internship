using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppPhase3.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        int GetMaxId(Func<T, decimal> columnSelector);
    }
}
