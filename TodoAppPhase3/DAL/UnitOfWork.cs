using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public class UnitOfWork
    {
        private DbContext _context;
        private TaskRepository _taskRepository;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public TaskRepository TaskRepository
        {
            get
            {
                if (_taskRepository == null)
                {
                    _taskRepository = new TaskRepository(_context);
                }
                return _taskRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
