using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateDocumentCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _repository.GetDocumentByIdAsync(request.Id);
            if (doc == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu tài liệu.");
            }

            var dto = request.Data;
            doc.Update(
                docTypeName: dto.DocTypeName,
                isRequired: dto.IsRequired,
                checkDigitalSignature: dto.CheckDigitalSignature,
                sortOrder: dto.SortOrder,
                modifiedBy: _currentUserService.UserId
            );

            await _repository.SaveDocumentAsync(doc);
            return true;
        }
    }
}