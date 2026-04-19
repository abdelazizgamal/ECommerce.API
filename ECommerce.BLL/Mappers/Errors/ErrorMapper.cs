using ECommerce.Common;
using FluentValidation.Results;

namespace  ECommerce.BLL
{
    public class ErrorMapper : IErrorMapper
    {
        public Dictionary<string, List<Errors>> MapError(ValidationResult validationResult)
        {
            return validationResult.Errors
                    .GroupBy(r => r.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => new Errors
                        {
                            Code = e.ErrorCode,
                            Message = e.ErrorMessage,
                        }).ToList()
                    );
        }
    }
}
