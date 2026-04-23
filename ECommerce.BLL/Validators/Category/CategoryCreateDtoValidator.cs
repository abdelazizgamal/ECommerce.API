using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithErrorCode("Err-1")
                .MinimumLength(3)
                .WithMessage("Name Minmum Len is 3")
                .WithErrorCode("Err-2");
        }
    }
}
