using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IImageManager
    {
        Task<GeneralResult<ImageUploadResultDto>> UploadAsync(ImageUploadDto imageUploadDto, string basePath, string? schema, string? host);
    }
}