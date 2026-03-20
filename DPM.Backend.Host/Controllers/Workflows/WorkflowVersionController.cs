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
    [Tags("Workflow Version (Tab 6: Phiên bản)")]
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
            var workflow = await _queryService.GetByIdAsync(workflowId);
            return Ok(new EntityResponse<List<ViewWorkflowVersionDto>>(workflow?.Versions ?? new List<ViewWorkflowVersionDto>(), "Lấy lịch sử phiên bản thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewWorkflowVersionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowVersionDto dto)
        {
            var result = await _mediator.Send(new CreateWorkflowVersionCommand(dto.WorkflowId, dto));
            return Ok(new EntityResponse<ViewWorkflowVersionDto>(result!, "Tạo phiên bản nháp thành công."));
        }

        [HttpPost("clone/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewWorkflowVersionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Clone(int id, [FromBody] CloneWorkflowVersionDto dto)
        {
            // Placeholder: Call Clone Command
            return Ok(new EntityResponse<ViewWorkflowVersionDto>(new ViewWorkflowVersionDto(), "Sao chép phiên bản thành công."));
        }

        [HttpPost("activate/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Activate(int id)
        {
            // Placeholder: Call Activate Command (requires WorkflowId, can fetch inside handler)
            var result = await _mediator.Send(new ActivateWorkflowVersionCommand(0, id));
            return Ok(new EntityResponse<bool>(result, "Kích hoạt phiên bản thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            // Placeholder: Call Delete Command
            return Ok(new EntityResponse<bool>(true, "Xóa phiên bản nháp thành công."));
        }
    }
}