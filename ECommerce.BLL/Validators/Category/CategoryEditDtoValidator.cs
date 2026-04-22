using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryEditDtoValidator : AbstractValidator<CategoryEditDTO>
    {
        public CategoryEditDtoValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0")
                .WithErrorCode("ERR-03");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithErrorCode("Err-1")
                .MinimumLength(3)
                .WithMessage("Name Minmum Len is 3")
                .WithErrorCode("Err-2");
        }
    }
}
