using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DataContext _context;
        private DbSet<T> _table;

        public GenericRepository()
        {
            _context = new DataContext();
            _table = _context.Set<T>();
        }
            
        public GenericRepository(DataContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(object id)
        {
            return _table.Find(id);
        }

        public void Add(T obj)
        {
            _table.Add(obj);
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public int GetMaxId(Func<T, decimal> columnSelector)
        {
            return (int)_table.Max(columnSelector);
        }
    }
}
