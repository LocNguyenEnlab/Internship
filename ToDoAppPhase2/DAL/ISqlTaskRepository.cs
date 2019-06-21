using System.Collections.Generic;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public interface ISqlTaskRepository
    {
        void AddTask(Task task);
        void UpdateTask(Task task);
        int GetMaxId();
        void DeleteTask(int taskId);
        Task GetTask(int taskId);
        List<Task> GetAllTask();

    }
}
