using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.WorkflowDefinitions.Commands.Versions;
using Workflow.Application.WorkflowDefinitions.Queries;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-version")]
    [Tags("Workflow Version (Phiên bản quy trình)")]
    public class WorkflowVersionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowVersionController(IMediator mediator, IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("get-list/{workflowId}")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewWorkflowVersionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(int workflowId)
        {
            var data = await _queryService.GetByIdAsync(workflowId);
            return Ok(new EntityResponse<List<ViewWorkflowVersionDto>>(data?.Versions ?? new(), "Lấy danh sách phiên bản thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewWorkflowVersionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowVersionDto dto)
        {
            var result = await _mediator.Send(new CreateWorkflowVersionCommand(dto.WorkflowId, dto));
            return Ok(new EntityResponse<ViewWorkflowVersionDto>(result, "Tạo phiên bản mới thành công."));
        }

        [HttpPost("clone/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Clone(int id, [FromBody] CloneWorkflowVersionDto dto)
        {
            var result = await _mediator.Send(new CloneWorkflowVersionCommand(id, dto));
            return Ok(new EntityResponse<bool>(result, "Sao chép phiên bản thành công."));
        }

        [HttpPost("activate/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _mediator.Send(new ActivateWorkflowVersionByIdCommand(id));
            return Ok(new EntityResponse<bool>(result, "Kích hoạt phiên bản thành công."));
        }

        [HttpPost("validate/{id}")]
        [ProducesResponseType(typeof(EntityResponse<WorkflowValidationResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Validate(int id)
        {
            var result = await _mediator.Send(new ValidateWorkflowVersionCommand(id));
            var message = result.IsValid ? "Phiên bản hợp lệ." : "Phiên bản có lỗi logic.";
            return Ok(new EntityResponse<WorkflowValidationResultDto>(result, message));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWorkflowVersionCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa phiên bản thành công."));
        }
    }
}