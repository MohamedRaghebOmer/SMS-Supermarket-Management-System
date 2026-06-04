using SMS.Contracts.Requests.Categories;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class CategoryMapper
    {
        public static Category ToEntity(this CreateCategoryRequestDto dto)
        {
            return new Category(dto.CategoryName, dto.CategoryDescription, true);
        }

        public static Category ToEntity(this UpdateCategoryRequestDto dto, int categoryId)
        {
            return new Category(categoryId, dto.CategoryName, dto.CategoryDescription, dto.IsActive);
        }

        public static CategoryResponseDto ToDto(this Category entity)
        {
            return new CategoryResponseDto
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                CategoryDescription = entity.CategoryDescription,
                IsActive = entity.IsActive
            };
        }
    }
}
