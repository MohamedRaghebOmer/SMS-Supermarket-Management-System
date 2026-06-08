using SMS.Contracts.Requests.ReturnItems;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class ReturnItemMapper
    {
        public static ReturnItem ToEntity(this CreateReturnItemRequestDto dto)
        {
            return new ReturnItem
            {
                ReturnId = dto.ReturnId,
                SaleItemId = dto.SaleItemId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                LineTotal = dto.LineTotal,
            };
        }

        public static ReturnItemResponseDto ToDto(this ReturnItem entity)
        {
            return new ReturnItemResponseDto
            {
                ReturnItemId = entity.ReturnItemId,
                ReturnId = entity.ReturnId,
                SaleItemId = entity.SaleItemId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                LineTotal = entity.LineTotal
            };
        }
    }
}
