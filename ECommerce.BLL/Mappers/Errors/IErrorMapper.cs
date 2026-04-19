using ECommerce.Common;
using FluentValidation.Results;

namespace ECommerce.BLL
{
    public interface IErrorMapper
    {
        Dictionary<string, List<Errors>> MapError(ValidationResult validationResult);
    }
}