using System.Collections.Generic;
using System.Linq;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public class TaskRepository : ITaskRepository
    {
        private DbContext _context;

        public TaskRepository(DbContext context)
        {
            _context = context;
        }

        public void AddTask(Task t)
        {
            _context.Task.Add(t);
        }

        public List<Task> GetAllTask()
        {
            return _context.Task.ToList();
        }

        public Task GetTask(int id)
        {
            return _context.Task.Where(s => s.Id == id).SingleOrDefault();
        }

        public void UpdateTask(Task t)
        {
            var task = _context.Task.Where(s => s.Id == t.Id).SingleOrDefault();
            _context.Entry(task).CurrentValues.SetValues(t);
        }

        public void DeleteTask(int id)
        {
            var t = _context.Task.Where(s => s.Id == id).SingleOrDefault();
            _context.Task.Remove(t);
        }

        public int GetMaxId()
        {
            return _context.Task.OrderByDescending(s => s.Id).FirstOrDefault().Id;
        }
    }
}
