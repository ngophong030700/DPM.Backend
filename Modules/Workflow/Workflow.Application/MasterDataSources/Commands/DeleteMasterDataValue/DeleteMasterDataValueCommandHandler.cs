using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.DeleteMasterDataValue
{
    public class DeleteMasterDataValueCommandHandler
        : IRequestHandler<DeleteMasterDataValueCommand, bool>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteMasterDataValueCommandHandler(
            IMasterDataSourceRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteMasterDataValueCommand request,
            CancellationToken cancellationToken)
        {
            var success = await _repository.SoftDeleteValueAsync(request.ValueId, _currentUser.UserId);
            if (success)
            {
                await _repository.SaveChangesAsync();
            }
            return success;
        }
    }
}