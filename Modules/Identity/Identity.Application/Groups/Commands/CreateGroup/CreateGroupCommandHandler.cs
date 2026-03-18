using Identity.Application.Groups.Mappings;
using Identity.Application.Groups.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler
        : IRequestHandler<CreateGroupCommand, ViewDetailGroupDto?>
    {
        private readonly IGroupRepository _repository;
        private readonly IGroupQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public CreateGroupCommandHandler(
            IGroupRepository repository,
            IGroupQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailGroupDto?> Handle(
            CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name))
                throw new DuplicateException($"Tên nhóm '{dto.Name}' đã tồn tại.");

            // 🔹 2. Mapping
            var entity = dto.ToCreateEntity(_currentUser.UserId);

            // 🔹 3. Create
            var created = await _repository.CreateAsync(entity);

            // 🔹 4. Add users if any
            if (dto.UserIds != null && dto.UserIds.Any())
            {
                foreach (var userId in dto.UserIds)
                {
                    await _repository.AddUserToGroupAsync(userId, created.Id, _currentUser.UserId);
                }
            }

            // Refresh to get full detail with groups and Names
            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
