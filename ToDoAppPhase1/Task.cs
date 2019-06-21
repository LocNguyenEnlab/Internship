using System;

namespace ToDoAppPhase1
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeCreate { get; set; }

        public bool Compare(Task task)
        {
            if (this.Title != task.Title)
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            if (this.Title == "" || this.Description == "")
            {
                return true;
            }
            return false;
        }
    }
}
