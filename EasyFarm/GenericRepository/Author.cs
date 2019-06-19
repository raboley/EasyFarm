using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.GenericRepository
{
    [Table("Author")]
    public partial class Author : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string authorname { get; set; }
    }
}
