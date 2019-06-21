using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public class SqlTaskRepository : ISqlTaskRepository
    {
        private string _connectionString;
        private SqlConnection _cnn;
        

        public SqlTaskRepository()
        {
            //connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TodoAppPhase2;Integrated Security=True";
            _connectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            _cnn = new SqlConnection(_connectionString);
        }

        public void AddTask(Task task)
        {            
            _cnn.Open();
            var query = string.Format("insert into Task (Title, Description, TypeList, TimeCreate) " +
                "values (N'{0}', N'{1}', {2}, '{3}')", task.Title, task.Description, task.TypeList, task.TimeCreate);
            var cmd = new SqlCommand(query, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public void UpdateTask(Task task)
        {
            _cnn.Open();
            var query = string.Format("update Task set Title = N'{0}', Description = N'{1}', TypeList = {2} where Id = {3}",
                task.Title, task.Description, task.TypeList, task.Id);
            var cmd = new SqlCommand(query, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public int GetMaxId()
        {
            int maxId = 0;
            _cnn.Open();
            var query = "select Max(Id) from Task";
            var cmd = new SqlCommand(query, _cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    maxId = Convert.ToInt32(reader[0]);
                }
                catch (Exception)
                {
                    maxId = -1;
                }
            }
            reader.Close();
            cmd.Dispose();
            _cnn.Close();
            return maxId;
        }

        public void DeleteTask(int taskId)
        {
            _cnn.Open();
            var query = string.Format("delete Task where Id = {0}", taskId);
            var cmd = new SqlCommand(query, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public Task GetTask(int taskId)
        {
            var task = new Task();
            _cnn.Open();
            var query = string.Format("select * from Task where Id = {0}", taskId);
            var cmd = new SqlCommand(query, _cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                task.Id = Convert.ToInt32(reader["Id"]);
                task.Title = reader["Title"].ToString();
                task.Description = reader["Description"].ToString();
                task.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
            }
            reader.Close();
            cmd.Dispose();
            _cnn.Close();
            return task;
        }

        public List<Task> GetAllTask()
        {
            var taskList = new List<Task>();
            _cnn.Open();
            var query = string.Format("select * from Task");
            var cmd = new SqlCommand(query, _cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var task = new Task();
                task.Id = Convert.ToInt32(reader["Id"]);
                task.Title = reader["Title"].ToString();
                task.Description = reader["Description"].ToString();
                task.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
                task.TypeList = Convert.ToInt32(reader["TypeList"]);
                taskList.Add(task);
            }
            reader.Close();
            cmd.Dispose();
            _cnn.Close();
            return taskList;
        }
    }
}
