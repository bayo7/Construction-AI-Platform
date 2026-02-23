using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.Abstract
{
    public interface ITestimonialService : IGenericService<Testimonial>
    {
        Task<List<Testimonial>> GetTestimonialsWithProjectAsync();
    }
}
