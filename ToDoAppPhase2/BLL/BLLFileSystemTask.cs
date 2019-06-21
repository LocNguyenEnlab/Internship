using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoAppPhase1.DAL;
using ToDoAppPhase2;

namespace ToDoAppPhase1.BLL
{
    public class BLLFileSystemTask
    {
        private IFileSystemTaskRepository _fileSystem;


        public BLLFileSystemTask()
        {
            _fileSystem = new FileSystemTaskRepository();
        }

        public void AddTask(Task task)
        {
            _fileSystem.AddTask(task);
        }

        public int GetMaxId()
        {
            return _fileSystem.GetMaxId();
        }

        public List<Task> GetAllTask()
        {
            return _fileSystem.GetAllTask();
        }

        public Task GetTask(int taskId)
        {
            return _fileSystem.GetTask(taskId);
        }

        public void UpdateTask(Task taskId)
        {
            _fileSystem.UpdateTask(taskId);
        }

        public void DeleteTask(int taskId)
        {
            _fileSystem.DeleteTask(taskId);
        }

        public bool IsDuplicateTask(Task task)
        {
            task.Title = " " + task.Title;
            var taskList = GetAllTask();
            foreach(var item in taskList)
            {
                if (item.Compare(task))
                    return true; 
            }
            return false;
        }
    }
}
