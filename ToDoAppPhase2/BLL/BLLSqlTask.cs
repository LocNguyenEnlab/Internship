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

        public void AddTask(Task t)
        {
            _sql.AddTask(t);
        }
        
        public Task GetTask(int id)
        {
            return _sql.GetTask(id);
        }

        public List<Task> GetAllTask()
        {
            return _sql.GetAllTask();
        }

        public int GetMaxId()
        {
            return _sql.GetMaxId();
        }

        public void DeleteTask(int idTask)
        {
            _sql.DeleteTask(idTask);
        }

        public void UpdateTask(Task t)
        {
            _sql.UpdateTask(t);
        }

        /// <summary>
        /// Check duplicate task
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns>true if t already exists in list, otherwise return false</returns>
        public bool IsDuplicateTask(Task t)
        {
            var list = _sql.GetAllTask();
            foreach (var item in list)
            {
                if (item.Compare(t))
                    return true;
            }
            return false;
        }
    }
}
