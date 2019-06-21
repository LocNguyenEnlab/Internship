using System.Collections.Generic;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3.DAL
{
    public interface ITaskRepository
    {
        void AddTask(Task task);
        List<Task> GetAllTask();
        Task GetTask(int taskId);
        void UpdateTask(Task task);
        void DeleteTask(int taskId);
        int GetMaxId();
    }
}
