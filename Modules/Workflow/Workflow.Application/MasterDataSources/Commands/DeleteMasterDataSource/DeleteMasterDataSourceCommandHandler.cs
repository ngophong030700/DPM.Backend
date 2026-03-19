using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.DeleteMasterDataSource
{
    public class DeleteMasterDataSourceCommandHandler
        : IRequestHandler<DeleteMasterDataSourceCommand, bool>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteMasterDataSourceCommandHandler(
            IMasterDataSourceRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteMasterDataSourceCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.SoftDeleteAsync(request.Id, _currentUser.UserId);
        }
    }
}