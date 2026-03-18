using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;

namespace Identity.Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler
        : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly IGroupRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteGroupCommandHandler(
            IGroupRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteGroupCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Nhóm không tồn tại.");

            // 🔹 Soft delete
            return await _repository.SoftDeleteAsync(request.Id, _currentUser.UserId);
        }
    }
}
