using MediatR;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class ValidateWorkflowVersionCommandHandler : IRequestHandler<ValidateWorkflowVersionCommand, WorkflowValidationResultDto>
    {
        private readonly IWorkflowDefinitionRepository _repository;

        public ValidateWorkflowVersionCommandHandler(IWorkflowDefinitionRepository repository)
        {
            _repository = repository;
        }

        public async Task<WorkflowValidationResultDto> Handle(ValidateWorkflowVersionCommand request, CancellationToken cancellationToken)
        {
            var result = new WorkflowValidationResultDto();
            
            var version = await _repository.GetVersionByIdAsync(request.Id);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản quy trình.");
            }

            var steps = await _repository.GetStepsByVersionIdAsync(request.Id);
            if (steps == null || !steps.Any())
            {
                result.Errors.Add("Quy trình chưa có bước nào.");
                return result;
            }

            // 1. Kiểm tra node Start
            var startNodes = steps.Where(s => s.StepType == WorkflowStepType.Start).ToList();
            if (startNodes.Count == 0)
                result.Errors.Add("Quy trình thiếu bước 'Bắt đầu'.");
            else if (startNodes.Count > 1)
                result.Errors.Add("Quy trình không được có nhiều hơn một bước 'Bắt đầu'.");

            // 2. Kiểm tra node End
            var endNodes = steps.Where(s => s.StepType == WorkflowStepType.End).ToList();
            if (endNodes.Count == 0)
                result.Errors.Add("Quy trình thiếu bước 'Kết thúc'.");

            // 3. Kiểm tra tính kết nối (Reachability)
            if (startNodes.Count == 1)
            {
                var startNodeId = startNodes[0].Id;
                var visited = new HashSet<string>();
                var queue = new Queue<string>();
                queue.Enqueue(startNodeId);

                while (queue.Count > 0)
                {
                    var currentId = queue.Dequeue();
                    if (visited.Contains(currentId)) continue;
                    visited.Add(currentId);

                    var currentStep = steps.FirstOrDefault(s => s.Id == currentId);
                    if (currentStep != null)
                    {
                        foreach (var action in currentStep.Actions)
                        {
                            if (!string.IsNullOrEmpty(action.TargetStepId))
                                queue.Enqueue(action.TargetStepId);
                            
                            foreach (var rule in action.Rules)
                            {
                                if (!string.IsNullOrEmpty(rule.TargetStepId))
                                    queue.Enqueue(rule.TargetStepId);
                            }
                        }
                    }
                }

                // Node mồ côi (không đi tới được từ Start)
                var unreachableNodes = steps.Where(s => !visited.Contains(s.Id)).Select(s => s.Label).ToList();
                foreach (var name in unreachableNodes)
                {
                    result.Warnings.Add($"Bước '{name}' không thể đi tới được từ bước 'Bắt đầu'.");
                }

                // Kiểm tra xem có thể đi tới node End nào không
                var canReachEnd = endNodes.Any(e => visited.Contains(e.Id));
                if (!canReachEnd)
                {
                    result.Errors.Add("Quy trình không có đường dẫn nào đi tới bước 'Kết thúc'.");
                }
            }

            // 4. Kiểm tra node Task không có Actions (dead-end)
            foreach (var step in steps.Where(s => s.StepType == WorkflowStepType.Task))
            {
                if (!step.Actions.Any())
                {
                    result.Errors.Add($"Bước nghiệp vụ '{step.Label}' chưa cấu hình các nút xử lý (Actions).");
                }
            }

            return result;
        }
    }
}