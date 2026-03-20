using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.WorkflowDefinitions.Commands.Configurations;
using Workflow.Application.WorkflowDefinitions.Queries;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-layout")]
    [Tags("Workflow Layout (Tab 3: Giao diện)")]
    public class WorkflowLayoutController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowLayoutController(IMediator mediator, IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("get-detail/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<SetupWorkflowLayoutDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int versionId)
        {
            var data = await _queryService.GetLayoutByVersionIdAsync(versionId);
            return Ok(new EntityResponse<SetupWorkflowLayoutDto>(data!, "Lấy cấu hình giao diện thành công."));
        }

        [HttpPut("save/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Save(int versionId, [FromBody] SetupWorkflowLayoutDto dto)
        {
            var result = await _mediator.Send(new SetupWorkflowLayoutCommand(versionId, dto));
            return Ok(new EntityResponse<bool>(result, "Lưu cấu hình giao diện thành công."));
        }

        [HttpGet("preview/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Preview(int versionId)
        {
            var layout = await _queryService.GetLayoutByVersionIdAsync(versionId);
            return Ok(new EntityResponse<object>(new { layout, sampleData = new { } }, "Lấy dữ liệu xem trước thành công."));
        }
    }
}