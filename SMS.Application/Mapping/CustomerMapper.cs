using SMS.Contracts.Requests.Customers;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class CustomerMapper
    {
        public static Customer ToEntity(this CreateCustomerRequestDto dto)
        {
            return new Customer(
                personId: dto.PersonId,
                joinDate: DateTime.UtcNow,
                isActive: true,
                paymentDay: dto.PaymentDay,
                currentBalance: 0m,
                lastPaymentDate: null,
                nextDueDate: null,
                notes: dto.Notes);
        }

        public static Customer ToEntity(this UpdateCustomerRequestDto dto, int customerId)
        {
            return new Customer(
                customerId: customerId,
                personId: dto.PersonId,
                joinDate: DateTime.UtcNow,
                isActive: true,
                paymentDay: dto.PaymentDay,
                currentBalance: 0m,
                lastPaymentDate: null,
                nextDueDate: null,
                notes: dto.Notes);
        }

        public static CustomerResponseDto ToDto(this Customer entity)
        {
            return new CustomerResponseDto
            {
                CustomerId = entity.CustomerId,
                PersonId = entity.PersonId,
                JoinDate = entity.JoinDate,
                IsActive = entity.IsActive,
                PaymentDay = entity.PaymentDay,
                CurrentBalance = entity.CurrentBalance,
                LastPaymentDate = entity.LastPaymentDate,
                NextDueDate = entity.NextDueDate,
                Notes = entity.Notes
            };
        }
    }
}
