using Microsoft.AspNetCore.Http;
using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.People;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Constants;
using SMS.Shared.Enums;
using SMS.Shared.Guards;
using System.Collections.Immutable;

namespace SMS.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IFileStorageService _fileStorageService;
        private readonly IDirectoryPathService _directoryPathService;
        private readonly IValidationHelper _validationHelper;
        private readonly IImageHelper _imageHelper;

        public PersonService(IPersonRepository repo,
            IFileStorageService fileStorageService,
            IDirectoryPathService directoryPathService,
            IValidationHelper validationHelper,
            IImageHelper imageHelper)
        {
            _repo = repo;
            _fileStorageService = fileStorageService;
            _directoryPathService = directoryPathService;
            _validationHelper = validationHelper;
            _imageHelper = imageHelper;
        }

        public async Task<int> AddAsync(CreatePersonRequestDto dto, IFormFile? image)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateDto(dto);
            EnsureIsImage(image, allowEmpty: true);

            Guid? imageGuid = image is null ? null : Guid.NewGuid();

            var entity = dto.ToEntity();
            entity.ImageGuid = imageGuid;

            var result = await _repo.AddAsync(entity);
            result.ThrowIfNotSuccess();

            if (imageGuid is not null)
            {
                await _fileStorageService.SaveFileAsync(image!, _directoryPathService.PeopleDirectory, (Guid)imageGuid);
            }

            return result.Data;
        }

        public async Task<PersonResponseDto> GetByIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.FindByIdAsync(personId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return await BuildDtoWithImageAsync(result.Data!);
        }

        public async Task<FileResponse> GetImageAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.GetImageAsync(personId);
            result.ThrowIfNotSuccess();

            if (!result.Data.HasValue)
            {
                throw new NoContentException("Person does not have an image.");
            }

            var imagePath = _imageHelper.ResolveImagePath(_directoryPathService.PeopleDirectory, result.Data.Value);
            return await _fileStorageService.LoadFileAsync(imagePath);
        }

        public async Task<PersonResponseDto> GetByNationalNoAsync(string nationalNo)
        {
            StringGuard.AgainstNullOrWhiteSpace(nationalNo, nameof(nationalNo));

            var result = await _repo.FindByNationalNoAsync(nationalNo);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return await BuildDtoWithImageAsync(result.Data!);
        }

        public async Task<PaginationResponse<PersonResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return await BuildPagedResponseAsync(result, paginationRequest);
        }

        public async Task<PaginationResponse<PersonResponseDto>> GetByGenderAsync(
            Gender gender, PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetByGenderAsync(gender, paginationRequest);
            result.ThrowIfNotSuccess();

            return await BuildPagedResponseAsync(result, paginationRequest);
        }

        public async Task<PersonResponseDto> GetByEmailAsync(string email)
        {
            _validationHelper.ValidateEmail(email, nameof(email), isRequired: true, minLength: 5, maxLength: 50);

            var result = await _repo.FindByEmailAsync(email);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return await BuildDtoWithImageAsync(result.Data!);
        }

        public async Task<PaginationResponse<PersonResponseDto>> GetByNationalityCountryIdAsync(
            int countryId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(countryId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetByNationalityCountryIdAsync(countryId, paginationRequest);
            result.ThrowIfNotSuccess();

            return await BuildPagedResponseAsync(result, paginationRequest);
        }

        public async Task<bool> ExistsByIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.ExistsByIdAsync(personId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByNationalNoAsync(string nationalNo)
        {
            StringGuard.AgainstNullOrWhiteSpace(nationalNo, nameof(nationalNo));

            var result = await _repo.ExistsByNationalNoAsync(nationalNo);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            _validationHelper.ValidateEmail(email, nameof(email));

            var result = await _repo.ExistsByEmailAsync(email);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> SetImageAsync(int personId, IFormFile newImage)
        {
            NumericGuard.AgainstInvalidId(personId);
            EnsureIsImage(newImage);

            var existing = await _repo.FindByIdAsync(personId);
            existing.ThrowIfNotSuccess();
            existing.ThrowNotFoundIfDataNull();

            Guid newImageGuid;
            if (existing.Data!.ImageGuid.HasValue) // If person already has an image, replace it
            {
                newImageGuid = await _fileStorageService.ReplaceFileAsync(
                    existing.Data!.ImageGuid.Value,
                    newImage,
                    _directoryPathService.PeopleDirectory);
            }
            else // If person does not have an image, save the new image and get its guid
            {
                newImageGuid = await _fileStorageService.SaveFileAsync(newImage, _directoryPathService.PeopleDirectory);
            }

            if (newImageGuid != Guid.Empty)
            {
                var result = await _repo.SetImageAsync(personId, newImageGuid);
                result.ThrowIfNotSuccess();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveImageAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var existing = await _repo.FindByIdAsync(personId);
            existing.ThrowIfNotSuccess();
            existing.ThrowNotFoundIfDataNull();

            if (!existing.Data!.ImageGuid.HasValue)
            {
                throw new NoContentException("Person does not have an image.");
            }

            var imagePath = _imageHelper.ResolveImagePath(
                _directoryPathService.PeopleDirectory,
                existing.Data.ImageGuid.Value);

            await _fileStorageService.DeleteFileAsync(imagePath);

            var result = await _repo.SetImageAsync(personId, null);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(int personId, UpdatePersonRequestDto dto,
            IFormFile? newImage)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(personId);
            ValidateDto(dto);
            EnsureIsImage(newImage, true);

            var existing = await _repo.FindByIdAsync(personId);
            existing.ThrowIfNotSuccess();
            existing.ThrowNotFoundIfDataNull();

            Guid? newImageGuid = null;
            if (newImage is not null)
            {
                if (!existing.Data!.ImageGuid.HasValue)
                {
                    throw new InvalidOperationException("Person does not have an image to update.");
                }

                newImageGuid = await _fileStorageService.ReplaceFileAsync(
                    existing.Data.ImageGuid.Value,
                    newImage,
                    _directoryPathService.PeopleDirectory);
            }

            var result = await _repo.UpdateAsync(dto.ToEntity(personId));
            result.ThrowIfNotSuccess();

            if (newImageGuid.HasValue)
            {
                var updateImageResult = await _repo.SetImageAsync(personId, newImageGuid.Value);
                updateImageResult.ThrowIfNotSuccess();
            }

            return result.Data;
        }

        public async Task<bool> DeleteAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);
            var existing = await _repo.FindByIdAsync(personId);
            existing.ThrowIfNotSuccess();
            existing.ThrowNotFoundIfDataNull();

            var result = await _repo.DeleteAsync(personId);
            result.ThrowIfNotSuccess();

            if (existing.Data!.ImageGuid.HasValue)
            {
                var imagePath = _imageHelper.ResolveImagePath(_directoryPathService.PeopleDirectory,
                    existing.Data.ImageGuid.Value);
                await _fileStorageService.DeleteFileAsync(imagePath);
            }

            return result.Data;
        }

        public async Task<bool> DeleteAsync(string nationalNo)
        {
            StringGuard.AgainstNullOrWhiteSpace(nationalNo, nameof(nationalNo));
            var existing = await _repo.FindByNationalNoAsync(nationalNo);
            existing.ThrowIfNotSuccess();
            existing.ThrowNotFoundIfDataNull();

            var result = await _repo.DeleteAsync(nationalNo);
            result.ThrowIfNotSuccess();

            if (existing.Data!.ImageGuid.HasValue)
            {
                var imagePath = _imageHelper.ResolveImagePath(_directoryPathService.PeopleDirectory,
                    existing.Data.ImageGuid.Value);
                await _fileStorageService.DeleteFileAsync(imagePath);
            }

            return result.Data;
        }

        private static void ValidateDto(CreatePersonRequestDto dto)
        {
            StringGuard.AgainstNullOrWhiteSpace(dto.NationalNo, nameof(dto.NationalNo));
            StringGuard.AgainstNullOrWhiteSpace(dto.FirstName, nameof(dto.FirstName));
            StringGuard.AgainstNullOrWhiteSpace(dto.SecondName, nameof(dto.SecondName));
            StringGuard.AgainstNullOrWhiteSpace(dto.LastName, nameof(dto.LastName));
            StringGuard.AgainstNullOrWhiteSpace(dto.Address, nameof(dto.Address));
            StringGuard.AgainstNullOrWhiteSpace(dto.Phone, nameof(dto.Phone));
            NumericGuard.AgainstInvalidId(dto.NationalityCountryId);
            if (dto.DateOfBirth.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dto.DateOfBirth));
            }
        }

        private static void ValidateDto(UpdatePersonRequestDto dto)
        {
            StringGuard.AgainstNullOrWhiteSpace(dto.NationalNo, nameof(dto.NationalNo));
            StringGuard.AgainstNullOrWhiteSpace(dto.FirstName, nameof(dto.FirstName));
            StringGuard.AgainstNullOrWhiteSpace(dto.SecondName, nameof(dto.SecondName));
            StringGuard.AgainstNullOrWhiteSpace(dto.LastName, nameof(dto.LastName));
            StringGuard.AgainstNullOrWhiteSpace(dto.Address, nameof(dto.Address));
            StringGuard.AgainstNullOrWhiteSpace(dto.Phone, nameof(dto.Phone));
            NumericGuard.AgainstInvalidId(dto.NationalityCountryId);
            if (dto.DateOfBirth.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dto.DateOfBirth));
            }
        }


        private async Task<PaginationResponse<PersonResponseDto>> BuildPagedResponseAsync(
            Common.Results.OperationResult<PaginationResponse<Person>> result,
            PaginationRequest paginationRequest)
        {
            var items = await Task.WhenAll(
                result.Data!.Items.Select(async p => await BuildDtoWithImageAsync(p)));

            return new PaginationResponse<PersonResponseDto>
            {
                Items = items.ToImmutableList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }

        private async Task<PersonResponseDto> BuildDtoWithImageAsync(Person person)
        {
            FileResponse? imageResponse = null;
            if (person.ImageGuid.HasValue)
            {
                var imagePath =
                    _imageHelper.ResolveImagePath(_directoryPathService.PeopleDirectory, person.ImageGuid.Value);
                imageResponse = await _fileStorageService.LoadFileAsync(imagePath);
            }

            return person.ToDto(imageResponse);
        }

        private void EnsureIsImage(IFormFile? file, bool allowEmpty = false)
        {
            // Ensure file is provided if not allowed to be empty
            if (file is null || file.Length == 0)
            {
                if (!allowEmpty)
                {
                    throw new ArgumentException("Invalid file.", nameof(file));
                }

                return;
            }

            // Validate file size
            if (file.Length > Constants.MaxImageSizeInBytes)
            {
                throw new ArgumentException(
                    $"File size exceeds the allowed limit of {Constants.MaxImageSizeInBytes / (1024 * 1024)} MB.",
                    nameof(file));
            }

            // Validate content type
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The provided file is not an image.", nameof(file));
            }

            HashSet<string> allowedExtensions =
            [
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".webp"
            ];

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension) ||
                !allowedExtensions.Contains(extension.ToLowerInvariant()))
            {
                throw new ArgumentException("Unsupported image extension.", nameof(file));
            }
        }
    }
}