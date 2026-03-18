using Identity.Application.Departments.Mappings;
using Identity.Application.Departments.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler
        : IRequestHandler<UpdateDepartmentCommand, ViewDetailDepartmentDto?>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdateDepartmentCommandHandler(
            IDepartmentRepository repository,
            IDepartmentQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailDepartmentDto?> Handle(
            UpdateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Phòng ban không tồn tại.");

            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name, request.Id))
                throw new DuplicateException($"Tên phòng ban '{dto.Name}' đã tồn tại.");

            // 🔹 2. Tree logic
            int level = entity.Level;
            string pathCode = entity.PathCode;

            if (dto.ParentId.HasValue && dto.ParentId != entity.ParentId)
            {
                var parent = await _repository.GetByIdAsync(dto.ParentId.Value);
                if (parent == null)
                    throw new NotFoundException("Phòng ban cha không tồn tại.");
                
                level = parent.Level + 1;
                pathCode = $"{parent.PathCode} > {dto.Name}";
            }
            else if (dto.Name != entity.Name)
            {
                // Update PathCode if name changed but parent same
                if (entity.ParentId.HasValue)
                {
                    var parent = await _repository.GetByIdAsync(entity.ParentId.Value);
                    pathCode = $"{parent!.PathCode} > {dto.Name}";
                }
                else
                {
                    pathCode = dto.Name;
                }
            }

            // 🔹 3. Update
            entity.Update(
                name: dto.Name,
                description: dto.Description,
                modifiedBy: _currentUser.UserId,
                index: dto.Index,
                level: level,
                pathCode: pathCode,
                parentId: dto.ParentId
            );

            var updated = await _repository.UpdateAsync(entity);

            return await _queryService.GetByIdAsync(updated.Id);
        }
    }
}
