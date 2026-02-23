using Construction.Business.Abstract;
using Construction.DataAccess.Abstract;
using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.Concrete
{
    public class TestimonialManager : ITestimonialService
    {
        private readonly ITestimonialDal _testimonialDal;

        public TestimonialManager(ITestimonialDal testimonialDal)
        {
            _testimonialDal = testimonialDal;
        }

        public Task<List<Testimonial>> GetTestimonialsWithProjectAsync()
        {
            return _testimonialDal.GetTestimonialsWithProjectAsync();
        }

        public async Task TDeleteAsync(Testimonial entity)
        {
            await _testimonialDal.DeleteAsync(entity);

        }

        public async Task<Testimonial> TGetByIdAsync(int id)
        {
            return await _testimonialDal.GetByIdAsync(id);
        }

        public async Task<List<Testimonial>> TGetListAsync()
        {
            return await _testimonialDal.GetListAsync();
        }

        public async Task TInsertAsync(Testimonial entity)
        {
            await _testimonialDal.InsertAsync(entity);

        }

        public async Task TUpdateAsync(Testimonial entity)
        {
            await _testimonialDal.UpdateAsync(entity);
        }
    }
}
