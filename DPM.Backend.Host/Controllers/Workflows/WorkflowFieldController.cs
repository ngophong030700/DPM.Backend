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
    [Route("v1/api/workflow-field")]
    [Tags("Workflow Field (Tab 2: Trường dữ liệu)")]
    public class WorkflowFieldController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowFieldController(IMediator mediator, IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("form-metadata")]
        [ProducesResponseType(typeof(EntityResponse<WorkflowFieldMetadataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFormMetadata()
        {
            var result = await _queryService.GetFieldMetadataAsync();
            return Ok(new EntityResponse<WorkflowFieldMetadataDto>(result, "Lấy metadata thành công."));
        }

        [HttpGet("get-list/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<List<FieldConfigDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(int versionId)
        {
            var data = await _queryService.GetFieldsByVersionIdAsync(versionId);
            return Ok(new EntityResponse<List<FieldConfigDto>>(data, "Lấy danh sách trường dữ liệu thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<FieldConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FieldConfigDto dto)
        {
            var result = await _mediator.Send(new CreateFieldCommand(dto));
            return Ok(new EntityResponse<FieldConfigDto?>(result, "Tạo trường dữ liệu thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<FieldConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] FieldConfigDto dto)
        {
            var result = await _mediator.Send(new UpdateFieldCommand(id, dto));
            return Ok(new EntityResponse<FieldConfigDto?>(result, "Cập nhật trường dữ liệu thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteFieldCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa trường dữ liệu thành công."));
        }

        [HttpPut("reorder")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reorder([FromBody] List<ReorderItemDto> orderings)
        {
            var result = await _mediator.Send(new ReorderFieldsCommand(orderings));
            return Ok(new EntityResponse<bool>(result, "Cập nhật thứ tự thành công."));
        }
    }
}