using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public class UnitOfWork
    {
        private DataContext _context;
        private GenericRepository<Task> _taskRepository;
        private GenericRepository<Author> _authorRepository;
        private DbContextTransaction _transaction;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public GenericRepository<Task> TaskRepository
        {
            get
            {
                if (_taskRepository == null)
                {
                    _taskRepository = new GenericRepository<Task>(_context);
                }
                return _taskRepository;
            }
        }

        public GenericRepository<Author> AuthorRepository
        {
            get
            {
                if(_authorRepository == null)
                {
                    _authorRepository = new GenericRepository<Author>(_context);
                }
                return _authorRepository;
            }
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.SaveChanges();
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }
        
        public void RollBack()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }
    }
}
