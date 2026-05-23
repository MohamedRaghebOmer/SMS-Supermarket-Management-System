using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.ApplicationLogs;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Guards;
using LogLevel = SMS.Shared.Enums.LogLevel;

namespace SMS.Application.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public ApplicationLogService(IApplicationLogRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }


        public async Task<int> AddAsync(ApplicationLogRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(dto.AuditLogId);
            StringGuard.AgainstNullOrEmpty(dto.Message, nameof(dto.Message));

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<ApplicationLogResponseDto> GetAsync(int id)
        {
            NumericGuard.AgainstInvalidId(id);

            var result = await _repo.FindAsync(id);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<ApplicationLogResponseDto> GetByAuditLogIdAsync(int auditLogId)
        {
            NumericGuard.AgainstInvalidId(auditLogId);

            var result = await _repo.FindByAuditLogIdAsync(auditLogId);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<PaginationResponse<ApplicationLogResponseDto>> GetPagedAsync(
            PaginationRequest pagination)
        {
            _validationHelper.ValidatePagination(pagination);

            var result = await _repo.GetPagedAsync(pagination);
            result.ThrowIfNotSuccess();

            return new PaginationResponse<ApplicationLogResponseDto>
            {
                Items = result.Data.Items.Select(log => log.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }

        public async Task<PaginationResponse<ApplicationLogResponseDto>> GetPagedByLogLevelAsync(
            LogLevel logLevel, PaginationRequest pagination)
        {
            _validationHelper.ValidatePagination(pagination);

            var result = await _repo.GetPagedByLogLevelAsync(logLevel, pagination);
            result.ThrowIfNotSuccess();

            return new PaginationResponse<ApplicationLogResponseDto>
            {
                Items = result.Data.Items.Select(log => log.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }

        public async Task<PaginationResponse<ApplicationLogResponseDto>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, PaginationRequest pagination)
        {
            _validationHelper.ValidatePagination(pagination);
            DateGuard.AgainstFutureDate(startDate, nameof(startDate));
            DateGuard.AgainstFutureDate(endDate, nameof(endDate));

            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be greater than or equal to start date.", nameof(endDate));
            }

            var result = await _repo.GetPagedByDateRangeAsync(startDate, endDate, pagination);
            result.ThrowIfNotSuccess();

            return new PaginationResponse<ApplicationLogResponseDto>
            {
                Items = result.Data.Items.Select(log => log.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }

    }
}
