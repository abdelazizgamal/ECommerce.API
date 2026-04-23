using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductEditDtoValidator : AbstractValidator<ProductEditDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductEditDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.Title)
                .NotEmpty()
                .WithErrorCode("Err-1")
                .MinimumLength(3)
                .WithMessage("Title Minmum Len is 3")
                .WithErrorCode("Err-2")
                .MustAsync((dto, title, cancellationToken) => CheckUniqueTitle(dto.Id, title, cancellationToken))
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

            RuleFor(e => e.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0")
                .WithErrorCode("ERR-08");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithErrorCode("Err-8");
        }

        public async Task<bool> CheckUniqueTitle(int productId, string title, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.Title == title);
            if (products is null)
            {
                return true;
            }

            return !products.Any(p => p.Id != productId);
        }
    }
}
