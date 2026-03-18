using Identity.Application.Departments.Mappings;
using Identity.Application.Departments.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler
        : IRequestHandler<CreateDepartmentCommand, ViewDetailDepartmentDto?>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public CreateDepartmentCommandHandler(
            IDepartmentRepository repository,
            IDepartmentQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailDepartmentDto?> Handle(
            CreateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name))
                throw new DuplicateException($"Tên phòng ban '{dto.Name}' đã tồn tại.");

            // 🔹 2. Handle Tree logic (Level, PathCode)
            int level = 1;
            string pathCode = dto.Name;

            if (dto.ParentId.HasValue)
            {
                var parent = await _repository.GetByIdAsync(dto.ParentId.Value);
                if (parent == null)
                    throw new NotFoundException("Phòng ban cha không tồn tại.");

                level = parent.Level + 1;
                pathCode = $"{parent.PathCode} > {dto.Name}";
            }

            // 🔹 3. Mapping
            var entity = dto.ToCreateEntity(
                _currentUser.UserId,
                level,
                pathCode
            );

            // 🔹 4. Create
            var created = await _repository.CreateAsync(entity);

            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
