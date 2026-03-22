using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateReportCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var report = WorkflowReport.Create(
                versionId: dto.VersionId,
                name: dto.Name,
                fieldsConfigJson: dto.FieldsConfigJson,
                chartConfigJson: dto.ChartConfigJson,
                createdBy: userId
            );

            await _repository.SaveReportAsync(report);
            return true;
        }
    }
}