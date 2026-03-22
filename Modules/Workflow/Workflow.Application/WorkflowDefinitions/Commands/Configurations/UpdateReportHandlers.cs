using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateReportBasicCommandHandler : IRequestHandler<UpdateReportBasicCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReportBasicCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateReportBasicCommand request, CancellationToken cancellationToken)
        {
            var report = await _repository.GetReportByIdAsync(request.Id);
            if (report == null)
            {
                throw new NotFoundException("Không tìm thấy báo cáo.");
            }

            report.Update(
                name: request.Name,
                fieldsConfigJson: null,
                chartConfigJson: null,
                isActive: request.IsActive,
                modifiedBy: _currentUserService.UserId
            );

            await _repository.SaveReportAsync(report);
            return true;
        }
    }

    public class UpdateReportConfigCommandHandler : IRequestHandler<UpdateReportConfigCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReportConfigCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateReportConfigCommand request, CancellationToken cancellationToken)
        {
            var report = await _repository.GetReportByIdAsync(request.Id);
            if (report == null)
            {
                throw new NotFoundException("Không tìm thấy báo cáo.");
            }

            report.Update(
                name: null,
                fieldsConfigJson: request.FieldsConfigJson,
                chartConfigJson: request.ChartConfigJson,
                isActive: null,
                modifiedBy: _currentUserService.UserId
            );

            await _repository.SaveReportAsync(report);
            return true;
        }
    }
}