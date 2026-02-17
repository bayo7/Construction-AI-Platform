using Construction.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.ValidatonRules
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Lütfen kategori adını boş geçmeyiniz.") 
            .MinimumLength(3).WithMessage("Kategori adı en az 3 karakterden oluşmalıdır.")
            .MaximumLength(50).WithMessage("Kategori adı 50 karakterden uzun olamaz.")
            .WithName("Kategori Adı"); 
        }
    }
}
