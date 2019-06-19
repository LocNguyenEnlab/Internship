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

        public void AddTask(Task t)
        {            
            _cnn.Open();
            var sql = string.Format("insert into Task (Title, Description, TypeList, TimeCreate) " +
                "values (N'{0}', N'{1}', {2}, '{3}')", t.Title, t.Description, t.TypeList, t.TimeCreate);
            var cmd = new SqlCommand(sql, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public void UpdateTask(Task t)
        {
            _cnn.Open();
            var sql = string.Format("update Task set Title = N'{0}', Description = N'{1}', TypeList = {2} where Id = {3}",
                t.Title, t.Description, t.TypeList, t.Id);
            var cmd = new SqlCommand(sql, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public int GetMaxId()
        {
            int maxId = 0;
            _cnn.Open();
            var sql = "select Max(Id) from Task";
            var cmd = new SqlCommand(sql, _cnn);
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
            cmd.Clone();
            _cnn.Close();
            return maxId;
        }

        public void DeleteTask(int idTask)
        {
            _cnn.Open();
            var sql = string.Format("delete Task where Id = {0}", idTask);
            var cmd = new SqlCommand(sql, _cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _cnn.Close();
        }

        public Task GetTask(int id)
        {
            var t = new Task();
            _cnn.Open();
            var sql = string.Format("select * from Task where Id = {0}", id);
            var cmd = new SqlCommand(sql, _cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                t.Id = Convert.ToInt32(reader["Id"]);
                t.Title = reader["Title"].ToString();
                t.Description = reader["Description"].ToString();
                t.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
            }
            reader.Close();
            cmd.Dispose();
            _cnn.Close();
            return t;
        }

        public List<Task> GetAllTask()
        {
            var list = new List<Task>();
            _cnn.Open();
            var sql = string.Format("select * from Task");
            var cmd = new SqlCommand(sql, _cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var t = new Task();
                t.Id = Convert.ToInt32(reader["Id"]);
                t.Title = reader["Title"].ToString();
                t.Description = reader["Description"].ToString();
                t.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
                t.TypeList = Convert.ToInt32(reader["TypeList"]);
                list.Add(t);
            }
            reader.Close();
            cmd.Dispose();
            _cnn.Close();
            return list;
        }
    }
}
