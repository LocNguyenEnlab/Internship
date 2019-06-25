using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppPhase3.DAL;

namespace TodoAppPhase3.BLL
{
    public class BusinessLogic
    {
        private UnitOfWork _unitOfWork;

        public BusinessLogic()
        {
            _unitOfWork = new UnitOfWork(new DataContext());
        }

        #region Task

        internal bool IsEmptyTask(Task task)
        {
            if (task.Title == "" || task.Description == "")
                return true;
            else
                return false;
        }

        internal List<Task> GetAllTask()
        {
            return _unitOfWork.TaskRepository.GetAll().ToList();
        }

        internal void DeleteTask(int taskId)
        {
            _unitOfWork.TaskRepository.Delete(taskId);
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
            _unitOfWork.TaskRepository.Add(task);
        }

        internal int GetMaxTaskId()
        {
            return _unitOfWork.TaskRepository.GetMaxId(s => s.Id);
        }

        internal Task GetTask(int taskId)
        {
            return _unitOfWork.TaskRepository.GetById(taskId);
        }

        internal void UpdateTask(Task task)
        {
            _unitOfWork.TaskRepository.Update(task);
        }
        #endregion

        #region Author
        public void AddAuthor(Author author)
        {
            _unitOfWork.AuthorRepository.Add(author);
            _unitOfWork.AuthorRepository.Save();
        }

        public List<Author> GetAllAuthors()
        {
            return _unitOfWork.AuthorRepository.GetAll().ToList();
        }

        public int GetMaxAuthorId()
        {
            return _unitOfWork.AuthorRepository.GetMaxId(s => s.Id);
        }
        #endregion

        internal void CreateTransaction()
        {
            _unitOfWork.CreateTransaction();
        }

        internal void Commit()
        {
            _unitOfWork.Commit();
        }

        internal void RollBack()
        {
            _unitOfWork.RollBack();
        }

        internal Author GetAuthor(int id)
        {
            return _unitOfWork.AuthorRepository.GetById(id);
        }
    }
}
