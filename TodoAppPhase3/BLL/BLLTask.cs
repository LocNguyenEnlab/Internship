using System.Collections.Generic;
using TodoAppPhase3.DAL;

namespace TodoAppPhase3.BLL
{
    public class BLLTask
    {
        private ITaskRepository _taskRepo;

        public BLLTask()
        {
            _taskRepo = new TaskRepository();
        }

        internal bool IsEmpty(Task task)
        {
            if (task.Title == "" || task.Description == "")
                return true;
            else
                return false;
        }

        internal List<Task> GetAllTask()
        {
            return _taskRepo.GetAllTask();
        }

        internal void DeleteTask(int idTask)
        {
            _taskRepo.DeleteTask(idTask);
        }

        internal bool IsDuplicateTask(Task t)
        {
            var list = GetAllTask();
            foreach (var item in list)
            {
                if (item.Title == t.Title && item.Description == t.Description)
                    return true;
            }
            return false;
        }

        internal void AddTask(Task t)
        {
            _taskRepo.AddTask(t);
        }

        internal int GetMaxId()
        {
            return _taskRepo.GetMaxId();
        }

        internal Task GetTaskById(int id)
        {
            return _taskRepo.GetTaskById(id);
        }

        internal void UpdateTask(Task t)
        {
            _taskRepo.UpdateTask(t);
        }
    }
}
