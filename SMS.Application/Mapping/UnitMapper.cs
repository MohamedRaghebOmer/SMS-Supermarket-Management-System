using SMS.Contracts.Requests.Units;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class UnitMapper
    {
        public static Unit ToEntity(this CreateUnitRequestDto dto)
        {
            return new Unit(dto.UnitName, dto.Symbol, dto.IsDecimal);
        }

        public static Unit ToEntity(this UpdateUnitRequestDto dto, int unitId)
        {
            return new Unit(unitId, dto.UnitName, dto.Symbol, dto.IsDecimal);
        }

        public static UnitResponseDto ToDto(this Unit entity)
        {
            return new UnitResponseDto
            {
                UnitId = entity.UnitId,
                UnitName = entity.UnitName,
                Symbol = entity.Symbol,
                IsDecimal = entity.IsDecimal
            };
        }
    }
}
