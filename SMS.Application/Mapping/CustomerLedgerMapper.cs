using SMS.Contracts.Requests.CustomerLedgers;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;


namespace SMS.Application.Mapping
{
    internal static class CustomerLedgerMapper
    {
        public static CustomerLedger ToEntity(this CreateCustomerLedgerRequestDto dto, int userId)
        {
            return new CustomerLedger(
                customerId: dto.CustomerId,
                entryDate: DateTime.UtcNow,
                entryType: dto.EntryType,
                referenceType: dto.ReferenceType,
                referenceId: dto.ReferenceId,
                debitAmount: dto.DebitAmount,
                creditAmount: dto.CreditAmount,
                balanceBefore: 0,
                balanceAfter: 0,
                createdBy: userId,
                notes: dto.Notes?.Trim());
        }

        public static CustomerLedgerResponseDto ToDto(this CustomerLedger entity)
        {
            return new CustomerLedgerResponseDto
            {
                LedgerId = entity.LedgerId,
                CustomerId = entity.CustomerId,
                EntryDate = entity.EntryDate,
                EntryType = entity.EntryType,
                ReferenceType = entity.ReferenceType,
                ReferenceId = entity.ReferenceId,
                DebitAmount = entity.DebitAmount,
                CreditAmount = entity.CreditAmount,
                BalanceBefore = entity.BalanceBefore,
                BalanceAfter = entity.BalanceAfter,
                CreatedBy = entity.CreatedBy,
                Notes = entity.Notes
            };
        }
    }
}
