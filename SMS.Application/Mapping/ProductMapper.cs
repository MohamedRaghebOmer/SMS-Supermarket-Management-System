using SMS.Contracts.Requests.Products;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class ProductMapper
    {
        public static Product ToEntity(this CreateProductRequestDto dto)
        {
            return new Product(
                categoryId: dto.CategoryId,
                productName: dto.ProductName,
                sku: dto.SKU,
                description: dto.Description,
                unitId: dto.UnitId,
                costPrice: dto.CostPrice,
                sellPrice: dto.SellPrice,
                discountPercent: dto.DiscountPercent,
                imageGuid: null,
                createdAt: DateTime.UtcNow);
        }

        public static Product ToEntity(this UpdateProductRequestDto dto, int productId)
        {
            return new Product(
                productId: productId,
                categoryId: dto.CategoryId,
                productName: dto.ProductName,
                sku: dto.SKU,
                description: dto.Description,
                unitId: dto.UnitId,
                costPrice: dto.CostPrice,
                sellPrice: dto.SellPrice,
                discountPercent: dto.DiscountPercent,
                imageGuid: null,
                isActive: dto.IsActive,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow);
        }

        public static ProductResponseDto ToDto(this Product entity)
        {
            return new ProductResponseDto
            {
                ProductId = entity.ProductId,
                CategoryId = entity.CategoryId,
                ProductName = entity.ProductName,
                SKU = entity.SKU,
                Description = entity.Description,
                UnitId = entity.UnitId,
                CostPrice = entity.CostPrice,
                SellPrice = entity.SellPrice,
                DiscountPercent = entity.DiscountPercent,
                ImageGuid = entity.ImageGuid,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
