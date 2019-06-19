using System.Collections.Generic;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public interface ITaskRepository
    {
        void AddTask(Task t);
        List<Task> GetAllTask();
        Task GetTaskById(int id);
        void UpdateTask(Task t);
        void DeleteTask(int id);
        int GetMaxId();
    }
}
