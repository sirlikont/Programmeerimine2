using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Products
{
    public class SaveProductCommandValidator : AbstractValidator<SaveProductCommand>
    {
        public SaveProductCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters");

            RuleFor(x => x.PhotoUrl)
                .MaximumLength(255).WithMessage("Photo URL cannot exceed 255 characters");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be >= 0");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId is required and must be > 0");
        }
    }
}
