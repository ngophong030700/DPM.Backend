using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;

namespace Identity.Application.Positions.Commands.DeletePosition
{
    public class DeletePositionCommandHandler
        : IRequestHandler<DeletePositionCommand, bool>
    {
        private readonly IPositionRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeletePositionCommandHandler(
            IPositionRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeletePositionCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Chức vụ không tồn tại.");

            // 🔹 Soft delete
            return await _repository.SoftDeleteAsync(request.Id, _currentUser.UserId);
        }
    }
}
