using Identity.Application.Positions.Mappings;
using Identity.Application.Positions.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommandHandler
        : IRequestHandler<UpdatePositionCommand, ViewDetailPositionDto?>
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdatePositionCommandHandler(
            IPositionRepository repository,
            IPositionQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailPositionDto?> Handle(
            UpdatePositionCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Chức vụ không tồn tại.");

            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name, request.Id))
                throw new DuplicateException($"Tên chức vụ '{dto.Name}' đã tồn tại.");

            // 🔹 2. Update
            entity.Update(
                name: dto.Name,
                description: dto.Description,
                modifiedBy: _currentUser.UserId
            );

            var updated = await _repository.UpdateAsync(entity);

            return await _queryService.GetByIdAsync(updated.Id);
        }
    }
}
