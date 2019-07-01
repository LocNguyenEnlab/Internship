using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public class FileSystemTaskRepository : IFileSystemTaskRepository
    {
        private string _path;

        public FileSystemTaskRepository()
        {
            _path = @"TodoAppPhase2.txt";
            if (!File.Exists(_path))
            {
                var file = File.Create(_path);
                file.Close();
                File.WriteAllText(_path, "MaxID: 0\n");
            }
        }

        public void AddTask(Task task)
        {
            var addLine = string.Format("Id: {0}, Title: {1}, Description: {2}, TimeCreate: {3}, TypeList: {4}",
                task.Id, task.Title, task.Description, task.TimeCreate.ToString(), task.TypeList);
            var lines = File.ReadAllLines(_path);
            int id = GetMaxId() + 1;

            //delete file contents
            StreamWriter streamWriter = File.CreateText(_path);
            streamWriter.Flush();
            streamWriter.Close();
            
            using (StreamWriter streamWriter1 = File.AppendText(_path))
            {
                streamWriter1.WriteLine(addLine);
                for (int i = 0; i < lines.Count()-1; i++)
                {
                    streamWriter1.WriteLine(lines[i]);
                }
                streamWriter1.WriteLine("MaxID: {0}", id);
            }
        }

        public int GetMaxId()
        {
            var lines = File.ReadAllLines(_path);
            var finalLine = lines[lines.Count()-1];
            var finalLineSplit = finalLine.Split(':');
            int maxId = Convert.ToInt32(finalLineSplit[1]);
            return maxId;
        }

        public List<Task> GetAllTask()
        {
            var taskList = new List<Task>();
            var lines = File.ReadAllLines(_path);
            for (int i = 0; i < lines.Count()-1; i++)
            {
                var task = new Task();
                var lineSplit = lines[i].Split(':', ',');
                task.Id = Convert.ToInt32(lineSplit[1]);
                task.Title = lineSplit[3]; 
                task.Description = lineSplit[5];
                string time = lineSplit[7] + ":" + lineSplit[8] + ":" + lineSplit[9];
                task.TypeList = Convert.ToInt32(lineSplit[11]);
                task.TimeCreate = Convert.ToDateTime(time);
                taskList.Add(task);
            }
            return taskList;
        }

        public Task GetTask(int taskId)
        {
            var taskList = GetAllTask();
            return taskList.FirstOrDefault(s => s.Id == taskId);
        }

        /// <summary>
        /// idea: delete file contents and rewrite file contents after update 
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(Task task)
        {
            var lines = File.ReadAllLines(_path);
            //delete file contents
            StreamWriter streamWriter = File.CreateText(_path);
            streamWriter.Flush();
            streamWriter.Close();
            foreach(var item in lines)
            {
                int taskId = Convert.ToInt32(item.Split(':', ',')[1]);
                if (task.Id == taskId) 
                {
                    var updateLine = string.Format("Id: {0}, Title: {1}, Description: {2}, TimeCreate: {3}, TypeList: {4}", 
                        task.Id, task.Title, task.Description, task.TimeCreate.ToString(), task.TypeList);
                    using (StreamWriter streamWriter1 = File.AppendText(_path))
                    {
                        streamWriter1.WriteLine(updateLine);
                    }
                }
                else
                {
                    using (StreamWriter streamWriter1 = File.AppendText(_path))
                    {
                        streamWriter1.WriteLine(item);
                    }
                }
            }
        }

        /// <summary>
        /// idea: delete file contents and rewrite file contents ignore the task was selected
        /// </summary>
        /// <param name="taskId"></param>
        public void DeleteTask(int taskId)
        {
            var lines = File.ReadAllLines(_path);
            //delete file contents
            StreamWriter streamWriter = File.CreateText(_path);
            streamWriter.Flush();
            streamWriter.Close();

            foreach (var line in lines)
            {
                int id = Convert.ToInt32(line.Split(':', ',')[1]);
                if (taskId != id)
                {
                    using (StreamWriter streamWriter1 = File.AppendText(_path))
                    {
                        streamWriter1.WriteLine(line);
                    }
                }
            }
        }
    }
}
