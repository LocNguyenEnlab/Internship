using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppPhase3.BLL
{
    [Table("Author")]
    public class Author
    {
        public Author()
        {
            this.Tasks = new HashSet<Task>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
