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

        internal void DeleteTask(int idTask)
        {
            _uow.TaskRepository.DeleteTask(idTask);
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
            _uow.TaskRepository.AddTask(t);
        }

        internal int GetMaxId()
        {
            return _uow.TaskRepository.GetMaxId();
        }

        internal Task GetTask(int id)
        {
            return _uow.TaskRepository.GetTask(id);
        }

        internal void UpdateTask(Task t)
        {
            _uow.TaskRepository.UpdateTask(t);
        }

        internal void Commit()
        {
            _uow.Commit();
        }
    }
}
