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
            if (list.FirstOrDefault(s => s.Title == task.Title && s.Description == task.Description) != null)
            {
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
            return _unitOfWork.TaskRepository.Get(taskId);
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

        public bool IsEmptyAuthor(Author author)
        {
            if (author.AuthorName == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDuplicateAuthor(Author author)
        {
            var list = GetAllAuthors();
            if (list.FirstOrDefault(s => s.AuthorName == author.AuthorName) != null)
            {
                return true;
            }
            return false;
        }

        internal Author GetAuthor(int id)
        {
            return _unitOfWork.AuthorRepository.Get(id);
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
    }
}
