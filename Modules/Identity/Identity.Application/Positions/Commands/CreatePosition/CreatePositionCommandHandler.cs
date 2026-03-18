using Identity.Application.Positions.Mappings;
using Identity.Application.Positions.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Positions.Commands.CreatePosition
{
    public class CreatePositionCommandHandler
        : IRequestHandler<CreatePositionCommand, ViewDetailPositionDto?>
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public CreatePositionCommandHandler(
            IPositionRepository repository,
            IPositionQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailPositionDto?> Handle(
            CreatePositionCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 1. Check duplicate name
            if (await _repository.IsNameExistsAsync(dto.Name))
                throw new DuplicateException($"Tên chức vụ '{dto.Name}' đã tồn tại.");

            // 🔹 2. Mapping
            var entity = dto.ToCreateEntity(_currentUser.UserId);

            // 🔹 3. Create
            var created = await _repository.CreateAsync(entity);

            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
