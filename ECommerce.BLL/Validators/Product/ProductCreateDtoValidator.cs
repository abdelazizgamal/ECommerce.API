
using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.Title)
                .NotEmpty()
                .WithErrorCode("Err-1")
                .MinimumLength(3)
                .WithMessage("Title Minmum Len is 3")
                .WithErrorCode("Err-2")
                .MustAsync(CheckUniqueTitle)
                .WithMessage("Title already exists")
                .WithErrorCode("Err-3");


            RuleFor(e => e.Count)
                .GreaterThan(0)
                .WithMessage("Count must be greater than 0")
                .WithErrorCode("ERR-04");

            RuleFor(e => e.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0")
                .WithErrorCode("ERR-06");

            RuleFor(e => e.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0")
                .WithErrorCode("ERR-07");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithErrorCode("Err-8");
        }

        public async Task<bool> CheckUniqueTitle(String title, CancellationToken cancellationToken)
        {
            var flag = await _unitOfWork.Products.TitleCheck(title);
            return !flag;
        }
    }
}
