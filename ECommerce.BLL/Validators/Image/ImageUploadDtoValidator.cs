using FluentValidation;

namespace ECommerce.BLL
{
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {
        private static readonly string[] AllowedExtentions = { ".png", ".jpg", ".jpeg" };
        public ImageUploadDtoValidator()
        {
            RuleFor(i => i.File)
                .NotNull()
                .WithMessage("File is required")
                .WithErrorCode("ERR-01")
                .WithName("File");

            When(i => i.File != null, () =>
            {
                RuleFor(i => i.File.Length)
                    .GreaterThan(0)
                    .WithMessage("File must be not empty")
                    .WithErrorCode("ERR-01")
                    .WithName("FileSize")

                    .LessThanOrEqualTo(5_000_000)
                    .WithMessage("File must be less than 5MB")
                    .WithErrorCode("ERR-01")
                    .WithName("FileSize");

                RuleFor(i => Path.GetExtension(i.File.FileName).ToLower())
                    .Must(ext => AllowedExtentions.Contains(ext))
                    .WithMessage("Unsupported file extention")
                    .WithErrorCode("ERR-01")
                    .WithName("FileExtention");
            });
        }
    }
}
