using Construction.DataAccess.Abstract;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Concrete
{
    public class EfTestimonialDal : GenericDal<Testimonial>, ITestimonialDal
    {
        private readonly ConstructionDbContext _context;

        public EfTestimonialDal(ConstructionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Testimonial>> GetTestimonialsWithProjectAsync()
        {
            return await _context.Testimonials.Include(t => t.Project).ToListAsync();
        }
    }
}
