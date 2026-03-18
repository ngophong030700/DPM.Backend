using Identity.Application.Groups.Mappings;
using Identity.Application.Groups.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandHandler
        : IRequestHandler<UpdateGroupCommand, ViewDetailGroupDto?>
    {
        private readonly IGroupRepository _repository;
        private readonly IGroupQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdateGroupCommandHandler(
            IGroupRepository repository,
            IGroupQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailGroupDto?> Handle(
            UpdateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Nhóm không tồn tại.");

            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name, request.Id))
                throw new DuplicateException($"Tên nhóm '{dto.Name}' đã tồn tại.");

            // 🔹 2. Update
            entity.Update(
                name: dto.Name,
                description: dto.Description,
                modifiedBy: _currentUser.UserId
            );

            await _repository.UpdateAsync(entity);

            // 🔹 3. Sync users
            if (dto.UserIds != null)
            {
                // Clear and add again (Simple way)
                await _repository.ClearUsersInGroupAsync(entity.Id);
                foreach (var userId in dto.UserIds)
                {
                    await _repository.AddUserToGroupAsync(userId, entity.Id, _currentUser.UserId);
                }
            }

            // Refresh to get full detail with groups and Names
            return await _queryService.GetByIdAsync(entity.Id);
        }
    }
}
