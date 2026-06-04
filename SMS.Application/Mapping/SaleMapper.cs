using SMS.Contracts.Requests.Sales;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class SaleMapper
    {
        public static Sale ToEntity(this CreateSaleRequestDto dto, int cashierId)
        {
            return new Sale(
                saleDate: DateTime.UtcNow,
                customerId: dto.CustomerId,
                cashierId: cashierId,
                paymentMethod: dto.PaymentMethod,
                subTotal: dto.SubTotal,
                discountAmount: dto.DiscountAmount,
                netTotal: dto.NetTotal,
                paidAmount: dto.PaidAmount,
                changeAmount: dto.ChangeAmount,
                isCredit: dto.IsCredit,
                notes: dto.Notes);
        }

        public static SaleResponseDto ToDto(this Sale entity)
        {
            return new SaleResponseDto
            {
                SaleId = entity.SaleId,
                SaleDate = entity.SaleDate,
                CustomerId = entity.CustomerId,
                CashierId = entity.CashierId,
                PaymentMethod = entity.PaymentMethod,
                SubTotal = entity.SubTotal,
                DiscountAmount = entity.DiscountAmount,
                NetTotal = entity.NetTotal,
                PaidAmount = entity.PaidAmount,
                ChangeAmount = entity.ChangeAmount,
                IsCredit = entity.IsCredit,
                Notes = entity.Notes,
            };
        }
    }
}