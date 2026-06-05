using SMS.Contracts.Requests.SaleItems;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class SaleItemMapper
    {
        public static SaleItem ToEntity(this CreateSaleItemRequestDto dto)
        {
            return new SaleItem(
                saleId: dto.SaleId,
                productId: dto.ProductId,
                quantity: dto.Quantity,
                unitCostPriceAtSale: 0m,
                unitSellPriceAtSale: dto.UnitSellPriceAtSale,
                discountAmount: dto.DiscountAmount,
                lineTotal: dto.LineTotal);
        }

        public static SaleItem ToEntity(this UpdateSaleItemRequestDto dto, int saleItemId)
        {
            return new SaleItem(
                saleItemId: saleItemId,
                saleId: dto.SaleId,
                productId: dto.ProductId,
                quantity: dto.Quantity,
                unitCostPriceAtSale: 0m,
                unitSellPriceAtSale: dto.UnitSellPriceAtSale,
                discountAmount: dto.DiscountAmount,
                lineTotal: dto.LineTotal);
        }

        public static SaleItemResponseDto ToDto(this SaleItem entity)
        {
            return new SaleItemResponseDto
            {
                SaleItemId = entity.SaleItemId,
                SaleId = entity.SaleId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                UnitCostPriceAtSale = entity.UnitCostPriceAtSale,
                UnitSellPriceAtSale = entity.UnitSellPriceAtSale,
                DiscountAmount = entity.DiscountAmount,
                LineTotal = entity.LineTotal
            };
        }
    }
}
