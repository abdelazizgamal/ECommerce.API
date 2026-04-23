using FluentValidation;

namespace ECommerce.BLL
{
    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDTO>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than 0")
                .WithErrorCode("ERR-01");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0")
                .WithErrorCode("ERR-02");
        }
    }
}
