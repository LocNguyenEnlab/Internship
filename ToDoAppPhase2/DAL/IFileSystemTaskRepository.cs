using System.Collections.Generic;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public interface IFileSystemTaskRepository
    {
        void AddTask(Task task);
        int GetMaxId();
        List<Task> GetAllTask();
        Task GetTask(int taskId);
        void UpdateTask(Task task);
        void DeleteTask(int taskId);
    }
}
