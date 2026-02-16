using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Entity.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ProjectStatus { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice { get; set; }
        public string CoverImageUrl { get; set; }

        public string? DetailsEmbedding { get; set; }
    }
}
