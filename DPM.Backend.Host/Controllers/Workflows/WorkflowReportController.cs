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

        [HttpGet("get-config/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewWorkflowReportDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfig(int id)
        {
            var data = await _queryService.GetReportByIdAsync(id);
            return Ok(new EntityResponse<ViewWorkflowReportDto>(data, "Lấy cấu hình báo cáo thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SetupWorkflowReportDto dto)
        {
            var result = await _mediator.Send(new CreateReportCommand(dto));
            return Ok(new EntityResponse<bool>(result, "Tạo báo cáo thành công."));
        }

        [HttpPut("update-basic/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasic(int id, [FromBody] SetupWorkflowReportDto dto)
        {
            // Note: In a real app, we'd use a specific DTO for basic info. 
            // Reusing SetupWorkflowReportDto and taking what we need.
            var result = await _mediator.Send(new UpdateReportBasicCommand(id, dto.Name, true)); // Assuming true for isActive
            return Ok(new EntityResponse<bool>(result, "Cập nhật thông tin cơ bản báo cáo thành công."));
        }

        [HttpPut("update-config/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateConfig(int id, [FromBody] SetupWorkflowReportDto dto)
        {
            var result = await _mediator.Send(new UpdateReportConfigCommand(id, dto.FieldsConfigJson, dto.ChartConfigJson));
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