using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.AuditLogs;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using SMS.Shared.Guards;
using System.Net;

namespace SMS.Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;

        public AuditLogService(IAuditLogRepository repo)
        {
            _repo = repo;
        }


        public async Task<long> AddAsync(AuditLogRequestDto requestDto)
        {
            ArgumentNullException.ThrowIfNull(requestDto);
            NumericGuard.AgainstInvalidId(requestDto.UserId);

            if (requestDto.CorrelationId == Guid.Empty)
            {
                throw new ArgumentException("CorrelationId cannot be empty.", nameof(requestDto.CorrelationId));
            }

            StringGuard.AgainstNullOrEmptyString(requestDto.Endpoint, nameof(requestDto.Endpoint));
            NumericGuard.AgainstNegativeNumber(requestDto.Duration, nameof(requestDto.Duration));
            StringGuard.AgainstNullOrEmptyString(requestDto.IpAddress, nameof(requestDto.IpAddress));

            var result = await _repo.AddAuditLogAsync(requestDto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<AuditLogResponseDto> GetAsync(long auditLogId)
        {
            NumericGuard.AgainstInvalidId(auditLogId);

            var result = await _repo.FindAsync((int)auditLogId);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<AuditLogResponseDto> GetByCorrelationIdAsync(Guid correlationId)
        {
            if (correlationId == Guid.Empty)
            {
                throw new ArgumentException("CorrelationId cannot be empty.", nameof(correlationId));
            }

            var result = await _repo.FindByCorrelationIdAsync(correlationId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            OperationResult<PaginationResponse<AuditLog>> result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedByUserIdAsync(
            int userId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(userId);
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByUserIdAsync(userId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedByActionTypeAsync(
            AuditActionType actionType, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByActionTypeAsync(actionType, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedByEndpointAsync(
            string endpointUrl, PaginationRequest paginationRequest)
        {
            StringGuard.AgainstNullOrEmptyString(endpointUrl, nameof(endpointUrl));
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByEndpointUrlAsync(endpointUrl, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedByHttpStatusCodeAsync(
            HttpStatusCode httpStatusCode, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByHttpStatusCodeAsync(httpStatusCode, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedByIpAddressAsync(
            string ipAddress, PaginationRequest paginationRequest)
        {
            StringGuard.AgainstNullOrEmptyString(ipAddress, nameof(ipAddress));
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByIpAddressAsync(ipAddress, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedCreatedBeforeAsync(
            DateTime dateTime, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);
            DateGuard.AgainstFutureDate(dateTime, nameof(dateTime));

            var result = await _repo.GetPagedByCreatedBeforeAsync(dateTime, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }

        public async Task<PaginationResponse<AuditLogResponseDto>> GetPagedCreatedAfterAsync(
            DateTime dateTime, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);
            DateGuard.AgainstFutureDate(dateTime, nameof(dateTime));

            var result = await _repo.GetPagedByCreatedAfterAsync(dateTime, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result);
        }



        private static PaginationResponse<AuditLogResponseDto> BuildPagedResponse(
            OperationResult<PaginationResponse<AuditLog>> result)
        {
            return new PaginationResponse<AuditLogResponseDto>
            {
                Items = result.Data.Items.Select(p => p.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }

        private static void ValidatePagination(PaginationRequest paginationRequest)
        {
            ArgumentNullException.ThrowIfNull(paginationRequest);
            NumericGuard.AgainstInvalidId(paginationRequest.Page);
            NumericGuard.AgainstInvalidId(paginationRequest.PageSize);
        }
    }
}
