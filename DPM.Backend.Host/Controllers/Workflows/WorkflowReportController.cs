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
    [Route("v1/api/workflow-report")]
    [Tags("Workflow Report (Tab 5: Báo cáo)")]
    public class WorkflowReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowReportController(IMediator mediator, IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("get-list/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewWorkflowReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(int versionId)
        {
            var data = await _queryService.GetReportsByVersionIdAsync(versionId);
            return Ok(new EntityResponse<List<ViewWorkflowReportDto>>(data, "Lấy danh sách báo cáo thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SetupWorkflowReportDto dto)
        {
            // Popup 1: Create
            var result = await _mediator.Send(new SetupWorkflowReportCommand(0, dto));
            return Ok(new EntityResponse<bool>(result, "Tạo báo cáo thành công."));
        }

        [HttpPut("update-basic/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasic(int id, [FromBody] SetupWorkflowReportDto dto)
        {
            // Popup 1: Update name, etc.
            dto.Id = id;
            var result = await _mediator.Send(new SetupWorkflowReportCommand(0, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật thông tin báo cáo thành công."));
        }

        [HttpGet("get-config/{id}")]
        [ProducesResponseType(typeof(EntityResponse<SetupWorkflowReportDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfig(int id)
        {
            // Placeholder: Call query to get config by ReportId
            return Ok(new EntityResponse<SetupWorkflowReportDto>(new SetupWorkflowReportDto(), "Lấy cấu hình báo cáo thành công."));
        }

        [HttpPut("update-config/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateConfig(int id, [FromBody] SetupWorkflowReportDto dto)
        {
            // Popup 2: Update detailed configs
            dto.Id = id;
            var result = await _mediator.Send(new SetupWorkflowReportCommand(0, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật cấu hình báo cáo thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWorkflowReportCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa báo cáo thành công."));
        }
    }
}