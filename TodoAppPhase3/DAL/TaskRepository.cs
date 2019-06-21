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

        public void AddTask(Task task)
        {
            _context.Task.Add(task);
        }

        public List<Task> GetAllTask()
        {
            return _context.Task.ToList();
        }

        public Task GetTask(int taskId)
        {
            return _context.Task.SingleOrDefault(s => s.Id == taskId);
        }

        public void UpdateTask(Task task)
        {
            //var task = _context.Task.SingleOrDefault(s => s.Id == task.Id);
            _context.Entry(task).CurrentValues.SetValues(task);
        }

        public void DeleteTask(int taskId)
        {
            var t = _context.Task.SingleOrDefault(s => s.Id == taskId);
            _context.Task.Remove(t);
        }

        public int GetMaxId()
        {
            return _context.Task.OrderByDescending(s => s.Id).FirstOrDefault().Id;
        }
    }
}
