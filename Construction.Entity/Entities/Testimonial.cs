using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Entity.Entities
{
    public class Testimonial : BaseEntity
    {
        public string NameSurname { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

    }
}
