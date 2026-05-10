using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class CountryMapper
    {
        public static Country ToEntity(this CreateCountryRequestDto dto)
        {
            return new Country(dto.CountryName);
        }

        public static Country ToEntity(this UpdateCountryRequestDto dto)
        {
            return new Country(dto.CountryName);
        }

        public static CountryResponseDto ToResponseDto(this Country entity)
        {
            return new CountryResponseDto
            {
                CountryId = entity.CountryId,
                CountryName = entity.CountryName
            };
        }
    }
}
