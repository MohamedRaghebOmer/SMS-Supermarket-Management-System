using SMS.Contracts.Requests.People;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class PersonMapper
    {
        public static Person ToEntity(this CreatePersonRequestDto dto)
        {
            return new Person(
                nationalNo: dto.NationalNo,
                firstName: dto.FirstName,
                secondName: dto.SecondName,
                thirdName: dto.ThirdName,
                lastName: dto.LastName,
                dateOfBirth: dto.DateOfBirth,
                gender: dto.Gender,
                address: dto.Address,
                phone: dto.Phone,
                email: dto.Email,
                nationalityCountryId: dto.NationalityCountryId,
                imageGuid: null,
                createdAt: DateTime.UtcNow);
        }

        public static Person ToEntity(this UpdatePersonRequestDto dto, int personId)
        {
            return new Person(
                personId: personId,
                nationalNo: dto.NationalNo,
                firstName: dto.FirstName,
                secondName: dto.SecondName,
                thirdName: dto.ThirdName,
                lastName: dto.LastName,
                dateOfBirth: dto.DateOfBirth,
                gender: dto.Gender,
                address: dto.Address,
                phone: dto.Phone,
                email: dto.Email,
                nationalityCountryId: dto.NationalityCountryId,
                imageGuid: null,
                createdAt: DateTime.UtcNow);
        }

        public static PersonResponseDto ToDto(this Person entity, Stream? imageStream)
        {
            return new PersonResponseDto
            {
                PersonId = entity.PersonId,
                NationalNo = entity.NationalNo,
                FirstName = entity.FirstName,
                SecondName = entity.SecondName,
                ThirdName = entity.ThirdName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Address = entity.Address,
                Phone = entity.Phone,
                Email = entity.Email,
                NationalityCountryId = entity.NationalityCountryId,
                Image = imageStream
            };
        }
    }
}
