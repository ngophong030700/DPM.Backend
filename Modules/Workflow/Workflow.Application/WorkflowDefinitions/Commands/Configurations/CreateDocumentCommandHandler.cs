using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateDocumentCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var document = WorkflowStepDefineDocument.Create(
                stepId: dto.StepId,
                docTypeName: dto.DocTypeName,
                isRequired: dto.IsRequired,
                checkDigitalSignature: dto.CheckDigitalSignature,
                createdBy: userId,
                sortOrder: dto.SortOrder
            );

            await _repository.SaveDocumentAsync(document);
            return true;
        }
    }
}
