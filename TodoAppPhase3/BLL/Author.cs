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
        [Key]
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
