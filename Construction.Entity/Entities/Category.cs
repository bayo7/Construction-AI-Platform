using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Entity.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public List<Project> Projects { get; set; }
    }
}
