using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Abstract
{
    public interface ITestimonialDal : IGenericDal<Testimonial>
    {
        Task<List<Testimonial>> GetTestimonialsWithProjectAsync();
    }
}
