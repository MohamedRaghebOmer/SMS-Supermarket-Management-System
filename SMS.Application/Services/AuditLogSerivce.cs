using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Common;
using SMS.Contracts.Requests.AuditLogs;
using SMS.Contracts.Responses;
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


        public async Task<int> AddAuditLogAsync(AuditLogRequestDto requestDto)
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

            var result = await _repo.GetAsync((int)auditLogId);
            result.ThrowIfNotSuccess();

            if (result.Data is null)
            {
                throw new NotFoundException($"Audit log with Id {auditLogId} was not found.");
            }

            return result.Data.ToDto();
        }

        public async Task<AuditLogResponseDto> GetByCorrelationIdAsync(Guid correlationId)
        {
            if (correlationId == Guid.Empty)
            {
                throw new ArgumentException("CorrelationId cannot be empty.", nameof(correlationId));
            }

            var result = await _repo.GetByCorrelationIdAsync(correlationId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByUserIdAsync(int userId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(userId);
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByUserIdPagedAsync(userId, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByActionTypeAsync(AuditActionType actionType, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByActionTypePagedAsync(actionType, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByEndpointAsync(string endpointUrl, PaginationRequest paginationRequest)
        {
            StringGuard.AgainstNullOrEmptyString(endpointUrl, nameof(endpointUrl));
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByEndpointUrlPagedAsync(endpointUrl, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByHttpStatusCodeAsync(HttpStatusCode httpStatusCode, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByHttpStatusCodePagedAsync(httpStatusCode, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByIpAddressAsync(string ipAddress, PaginationRequest paginationRequest)
        {
            StringGuard.AgainstNullOrEmptyString(ipAddress, nameof(ipAddress));
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByIpAddressPagedAsync(ipAddress, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedCreatedBeforeAsync(DateTime dateTime, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);
            DateGuard.AgainstFutureDate(dateTime, nameof(dateTime));

            var result = await _repo.GetCreatedBeforePagedAsync(dateTime, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }

        public async Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedCreatedAfterAsync(DateTime dateTime, PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);
            DateGuard.AgainstFutureDate(dateTime, nameof(dateTime));

            var result = await _repo.GetCreatedAfterPagedAsync(dateTime, paginationRequest.Page, paginationRequest.PageSize);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(paginationRequest, result.Data);
        }



        private static IReadOnlyList<PaginationResponse<AuditLogResponseDto>> BuildPagedResponse(PaginationRequest paginationRequest, IReadOnlyList<SMS.Domain.Entities.AuditLog> auditLogs)
        {
            IReadOnlyList<AuditLogResponseDto> dtoReadOnlyList = auditLogs
                .Select(p => p.ToDto())
                .ToList();

            return new List<PaginationResponse<AuditLogResponseDto>>
            {
                new PaginationResponse<AuditLogResponseDto>
                {
                    Items = dtoReadOnlyList,
                    TotalCount = auditLogs.Count,
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize
                }
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
