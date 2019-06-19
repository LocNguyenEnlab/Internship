using System.Collections.Generic;
using System.Linq;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public class TaskRepository : ITaskRepository
    {
        public void AddTask(Task t)
        {
            using (var context = new TaskDbContext())
            {
                context.Task.Add(t);
                context.SaveChanges();
            }
        }
        public List<Task> GetAllTask()
        {
            using (var context = new TaskDbContext())
            {
                return context.Task.ToList();
            }
        }
        public Task GetTaskById(int id)
        {
            using (var context = new TaskDbContext())
            {
                return context.Task.Where(s => s.Id == id).SingleOrDefault();
            }
        }
        public void UpdateTask(Task t)
        {
            using (var context = new TaskDbContext())
            {
                var task = context.Task.Where(s => s.Id == t.Id).SingleOrDefault();
                context.Entry(task).CurrentValues.SetValues(t);
                context.SaveChanges();
            }
        }

        public void DeleteTask(int id)
        {
            using (var context = new TaskDbContext())
            {
                var t = context.Task.Where(s => s.Id == id).SingleOrDefault();
                context.Task.Remove(t);
                context.SaveChanges();
            }
        }

        public int GetMaxId()
        {
            using (var context = new TaskDbContext())
            {
                return context.Task.OrderByDescending(s => s.Id).FirstOrDefault().Id;
            }
        }
    }
}
