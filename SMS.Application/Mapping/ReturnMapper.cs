using SMS.Contracts.Requests.Returns;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class ReturnMapper
    {
        public static Return ToEntity(this CreateReturnRequestDto dto, int createdBy)
        {
            return new Return(
                saleId: dto.SaleId,
                customerId: null,
                returnReason: dto.ReturnReason,
                returnTotal: dto.ReturnTotal,
                createdBy: createdBy);
        }

        public static ReturnResponseDto ToDto(this Return entity)
        {
            return new ReturnResponseDto
            {
                ReturnId = entity.ReturnId,
                SaleId = entity.SaleId,
                CustomerId = entity.CustomerId,
                ReturnReason = entity.ReturnReason,
                ReturnTotal = entity.ReturnTotal,
                CreatedBy = entity.CreatedBy,
                ReturnDate = entity.ReturnDate
            };
        }
    }
}
