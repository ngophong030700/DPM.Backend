using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowLayoutCommandHandler : IRequestHandler<SetupWorkflowLayoutCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public SetupWorkflowLayoutCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SetupWorkflowLayoutCommand request, CancellationToken cancellationToken)
        {
            var version = await _repository.GetVersionByIdAsync(request.VersionId);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản quy trình.");
            }

            var userId = _currentUserService.UserId;

            var layout = WorkflowLayout.Create(
                versionId: request.VersionId,
                rowsJson: request.Data.RowsJson,
                attachmentSettingsJson: request.Data.AttachmentSettingsJson,
                createdBy: userId
            );

            await _repository.SaveLayoutAsync(layout);

            return true;
        }
    }
}