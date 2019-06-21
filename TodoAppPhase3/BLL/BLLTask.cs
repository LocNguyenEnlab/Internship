using System.Collections.Generic;
using TodoAppPhase3.DAL;

namespace TodoAppPhase3.BLL
{
    public class BLLTask
    {
        private UnitOfWork _uow;

        public BLLTask()
        {
            _uow = new UnitOfWork(new DbContext());
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
            return _uow.TaskRepository.GetAllTask();
        }

        internal void DeleteTask(int taskId)
        {
            _uow.TaskRepository.DeleteTask(taskId);
        }

        internal bool IsDuplicateTask(Task task)
        {
            var list = GetAllTask();
            foreach (var item in list)
            {
                if (item.Title == task.Title && item.Description == task.Description)
                    return true;
            }
            return false;
        }

        internal void AddTask(Task task)
        {
            _uow.TaskRepository.AddTask(task);
        }

        internal int GetMaxId()
        {
            return _uow.TaskRepository.GetMaxId();
        }

        internal Task GetTask(int taskId)
        {
            return _uow.TaskRepository.GetTask(taskId);
        }

        internal void UpdateTask(Task task)
        {
            _uow.TaskRepository.UpdateTask(task);
        }

        internal void Commit()
        {
            _uow.Commit();
        }
    }
}
