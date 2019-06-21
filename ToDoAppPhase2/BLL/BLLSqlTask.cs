using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppPhase1.DAL;
using ToDoAppPhase2;

namespace ToDoAppPhase1.BLL
{
    public class BLLSqlTask
    {
        private ISqlTaskRepository _sql;


        public BLLSqlTask () {
            _sql = new SqlTaskRepository();
        }

        public void AddTask(Task task)
        {
            _sql.AddTask(task);
        }
        
        public Task GetTask(int taskId)
        {
            return _sql.GetTask(taskId);
        }

        public List<Task> GetAllTask()
        {
            return _sql.GetAllTask();
        }

        public int GetMaxId()
        {
            return _sql.GetMaxId();
        }

        public void DeleteTask(int taskId)
        {
            _sql.DeleteTask(taskId);
        }

        public void UpdateTask(Task task)
        {
            _sql.UpdateTask(task);
        }

        /// <summary>
        /// Check duplicate task
        /// </summary>
        /// <param name="list"></param>
        /// <param name="task"></param>
        /// <returns>true if t already exists in list, otherwise return false</returns>
        public bool IsDuplicateTask(Task task)
        {
            var taskList = _sql.GetAllTask();
            foreach (var item in taskList)
            {
                if (item.Compare(task))
                    return true;
            }
            return false;
        }
    }
}
