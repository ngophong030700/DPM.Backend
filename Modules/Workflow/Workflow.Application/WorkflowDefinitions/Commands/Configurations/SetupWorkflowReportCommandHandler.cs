using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowReportCommandHandler : IRequestHandler<SetupWorkflowReportCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public SetupWorkflowReportCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SetupWorkflowReportCommand request, CancellationToken cancellationToken)
        {
            var version = await _repository.GetVersionByIdAsync(request.VersionId);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản quy trình.");
            }

            var userId = _currentUserService.UserId;

            var report = WorkflowReport.Create(
                versionId: request.VersionId,
                name: request.Data.Name,
                fieldsConfigJson: request.Data.FieldsConfigJson,
                chartConfigJson: request.Data.ChartConfigJson,
                createdBy: userId
            );

            if (request.Data.Id.HasValue && request.Data.Id.Value > 0)
            {
                // Sử dụng Reflection hoặc method để set private Id (EF sẽ tracking theo Id này)
                typeof(WorkflowReport).GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(report, request.Data.Id.Value);
            }

            await _repository.SaveReportAsync(report);

            return true;
        }
    }
}