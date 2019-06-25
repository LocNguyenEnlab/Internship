using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppPhase3.BLL
{
    [Table("Task")]
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeCreate { get; set; }
        public int TypeList { get; set; } //0: todo, 1: doing, 2: done
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public virtual Author Author { get; set; }
    }
}
