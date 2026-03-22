using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteDocumentCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var success = await _repository.DeleteDocumentAsync(request.Id, _currentUserService.UserId);
            if (!success)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu tài liệu cần xóa.");
            }
            return true;
        }
    }
}