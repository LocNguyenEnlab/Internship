using System.Collections.Generic;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public interface ISqlTaskRepository
    {
        void AddTask(Task t);
        void UpdateTask(Task t);
        int GetMaxId();
        void DeleteTask(int idTask);
        Task GetTask(int id);
        List<Task> GetAllTask();

    }
}
