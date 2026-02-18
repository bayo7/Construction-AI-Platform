using Construction.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.ValidatonRules
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Lütfen proje başlığını boş geçmeyiniz.")
                .MinimumLength(5).WithMessage("Proje başlığı en az 5 karakterden oluşmalıdır.")
                .MaximumLength(100).WithMessage("Proje başlığı 100 karakterden uzun olamaz.")
                .WithName("Proje Başlığı");
           RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Lütfen proje açıklamasını boş geçmeyiniz.")
                .MinimumLength(10).WithMessage("Proje açıklaması en az 10 karakterden oluşmalıdır.")
                .WithName("Proje Açıklaması");
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Lütfen proje konumunu boş geçmeyiniz.")
                .MaximumLength(200).WithMessage("Proje konumu 200 karakterden uzun olamaz.")
                .WithName("Proje Konumu");
            RuleFor(x => x.ProjectStatus)
                .NotEmpty().WithMessage("Lütfen proje durumunu boş geçmeyiniz.")
                .MaximumLength(50).WithMessage("Proje durumu 50 karakterden uzun olamaz.")
                .WithName("Proje Durumu");
            RuleFor(x => x.MinPrice)
                .GreaterThan(0).WithMessage("Lütfen geçerli bir minimum fiyat giriniz.")
                .WithName("Minimum Fiyat");


        }
    }
}
